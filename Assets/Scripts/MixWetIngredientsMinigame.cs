﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MixWetIngredientsMinigame : MonoBehaviour {

    [Header("Minigame parameters")]
    public float timeBeforePointingHand = 1f;
    public int cracksNeeded = 1;
    public float crackForceThreshold = 0f;
    public int eggsNeeded = 2;
    public float milkNeeded = 10f;

    public Sprite[] eggSprites;


    [Space(25)]
    [Header("External references")]
    public PointerControl pointer;
    public Transform eggsTarget;
    public Transform bowlBorderTarget;
    public Transform hoverMarkTarget;
    public GameObject hands;
    public Animator[] handsAnimators;
    public GameObject crackedEgg;
    public EggDrag[] eggs;
    public GameObject milk;
    public JugControl jug;
    public PromptControl eggPromptControl;
    
    
    [HideInInspector]
    public float milkPoured = 0f;
    private Vector2 eggPosition;
    private bool draggingPhase = true;
    private int cracks = 0;
    private bool blockCalls = false;
    private bool calledOnce = false;
    private int eggsOpened = 0;



    private void Start() {
        eggs[0].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        for (int i = 1; i < eggs.Length; i++) {
            eggs[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        eggPromptControl.ShowPromptAfter(1, 3, StartMinigame);
    }

    public void StartMinigame() {
        FindObjectOfType<InputManager>().SetMultitouch(true);
        eggPosition = eggsTarget.position;
        SetPointer(eggPosition);
        eggs[0].gameObject.GetComponent<BoxCollider2D>().enabled = true;

    }

    public void StartDraggingEgg() {
        if (draggingPhase) {
            SetPointer(bowlBorderTarget.position);
        }
    }

    public void EndDraggingEgg() {
        if (draggingPhase) {
            SetPointer(eggPosition);
        }
    }

    public void CrackEgg(EggDrag egg) {
        //Debug.Log("Detected that! " + egg.velocity);
        cracks++;
        egg.GetComponentInChildren<SpriteRenderer>().sprite = eggSprites[Math.Min(cracks, eggSprites.Length - 1)];
        egg.GetComponentInChildren<SpriteRenderer>().transform.localEulerAngles = new Vector3(0,0,90);
        if (cracks == cracksNeeded) {
            draggingPhase = false;
            blockCalls = true;
            egg.CancelDrag();
            egg.MoveAndRotateTo(hoverMarkTarget.position, Quaternion.Euler(egg.transform.rotation.x, egg.transform.rotation.y, 90f), true, EnableCrackedEgg);
            pointer.Hide();
            StartEggCrackHandsAnimation();
        }
    }

    public void EnableCrackedEgg() {
        crackedEgg.SetActive(true);
    }

    private void StartEggCrackHandsAnimation() {
        //hands.SetActive(true);
        foreach (Animator animator in handsAnimators) {
            animator.Play("Animation");
        }
    }

    private void SetPointer(Vector3 position) {
        pointer.Hide();
        pointer.PointTo(position);
        StartCoroutine(CallAfter(timeBeforePointingHand, pointer.Show));
    }

    public void SeparatingEgg(bool separating) {
        //hands.SetActive(!separating);
        if (!separating) StartEggCrackHandsAnimation();
    }

    public void SeparatingEggCompleted() {
        //hands.SetActive(false);

        foreach (Rigidbody2D sec in crackedEgg.GetComponentsInChildren<Rigidbody2D>()) {
            GameObject go = Instantiate(sec.gameObject);
            go.transform.parent = crackedEgg.transform.parent;
            go.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            go.transform.position = sec.transform.position;
            go.transform.localScale = sec.transform.localScale;
        }

        crackedEgg.SetActive(false);
        if ((++eggsOpened) == eggsNeeded) {
            eggPromptControl.Hide();
            MilkStep();
            return;
        }

        NextEgg();
    }

    private void NextEgg() {
        cracks = 0;




        draggingPhase = true;
        if (eggsOpened < eggsNeeded) {
            eggs[eggsOpened].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            blockCalls = false;
            SetPointer(eggPosition);
        }
    }

    private void MilkStep() {
        milk.GetComponent<Collider2D>().enabled = true;
        blockCalls = false;
        SetPointer(milk.transform.position);
    }

    public void HoverMilk() {
        pointer.Hide();
        blockCalls = true;
        milk.GetComponent<MilkControl>().Hover();
        jug.Show();
        jug.EnableDrag();
    }

    public void StartDraggingJug() {
        milk.GetComponent<MilkControl>().HideMilk();
    }

    public void EndDraggingJug() {
        milk.GetComponent<MilkControl>().Hover();
    }

    private IEnumerator CallAfter(float seconds, Action function) {
        float start = Time.time;
        while (Time.time - start < seconds) {
            yield return false;
        }
        if (!blockCalls) {
            function();
        }
    }



}

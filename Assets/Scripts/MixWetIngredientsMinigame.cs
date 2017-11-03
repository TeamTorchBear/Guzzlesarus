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


    private Vector2 eggPosition;
    private bool draggingPhase = true;
    private int cracks = 0;
    private bool blockCalls = false;
    private bool calledOnce = false;
    private int eggsOpened = 0;



    private void Start() {
        eggPosition = eggsTarget.position;
        SetPointer(eggPosition);
        for (int i = 1; i < eggs.Length; i++) {
            eggs[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
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
        if ((++cracks) == cracksNeeded) {
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
        hands.SetActive(true);
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
        hands.SetActive(!separating);
        if (!separating) StartEggCrackHandsAnimation();
    }

    public void SeparatingEggCompleted() {
        if (!calledOnce) {
            calledOnce = true;
            return;
        }

        hands.SetActive(false);

        if ((++eggsOpened) == eggsNeeded) {
            MilkStep();
            return;
        }

        NextEgg();
    }

    private void NextEgg() {
        cracks = 0;
        calledOnce = false;
        foreach (SeparateEggControl sec in crackedEgg.GetComponentsInChildren<SeparateEggControl>()) {
            sec.Reset();
        }
        crackedEgg.SetActive(false);
        draggingPhase = true;
        if (eggsOpened < eggsNeeded) {
            eggs[eggsOpened].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            blockCalls = false;
            SetPointer(eggPosition);
        }
    }

    private void MilkStep() {
        // TODO: Show hand pointing to the milk
        // TODO: Tap de milk
        blockCalls = false;
        SetPointer(milk.transform.position);

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

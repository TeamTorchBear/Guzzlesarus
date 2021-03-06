﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourControl : MonoBehaviour
{
    public bool pouring;
    public float pourTime;
    public GameObject pancakePrefabs;
    public GameObject pancake;
    public Sprite bowl1, bowl2;
    public Transform pancakePos;
    public Sprite[] bowlSheet;

    private bool isPromptFinish = false;
    private PromptControl prompt;
    // Use this for initialization
    void Start()
    {
        //bowlSheet = new Sprite[43];
        pourTime = 0;
        prompt = FindObjectOfType<PromptControl>();
        prompt.ShowPromptAfter(0, 4, () =>
        {
            Debug.Log("Closed");
            isPromptFinish = true;
        }, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPromptFinish)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                StartPour();
                pourTime += Time.deltaTime;
                
            }
            else
            {
                StopPour();
            }
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began ||
                    Input.GetTouch(0).phase == TouchPhase.Stationary ||
                    Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    StartPour();
                    pourTime += Time.deltaTime;
                }
                else
                {
                    StopPour();
                }
            }
        }
    }

    void StartPour()
    {
        int i = (int)(pourTime / 0.1f);
        this.GetComponent<SpriteRenderer>().sprite = bowlSheet[i];
        Debug.Log("Start Pouring");
        if (pourTime >= 1.2)
        {
            if (!pouring)
            {
                Debug.Log("Pouring once");
                //HERE================
                //Only called once===================
                AkSoundEngine.PostEvent("Stop_Sizzle", gameObject);
                AkSoundEngine.PostEvent("Pan_Sizzle", gameObject);
                AkSoundEngine.SetRTPCValue("SizzleVolume", 60f, null, 300);
            }
            pouring = true;
            pancake.transform.localScale = new Vector3((pourTime-1.2f) / 3f * 1.2f, (pourTime - 1.2f) / 3f * 1.2f, 1);
        }
        if (pourTime >= 4.2f)
        {
            //stop animation
            pancake.transform.localScale = new Vector3(1.2f, 1.2f, 1);
            this.GetComponent<BowlLeave>().enabled = true;
            this.GetComponent<PourControl>().enabled = false;
            this.GetComponent<SpriteRenderer>().sprite = bowl1;
            prompt.GetComponent<Transform>().position = new Vector3(0, 0, -11);
        }
    }

    void StopPour()
    {
        if (pouring)
        {
            Debug.Log("Stop pouring once");
            //HERE================
            //Only called once=====================
        }
        pouring = false;
        //this.GetComponent<SpriteRenderer>().sprite = bowl1;
        Debug.Log("Stop Pouring");
        /*Pause Animation
       *
       * PAUSE ANIMATION
       * 
       * 
       */
    }

}

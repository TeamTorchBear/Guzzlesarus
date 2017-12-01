﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReplacePancake : MonoBehaviour
{
    private enum FRY_STATE 
    {
        WAIT,
        DO_IT,
        THE_OTHER_ONE
    }


    public Sprite pancakel1, pancakel2, pancakel3, pancakel4;
    public GameObject pancake;
    public GameObject Light;

    public Image Screen;
    public SpriteRenderer spriteRenderer;


    public Animator feedbackGuzz;

    private Animator animator;
    private PromptControl promt;
    private float fryTime = 0;
    private int checkPancakeReplaceTimes = 0;
    private bool isPromptFinish = false;
    private bool isCalledPrompt = false;
    private bool isEnd = false;
    private bool quit = false;
    private int state = 1;

    private FRY_STATE fryState = FRY_STATE.THE_OTHER_ONE;

    // Use this for initialization
    void Start()
    {
        
        animator = this.GetComponentInChildren<Animator>();
        promt = FindObjectOfType<PromptControl>();
        pancake = GameObject.FindGameObjectWithTag("Minigame4Pancake");
        //pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel1;
        Light.SetActive(true);
        Debug.Log("timetocook!");
        AkSoundEngine.PostEvent("CookPrompt", gameObject);
        AkSoundEngine.SetRTPCValue("MiniGame3Finish", 60f, GameObject.FindGameObjectWithTag("MainCamera"), 200);
        Light.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        //promt.ShowPromptAfter(0, 2, () =>
        //{
        //    Debug.Log("Closed");
        //    isPromptFinish = true;
        //    isCalledPrompt = true;
        //}, true);
        isCalledPrompt = true;
        promt.Hide(() =>
        {
            
            promt.SetContent(promt.GetComponentsInChildren<Transform>(true)[5].gameObject);
            promt.PlayAnimations();
            promt.ShowPromptAfter(0, 4, () =>
            {
                isPromptFinish = true;
            }, true);
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnd)
        {
            if (isPromptFinish)
            {
                fryTime += Time.deltaTime;
            }
            if (fryTime >= 4)
            {
                FindObjectOfType<PanShake>().enabled = true;
                Replace();
                if (fryTime >= 4 && fryTime < 6)
                {
                    if (fryState != FRY_STATE.WAIT)
                    {
                        fryState = FRY_STATE.WAIT;
                        Debug.Log("WaitForIt");
                        AkSoundEngine.PostEvent("WaitForIt", gameObject);
                        AkSoundEngine.SetRTPCValue("SizzleVolume", 80f, null, 200);
                        AkSoundEngine.SetRTPCValue("SizzlePitch", 100f, null, 200);
                    }
                    Light.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    FindObjectOfType<PanShake>().deltaRotation = 20;
                }
                else if (fryTime >= 6 && fryTime < 7)
                {
                    if (fryState != FRY_STATE.DO_IT) 
                    {
                        fryState = FRY_STATE.DO_IT;    
                        Debug.Log("FlipIT");
                        AkSoundEngine.PostEvent("FlipIt", gameObject);
					}
                    Light.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                    FindObjectOfType<PanShake>().deltaRotation = 100;
                }
                else if (fryTime >= 7)
                {
                    if (fryState != FRY_STATE.THE_OTHER_ONE) 
                    {
                        fryState = FRY_STATE.THE_OTHER_ONE;
                        Light.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                        FindObjectOfType<PanShake>().deltaRotation = 200;
                    }
                }

                if (!FindObjectOfType<VibrateControl>().enabled)
                { 
                    ReplaceSprites();
                    fryTime = 0;
                    checkPancakeReplaceTimes = 0;
                    FindObjectOfType<PanShake>().enabled = false;
                    FindObjectOfType<PanShake>().GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
                    if (pancake.GetComponentInChildren<SpriteRenderer>().sprite == pancakel4)
                    {
                        
                        End();
                    }
                }
            }
            else
            {
                Light.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
        }
        if (quit)
        {
            ScreenFadeOut("ResultScreen");
        }
    }

    private void Replace()
    {
        if (checkPancakeReplaceTimes == 0)
        { 
        //    if (pancake.GetComponentInChildren<SpriteRenderer>().sprite == pancakel1)
        //        pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel2;
        //    else if (pancake.GetComponentInChildren<SpriteRenderer>().sprite == pancakel2)
        //        pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel3;
        //    else if (pancake.GetComponentInChildren<SpriteRenderer>().sprite == pancakel3)
        //        pancake.GetComponentInChildren<SpriteRenderer>().sprite = pancakel4;
        //Animation.
            FindObjectOfType<VibrateControl>().enabled = true;
        }
        checkPancakeReplaceTimes++;
    }

    private void End()
    {
        isEnd = true;

        feedbackGuzz.Play("GuzzMoveOut");
        promt.Hide(() =>
        {

            promt.SetContent(promt.GetComponentsInChildren<Transform>(true)[10].gameObject);
            promt.PlayAnimations();
            AkSoundEngine.PostEvent("Finished", gameObject);
            AkSoundEngine.SetRTPCValue("SizzleVolume", 0f, null, 25);
            AkSoundEngine.SetRTPCValue("MiniGame3Finish", 0f, GameObject.FindGameObjectWithTag("MainCamera"), 200);
            promt.ShowPromptAfter(2, 4, () =>
            {
                quit = true;
            }, true);
        });
        Debug.Log("End");
    }

    void ScreenFadeOut(string scene)
    {
        FindObjectOfType<ScreenFadeIn>().enabled = false;
        if (Screen != null)
        {

            Screen.gameObject.SetActive(true);
            if (Screen.color.a < 1.0f)
            {
                Screen.color = new Color(Screen.color.r, Screen.color.g, Screen.color.b, Screen.color.a + 0.02f);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }
        else if (spriteRenderer != null)
        {
            spriteRenderer.gameObject.SetActive(true);
            if (spriteRenderer.color.a < 1.0f)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.02f);
            }
            else
            {
                SceneManager.LoadScene(scene);
            }
        }
    }

    void ReplaceSprites()
    {
        //play animation
        if (state <= 4)
            animator.Play("Animation" + state);
        else
            animator.Play("Animation4");
        state++;
    }
}

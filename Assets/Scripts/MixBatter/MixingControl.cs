﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct State {
    public int cycles;
    public Sprite sprite;
}
public class MixingControl : Interactable {

    [Header("Balancing Parameters")]
    public float minCycleTime = 0.6f;
    public float maxCycleTime = 2f;
    public int cyclesToComplete = 35;

    public State[] statesList;

    [Header("Dev Parameters")]
    public Transform debugMark;
    public Transform spoonTransform;
    public float multiplier = 0.02f;

    [SerializeField]
    private Collider2D outCollider;
    [SerializeField]
    private Collider2D inCollider;

    public float angleError = 10f;
    private int cyclesCompleted = 0;
    private int stateCyclesCompleted = 0;
    public SpriteRenderer spriteRenderer;

    private Transform spriteTransform;
    private float lx;
    private float ly;

    private bool mixing = false;
    private float speed = 0f;

    private Vector2 v0 = new Vector2();
    private Vector2 v1 = new Vector2();
    private float v_angle;
    private float s_angle = 0f;
    private bool cycleDone = false;
    private float cycleStartTime;
    private int currentState;


    public override void OnStart() {
        base.OnStart();
        currentState = 0;
        spriteRenderer.sprite = statesList[currentState].sprite;
        spriteTransform = spoonTransform.GetComponentInChildren<SpriteRenderer>().transform;
    }

    public override void OnInteractionStart(Vector3 position) {
        Vector2 touchPos = ScreenToWorldTouch(position);
        if (outCollider.OverlapPoint(touchPos) && !inCollider.OverlapPoint(touchPos)) {
            // The touch was made inside the desired area
            mixing = true;
            lx = touchPos.x;
            ly = touchPos.y;

            v0 = new Vector2(lx, ly).normalized;

            cycleStartTime = Time.time;


        }
    }


    public override void OnInteractionHold(Vector3 position) {
        if (!mixing) {
            return;
        }
        Vector2 touchPos = ScreenToWorldTouch(position);
        if (!(outCollider.OverlapPoint(touchPos) && !inCollider.OverlapPoint(touchPos))) {
            mixing = false;
            //Debug.Log("PointOutOfBounds");
            return;
        }

        speed = (Vector2.Distance(touchPos, new Vector2(lx, ly))) / Time.deltaTime;

        if ((touchPos.y > 0.0 && touchPos.x < (lx - (multiplier * speed))) ||
           (touchPos.y < -0.0 && touchPos.x > (lx + (multiplier * speed))) ||
           (touchPos.x > 0.0 && touchPos.y > (ly + (multiplier * speed))) ||
           (touchPos.x < -0.0 && touchPos.y < (ly - (multiplier * speed)))) {

            //Debug.Log("Not that way! \nLast position (" + lx + ", " + ly + ")\nCurrent Position (" + touchPos.x + ", " + touchPos.y + ")\nSpeed: " + speed);
            mixing = false;
            return;

        }

        lx = touchPos.x;
        ly = touchPos.y;

        debugMark.position = touchPos;

        v1 = new Vector2(lx, ly).normalized;

        v_angle = Vector2.SignedAngle(v1, v0);
        if (v_angle < 0) {
            v_angle = 360 + v_angle;
        }

        float delta = -(s_angle + v_angle);
        float x = Mathf.Cos(delta * Mathf.Deg2Rad) * 3f;
        float y = Mathf.Sin(delta * Mathf.Deg2Rad) * 0.8f;

        spriteTransform.localPosition = new Vector3(x, y, 0);

        if (v_angle > (360 - angleError)) {
            if (!cycleDone) {
                cycleDone = true;
                float cycleTime = Time.time - cycleStartTime;
                cycleStartTime = Time.time;
                CompleteCycle(cycleTime);
            }
        } else {
            cycleDone = false;
        }

    }
    public override void OnInteractionEnd(Vector3 position) {
        mixing = false;
        cycleDone = false;

        s_angle += v_angle;
        if (s_angle < 0) {
            s_angle = 360 + s_angle;
        }
    }

    private void CompleteCycle(float time) {
        Debug.Log("Cycle done in " + time + "s");

        if (time > maxCycleTime) {
            Debug.Log("Too slow!");
        } else if (time < minCycleTime) {
            Debug.Log("Too fast!");
        } else {
            cyclesCompleted++;
            stateCyclesCompleted++;
            if (stateCyclesCompleted >= statesList[currentState].cycles) {
                currentState++;
                stateCyclesCompleted = 0;
                if (currentState == statesList.Length) {
                    Debug.Log("COMPLETE!");
                } else {
                    spriteRenderer.sprite = statesList[currentState].sprite;
                }
            }
        }
    }
}
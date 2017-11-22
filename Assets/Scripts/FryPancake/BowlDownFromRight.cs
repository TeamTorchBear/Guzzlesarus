﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlDownFromRight : MonoBehaviour
{
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    // Use this for initialization
    void Start()
    {
        //this.transform.position = startMarker.position;
        startTime = Time.time;
        journeyLength = Vector2.Distance(startMarker.position, endMarker.position);
    }

    // Update is called once per frame
    void Update()
    {

        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        this.transform.position = Vector2.Lerp(startMarker.position, endMarker.position, fracJourney);
        if (this.transform.position.x == endMarker.position.x)
        {
            this.GetComponent<BowlDownFromRight>().enabled = false;
        }

    }
}

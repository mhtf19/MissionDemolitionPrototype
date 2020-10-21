﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in Inspector")]
    public float easing = .05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;

    void Awake(){
        camZ = this.transform.position.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(POI == null) return;
        Vector3 destination = POI.transform.position;

        //limit x and y to min
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //interpolate from current camera pos towards destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;

        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }
}

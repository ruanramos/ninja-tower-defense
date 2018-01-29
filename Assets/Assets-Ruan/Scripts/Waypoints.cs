using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {

    public static Transform[] yellowWaypoints;

    private void Awake()
    {
        yellowWaypoints = new Transform[transform.childCount];
        for (int i = 0; i < yellowWaypoints.Length; i++)
        {
            yellowWaypoints[i] = transform.GetChild(i);
        }
    }

}

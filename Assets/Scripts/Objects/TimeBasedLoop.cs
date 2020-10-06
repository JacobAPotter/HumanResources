using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBasedLoop : MonoBehaviour
{
    TimeManager timeManager;
    float theta;
    float radius;

    private void Start()
    {
        timeManager = GameManager.ActiveGameManager.TimeManager;
        radius = Vector3.Distance(transform.position, transform.GetChild(0).position);
    }

    private void Update()
    {
        theta += timeManager.WorldDeltaTime;

        transform.GetChild(0).position = transform.position +
                                          new Vector3(Mathf.Cos(theta) * radius, 0, Mathf.Sin(theta) * radius);
    }

}


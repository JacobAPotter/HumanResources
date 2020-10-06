using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearThing : MonoBehaviour
{
    Transform gear;
    Transform thing;
    Vector3 gearCenter;
    TimeManager timeManager;
    float radius;
    float speed = 2f;

    void Start()
    {
        gear = transform.Find("gear");
        radius = gear.GetComponent<Renderer>().bounds.extents.x * 0.4f;
         
        thing = transform.Find("thing");
        gearCenter = thing.position;

        timeManager = GameManager.ActiveGameManager.TimeManager;
    }

    void Update()
    {
        float theta = timeManager.WorldTime * speed;

        thing.position = gearCenter + new Vector3(Mathf.Cos(theta),
                                                 Mathf.Sin(theta),
                                                   0) * radius;

        gear.localRotation = Quaternion.Euler(270, -theta * Mathf.Rad2Deg, 0);
    }
}

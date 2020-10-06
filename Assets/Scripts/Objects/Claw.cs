using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    Transform[] arms;
    const float maxRot = 30;

    Vector3[] min = {new Vector3(270,0,270),
                     new Vector3(0,90,180),
                     new Vector3(90,0,90),
                    new Vector3(0,270,0)};


    Vector3[] max = {new Vector3(270 + maxRot,0,270),
                     new Vector3(0,90 - maxRot,180),
                     new Vector3(90 - maxRot,0,90),
                    new Vector3(0,270 + maxRot,0)};
    bool occupied;

    void Start()
    {
        arms = new Transform[4];
        for(int i =0; i< arms.Length; i++)
            arms[i] = transform.Find("arm" + (i+1).ToString());

        SetOccupied(transform.Find("body") != null);
    }

    public void SetOccupied(bool occupied)
    {
        this.occupied = occupied;
        if (occupied)
            for (int i = 0; i < 4; i++)
                arms[i].localRotation = Quaternion.Euler(max[i]);
        else
            for (int i = 0; i < 4; i++)
                arms[i].localRotation = Quaternion.Euler(min[i]);
    }

    public bool Occupied
    {
        get { return occupied; }
    }
}

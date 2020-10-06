using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAffectedRigidbody : MonoBehaviour
{
    Rigidbody rigid;
    float previousCoef = 1;
    //This object is affected by time dilation.
    float startingMass;
    TimeManager timeManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        previousCoef = 1;
    }
    private void Start()
    {
        startingMass = rigid.mass;
        timeManager = GameManager.ActiveGameManager.TimeManager;
    }

    private void Update()
    {
        if (timeManager.Coefficient < 0.001f)
        {
            return;
        }

        rigid.velocity /= previousCoef;
        rigid.mass *= previousCoef;
        rigid.angularVelocity /= previousCoef;

        //At this point the rigid body values are exactly as if  no time orbs ever existed.

        float timeCoef = timeManager.Coefficient;
        rigid.velocity *= timeCoef;
        rigid.mass /= timeCoef;
        rigid.angularVelocity *= timeCoef;

        previousCoef = timeCoef;
    }

    public void ResetTimeAffected()
    {
        rigid.velocity = Vector3.zero;
        rigid.mass = startingMass;
        rigid.angularVelocity = Vector3.zero;
        previousCoef = 1;
    }
}

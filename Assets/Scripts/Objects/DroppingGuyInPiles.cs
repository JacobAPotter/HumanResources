using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppingGuyInPiles : MonoBehaviour
{
    Vector3 clawStart;
    Vector3 clawEnd;
    Vector3 guyStart;

    [SerializeField]
    Vector3 guyDrop;

    [SerializeField]
    Vector3 topOfPile;

    float speed = 1f;

    Transform claw;
    Transform guy;
    float timeStamp;
    TimeManager timeManager;
    const float dropTime = 0.9f;

    private void Start()
    {
        claw = transform.Find("claw");
        guy = transform.Find("guy");
        timeManager = GameManager.ActiveGameManager.TimeManager;
        timeStamp = timeManager.WorldTime;
        clawStart = claw.position;
        guyStart = guy.position;
        //go past the drop point by 1/0.75
        clawEnd = clawStart + ((guyDrop - guyStart) / dropTime);
    }

    private void Update()
    {
        float t = ((timeManager.WorldTime - timeStamp)/5f) % 1f;

        if (t < dropTime)
            guy.position = Vector3.Lerp(guyStart, guyDrop, t / dropTime);
        else
            guy.position = Vector3.Lerp(guyDrop, topOfPile, (t - dropTime) * 1f/(1f- dropTime));

        claw.position = Vector3.Lerp(clawStart, clawEnd, t);
    }
}

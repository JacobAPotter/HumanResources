using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour
{
    Bounds bounds;

    [SerializeField]
    float orthoSize;

    [SerializeField]
    bool moveToPoint;

    Transform movePoint;

    private void Start()
    {
        bounds = GetComponent<Renderer>().bounds;
        GetComponent<Renderer>().enabled = false;
        GameManager.ActiveGameManager.CameraZoneManager.AddZone(this);
        if (GetComponent<Collider>())
            GetComponent<Collider>().enabled = false;

        movePoint = transform.Find("p");
    }

    public Bounds Bounds
    {
        get { return bounds; }
    }

    public float OrthoSize
    {
        get { return orthoSize; }
    }

    public bool MoveToPoint
    {
        get { return moveToPoint; }
    }
    public Transform MovePoint
    {
        get { return movePoint; }
    }


}

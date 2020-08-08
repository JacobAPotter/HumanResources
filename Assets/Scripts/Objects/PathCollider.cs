using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCollider : MonoBehaviour
{
    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        GameManager.ActiveGameManager.ColliderManager.AddCollider(this);
    }

    public Bounds ColliderBounds
    {
        get { return col.bounds; }
    }
 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.ActiveGameManager.ChairManager.AddChair(transform.position);
        this.enabled = false;
    }
}

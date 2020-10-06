using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    public Bounds Bounds { get; private set; }
    public string nextLevel;


    private void Start()
    {
        Bounds = GetComponent<Renderer>().bounds;
        GetComponent<Renderer>().enabled = false;
    }
}

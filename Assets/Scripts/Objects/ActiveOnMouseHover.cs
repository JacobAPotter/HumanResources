using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnMouseHover : MonoBehaviour
{
    StateManager stateManager;
    private void Start()
    {
        stateManager = GameManager.ActiveGameManager.StateManager;
    }

    private void Update()
    {
        Vector3 point;
        bool active = false;
        if(stateManager.RaycastMouse(out point))
        {
            if (Vector3.Distance(point, transform.position) < StateManager.maxDistFromPlayerForDraw)
                active = true;
        }

        for(int i =0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(active);

    }
}

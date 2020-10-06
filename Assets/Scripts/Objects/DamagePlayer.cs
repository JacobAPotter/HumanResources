using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    Health playerHealth;
    Bounds bounds;
    Renderer largestRenderer;

    void Start()
    {
        playerHealth = GameManager.ActiveGameManager.Player.GetComponent<Health>();

        bounds = new Bounds();

        if (GetComponent<Renderer>())
        {
            bounds = GetComponent<Renderer>().bounds;
            largestRenderer = GetComponent<Renderer>();
        }

        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).GetComponent<Renderer>())
            {
                if (transform.GetChild(i).GetComponent<Renderer>().bounds.size.magnitude > bounds.size.magnitude)
                {
                    bounds = transform.GetChild(i).GetComponent<Renderer>().bounds;
                    largestRenderer = transform.GetChild(i).GetComponent<Renderer>();
                }
            }
    }

    void Update()
    {
        bounds = largestRenderer.bounds;
        bounds.size = new Vector3(bounds.size.x, 10, bounds.size.z);

        if (bounds.Contains(playerHealth.transform.position))
            playerHealth.DamageHealth(1);
    }
}

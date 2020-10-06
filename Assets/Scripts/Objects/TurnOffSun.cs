using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffSun : MonoBehaviour
{
    Bounds bounds;
    Player player;
    bool insideBounds;

    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        bounds = rend.bounds;
        rend.enabled = false;
        player = GameManager.ActiveGameManager.Player;
    }

    private void Update()
    {
        if (!insideBounds)
        {
            if (bounds.Contains(player.transform.position))
            {
                insideBounds = true;
                GameManager.ActiveGameManager.Sun.enabled = false;
            }
        }
        else
        {
            if (!bounds.Contains(player.transform.position))
            {
                insideBounds = false;
                GameManager.ActiveGameManager.Sun.enabled = true;
            }
        }
    }
}

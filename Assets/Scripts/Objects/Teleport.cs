using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    Transform teleportTo;
    Player player;
    Bounds bounds;

    void Start()
    {
        teleportTo = transform.Find("t");
        player = GameManager.ActiveGameManager.Player;
        bounds = GetComponent<Renderer>().bounds;
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        if (bounds.Contains(player.transform.position))
        {
            player.transform.position = teleportTo.position;

            GameManager.ActiveGameManager.MainFemale.SetPosition(
                player.transform.position + new Vector3(2f,0,2f));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOrb : MonoBehaviour
{
    Player player;
    PowerManager powerManager;

    private void Start()
    {
        player = GameManager.ActiveGameManager.Player;
        powerManager = GameManager.ActiveGameManager.PowerManager;
    }

    void Update()
    {
        Vector3 diff = transform.position - player.transform.position;

        if(diff.sqrMagnitude < 1f)
        {
            powerManager.ResetPower();
        }
    }
}

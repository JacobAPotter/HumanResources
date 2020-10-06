using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSerum : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameManager.ActiveGameManager.Player;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2)
        {
            LevelManager.PlayerGotPowers();
            gameObject.SetActive(false);
        }
    }
}

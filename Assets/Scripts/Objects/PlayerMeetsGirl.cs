using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeetsGirl : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameManager.ActiveGameManager.Player;
    }

    void Update()
    {
            if (Vector3.Distance(player.transform.position, transform.position) < 3f)
            {
                LevelManager.PlayerMetGirl();
                gameObject.SetActive(false);
            }
                
    }
}

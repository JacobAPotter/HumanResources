using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageFromPlayer : InteractWithPlayer
{
    Health health;

    protected override void Start()
    {
        base.Start();
        health = GetComponent<Health>();
    }

    protected override void Interact()
    {
        base.Interact();

        if(GameManager.ActiveGameManager.StateManager.CURRENT_STATE == StateManager.PLAYER_STATE.DASH ||
            GameManager.ActiveGameManager.StateManager.CURRENT_STATE == StateManager.PLAYER_STATE.DRAW)
        {
            GameManager.ActiveGameManager.FXManager.GetOno(transform.position + Vector3.up * 3);
            GameManager.ActiveGameManager.FXManager.GetBloodExplosion(transform.position);
            health.DamageHealth(4);        
        }
    }
}

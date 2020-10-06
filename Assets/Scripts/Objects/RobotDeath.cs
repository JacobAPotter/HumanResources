using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDeath : Death
{
    public override void Die()
    {
        GameManager.ActiveGameManager.FXManager.GetChips(transform.position, 4);
        GameManager.ActiveGameManager.FXManager.GetExplosion(transform.position + Vector3.up);
        GameManager.ActiveGameManager.FXManager.GetBurn(transform.position);

        gameObject.SetActive(false);
    }
}


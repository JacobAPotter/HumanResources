using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateDeath : Death
{
    public override void Die()
    {
        GameManager.ActiveGameManager.FXManager.GetDebri(transform.position, 4);
        gameObject.SetActive(false);
    }
}

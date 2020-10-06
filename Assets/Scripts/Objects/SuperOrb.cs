using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperOrb : InteractWithPlayer
{
    PowerManager powerManager;

    protected override void Start()
    {
        base.Start();
        powerManager = GameManager.ActiveGameManager.PowerManager;
    }

    protected override void Interact()
    {
            if (powerManager.PowerUnits < powerManager.MaxPowerUnits)
                powerManager.ResetPower();
    }
}

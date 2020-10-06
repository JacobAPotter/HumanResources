using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOrb : InteractWithPlayer
{
    PowerManager powerManager;
    Renderer bodyRenderer;
    float resetTimeStamp;
    const float resetTime = 5f;

    protected override void Start()
    {
        base.Start();
        powerManager = GameManager.ActiveGameManager.PowerManager;
        bodyRenderer = transform.Find("Body").GetComponent<Renderer>();
    }

    protected override void Update()
    {
        base.Update();

        if(!bodyRenderer.enabled &&
           timeManager.WorldTime - resetTimeStamp > resetTime)
                bodyRenderer.enabled = true;
    }

    protected override void Interact()
    {
        if (bodyRenderer.enabled)
        {
            if (powerManager.PowerUnits < powerManager.MaxPowerUnits)
            {
                powerManager.ResetPower();
                resetTimeStamp = timeManager.WorldTime;
                bodyRenderer.enabled = false;
            }
        }
    }

}

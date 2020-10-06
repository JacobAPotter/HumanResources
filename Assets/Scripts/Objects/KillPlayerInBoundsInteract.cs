using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerInBoundsInteract : InteractWithPlayer
{
    Bounds bounds;
    Renderer rend;
    protected override void Start()
    {
        base.Start();
        rend = GetComponent<Renderer>();
        
    }

    protected override void Update()
    {
        bounds = rend.bounds;
        bounds.size = new Vector3(bounds.size.x, 10, bounds.size.z);
        base.Update();
    }

    public override void TestForInteraction(Vector3 position)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (bounds.Contains(position))
        {
            if (timeManager.GameTime - timeStamp > minimumTimeBetweenInteractions)
                Interact();
        }
    }

    protected override void Interact()
    {
        base.Interact();
        if (GameManager.ActiveGameManager.Player.Health.Alive)
            GameManager.ActiveGameManager.StateManager.DieOnPath(transform.position);
    }
}

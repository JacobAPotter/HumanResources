using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInteract : InteractWithPlayer
{
    const float squareCollisionDistance = 0.85f * 0.85f;

    protected override void Start()
    {
        base.Start();
    }

    public override void TestForInteraction(Vector3 position)
    {
        if (!gameObject.activeInHierarchy)
            return;

        Vector3 diff = transform.position - position;

        if (diff.sqrMagnitude < squareCollisionDistance)
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

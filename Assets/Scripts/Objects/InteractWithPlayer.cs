using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class InteractWithPlayer : MonoBehaviour
{
    protected Player player;
    protected float timeStamp;
    protected TimeManager timeManager;
    protected const float minimumTimeBetweenInteractions = 0.25f;
    protected const float squareInteractionDistance = 1.5f * 1.5f;

    protected virtual void Start()
    {
        GameManager.ActiveGameManager.PlayerInteractablesManager.AddInteractable(this);
        player = GameManager.ActiveGameManager.Player;
        timeManager = GameManager.ActiveGameManager.TimeManager;
        timeStamp = -99;
    }

    protected virtual void Update()
    {
        TestForInteraction(player.transform.position);
    }

    public virtual void TestForInteraction(Vector3 position)
    {
        if (!gameObject.activeInHierarchy)
            return;

        //test at the objects position
        Vector3 diff = transform.position - position;

        if (diff.sqrMagnitude < squareInteractionDistance)
        {
            if (timeManager.GameTime - timeStamp > minimumTimeBetweenInteractions)
                Interact();
        }

        //we also test on the floor, which is where the players position actually is
        Vector3 floorDiff = new Vector3(transform.position.x, 0, transform.position.z) - position;
        if (floorDiff.sqrMagnitude < squareInteractionDistance)
        {
            if (timeManager.GameTime - timeStamp > minimumTimeBetweenInteractions)
                Interact();
        }

    }

    protected virtual void Interact()
    {
        if(GameManager.ActiveGameManager.Player.Health.Alive)
            timeStamp = timeManager.GameTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bounds PlayerBounds { get; private set; }
    ColliderManager colliderManager;
    CharacterController characterController;
    StateManager stateManager;

    public Health Health { get; private set; }

    private void Start()
    {
        colliderManager = GameManager.ActiveGameManager.ColliderManager;
        characterController = GetComponent<CharacterController>();
        Health = GetComponent<Health>();
        stateManager = GameManager.ActiveGameManager.StateManager;
    }

    private void Update()
    {
        PlayerBounds = characterController.bounds;
        Bounds collisionBounds;

        //stay on the ground unless youre hanging
        if (stateManager.CURRENT_STATE != StateManager.PLAYER_STATE.HANG)
            transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);

        if (colliderManager.CheckForIntersections(transform.position, out collisionBounds))
        {
            Vector3 target = collisionBounds.ClosestPoint(transform.position);
            target.y = transform.position.y;

            //incase we get 'trapped' inside the collider, just
            //move towards the origin.
            while(collisionBounds.Contains(target))
                target = Vector3.MoveTowards(target, Vector3.zero, 0.25f);

            Vector3 move = target - transform.position;
            move.y = 0;

            characterController.Move(move);
        }
    }
}

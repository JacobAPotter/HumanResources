using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bounds PlayerBounds { get; private set; }
    ColliderManager colliderManager;
    CharacterController characterController;
    Renderer rend;

    private void Start()
    {
        rend = transform.Find("Renderer").GetComponent<Renderer>();
        colliderManager = GameManager.ActiveGameManager.ColliderManager;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerBounds = rend.bounds;
        Bounds collisionBounds;
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

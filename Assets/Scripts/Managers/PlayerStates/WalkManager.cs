using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkManager : MonoBehaviour
{
    InputManager inputManager;
    TimeManager timeManager;
    CharacterController characterController;
    StateManager stateManager;

    const float speed = 10f;
    float rollTimestamp;
    const float rollTime = 0.5f;
    const float rollSpeed = 20f;
    Vector3 rollDirection;
    Vector3 moveDirection;
    float walkTimeStamp;
    const float minWalkTime = 0.15f;

    public enum WALK_STATE
    {
        STANDING,
        WALKING,
        ROLLING
    }

    public WALK_STATE CURRENT_WALK_STATE { get; private set; }

    void Start()
    {
        inputManager = GameManager.ActiveGameManager.InputManager;
        timeManager = GameManager.ActiveGameManager.TimeManager;
        stateManager = GameManager.ActiveGameManager.StateManager;
        rollTimestamp = -99;
        walkTimeStamp = -99;
        characterController = GetComponent<CharacterController>();
        CURRENT_WALK_STATE = WALK_STATE.STANDING;
    }

    void Update()
    {
        if (timeManager.WorldTime - rollTimestamp < rollTime)
        {
            float ratio = (timeManager.WorldTime - rollTimestamp) / rollTime;
            MoveCharacter(rollDirection * RollCurveVelocity(ratio) * speed);
            CURRENT_WALK_STATE = WALK_STATE.ROLLING;
        }
        else
        {
            CURRENT_WALK_STATE = WALK_STATE.STANDING;

            if (timeManager.WorldTime - walkTimeStamp < minWalkTime)
            {
                if (moveDirection.magnitude > 0.1f)
                {
                    transform.forward = moveDirection;
                    MoveCharacter(moveDirection * speed);
                    CURRENT_WALK_STATE = WALK_STATE.WALKING;
                }
            }
            else if (inputManager.PlayerMovent.magnitude > 0.1f)
            {
                walkTimeStamp = timeManager.WorldTime;
                moveDirection = inputManager.PlayerMovent;
                CURRENT_WALK_STATE = WALK_STATE.WALKING;
            }

            if (inputManager.TryRoll && stateManager.CURRENT_STATE != StateManager.PLAYER_STATE.DASH )
            {
                rollTimestamp = timeManager.WorldTime;
                rollDirection = inputManager.PlayerMovent;
                CURRENT_WALK_STATE = WALK_STATE.ROLLING;
            }
        }
    }

    //sigmoid modified to the range 0,1
    float RollCurvePosition(float t)
    {
        const float e = 2.71828f;
        const float coef = 10f;
        const float offset = 0.5f;

        return 1.0f / (1 + Mathf.Pow(e, -(coef * (t - offset))));
    }

    //the derivate of the above
    float RollCurveVelocity(float t)
    {
        const float e = 2.71828f;
        const float coef = 10f;
        const float expo = 5f;

        return (rollSpeed * Mathf.Pow(e, coef * t + expo)) / Mathf.Pow((Mathf.Pow(e, coef * t) + Mathf.Pow(e, expo)), 2);
    }

    void MoveCharacter(Vector3 direction)
    {
        characterController.SimpleMove(direction * timeManager.Coefficient);
    }
}

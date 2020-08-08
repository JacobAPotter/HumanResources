using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashManager : MonoBehaviour
{
    Vector3 startPoint;
    InputManager inputManager;
    LineRenderer lineRenderer;
    StateManager stateManager;
    CharacterController characterController;
    MaterialManager materialManager;
    TimeManager timeManager;
    PowerManager powerManager;

    const float dashDistance = 10;
    const float delay = 0.2f;
    const float minVectorLength = 3;
    const float maxTime = 1.5f;
    float timeStamp;

    float delayTimeStamp;
    public bool Complete { get; private set; }

    Vector3 releasePoint;

    Vector3 lineRendAbove = Vector3.up * 0.1f;


    private void Start()
    {
        inputManager = GameManager.ActiveGameManager.InputManager;
        lineRenderer = GetComponent<LineRenderer>();
        stateManager = GameManager.ActiveGameManager.StateManager;
        characterController = GetComponent<CharacterController>();
        materialManager = GameManager.ActiveGameManager.MaterialManager;
        timeManager = GameManager.ActiveGameManager.TimeManager;
        powerManager = GameManager.ActiveGameManager.PowerManager;
    }

    private void Update()
    {


        if (inputManager.PathMouseHeld)
        {
            delayTimeStamp = timeManager.WorldTime;

            if (stateManager.RaycastMouse(out releasePoint))
            {
                if (!lineRenderer.enabled && (releasePoint - startPoint).magnitude > minVectorLength)
                    lineRenderer.enabled = true;

                lineRenderer.SetPosition(0, transform.position + lineRendAbove );
                lineRenderer.SetPosition(1, transform.position + (releasePoint - startPoint) + lineRendAbove);
            }
        }
        else
        {
            if (timeManager.WorldTime - delayTimeStamp > delay)
            {
                if ((releasePoint - startPoint).magnitude > minVectorLength)
                {
                    Vector3 move = (releasePoint - startPoint).normalized * dashDistance;
                    move.y = 0;
                    characterController.Move(move);

                    powerManager.UsePower();
                }

                Complete = true;
                lineRenderer.enabled = false;
            }
        }

        if(timeManager.WorldTime - timeStamp > maxTime) //took too long, abort 
        {
            Complete = true;
            lineRenderer.enabled = false;
        }

        timeManager.SlowTime();
    }

    public void Init(Vector3 startPos)
    {
        Complete = false;
        lineRenderer.positionCount = 2;
        //dont enable it until we have a second point 
        //to set

        lineRenderer.enabled = false;
        lineRenderer.material = materialManager.yellowScribble;
        startPoint = startPos;
        lineRenderer.SetPosition(0, transform.position);
        timeStamp = timeManager.WorldTime;

    }
}

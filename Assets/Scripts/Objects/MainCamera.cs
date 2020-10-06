using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Vector3 offset = new Vector3(0, 22.5f, -18.75f);
    Player player;
    float velocity;
    TimeManager timeManager;
    float defaultOrthoSize; //(12)
    float targetOrthoSize;
    const float orthoDelta = 10f;
    const float moveAccel = 20f;
    const float maxSpeed = 8f;
    CameraZoneManager zoneManager;
    public Camera Camera { get; private set; }

    private void Start()
    {
        player = GameManager.ActiveGameManager.Player;
        // offset = transform.position - player.transform.position;
        transform.position = player.transform.position + offset;
        timeManager = GameManager.ActiveGameManager.TimeManager;
        Camera = GetComponent<Camera>();
        defaultOrthoSize = Camera.orthographicSize;
        zoneManager = GameManager.ActiveGameManager.CameraZoneManager;
    }

    private void Update()
    {
        Vector3 target = player.transform.position + offset;

        //move towards the ortho size determined by camera zones
        targetOrthoSize = defaultOrthoSize;
        float minOrtho = float.MaxValue;
        foreach (CameraZone c in zoneManager.CameraZones)
        {
            if (c.Bounds.Contains(player.transform.position))
            {
                if (c.OrthoSize < minOrtho)
                {
                    targetOrthoSize = c.OrthoSize;
                    minOrtho = c.OrthoSize;
                }

                if (c.MoveToPoint)
                    target = c.MovePoint.position + offset;
            }
        }

        if (Camera.orthographicSize < targetOrthoSize)
        {
            Camera.orthographicSize += timeManager.WorldDeltaTime * orthoDelta;
            if (Camera.orthographicSize > targetOrthoSize)
                Camera.orthographicSize = targetOrthoSize;
        }
        else if (Camera.orthographicSize > targetOrthoSize)
        {
            Camera.orthographicSize -= timeManager.WorldDeltaTime * orthoDelta;
            if (Camera.orthographicSize < targetOrthoSize)
                Camera.orthographicSize = targetOrthoSize;
        }


        //smoothly move towars target
        float dist = Vector3.Distance(transform.position, target);
        velocity += dist * timeManager.GameDeltaTime * moveAccel;
        velocity = Mathf.Min(velocity, dist * maxSpeed);
        transform.position = Vector3.MoveTowards(transform.position, target, velocity * timeManager.GameDeltaTime);
        velocity *= (1f - timeManager.GameDeltaTime);

        


    }
}

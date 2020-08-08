using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Vector3 offset;
    Player player;
    float velocity;
    TimeManager timeManager;
    public Camera Camera { get; private set; }

    private void Start()
    {
        player = GameManager.ActiveGameManager.Player;
        offset = transform.position - player.transform.position;
        timeManager = GameManager.ActiveGameManager.TimeManager;
        Camera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector3 target = player.transform.position + offset;

        float dist = Vector3.Distance(transform.position, target);

        velocity += dist * timeManager.WorldDeltaTime * 5f;
        velocity = Mathf.Min(velocity, dist * 2f);


        transform.position = Vector3.MoveTowards(transform.position, target, velocity * timeManager.WorldDeltaTime);

        velocity *=  (1f - timeManager.WorldDeltaTime);
    }
}

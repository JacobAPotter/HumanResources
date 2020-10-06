using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Transform left;
    Transform right;
    float leftEndY;
    float rightEndY;
    bool open;
    Player player;
    TimeManager timeManager;
    const float speed = 200f;

    private void Start()
    {
        left = transform.Find("l");
        right = transform.Find("r");
        player = GameManager.ActiveGameManager.Player;
        timeManager = GameManager.ActiveGameManager.TimeManager;
        leftEndY = left.rotation.eulerAngles.y - 90;
        rightEndY = right.rotation.eulerAngles.y + 90;
    }

    private void Update()
    {
        if (!open)
        {
            float sqr = (transform.position - player.transform.position).sqrMagnitude;

            if(sqr < 8f)
                open = true;
        }
        else
        {
            left.rotation = Quaternion.RotateTowards(left.rotation,
                Quaternion.Euler(left.rotation.eulerAngles.x,
                                 leftEndY,
                                 left.rotation.eulerAngles.z), timeManager.WorldDeltaTime * speed);

            right.rotation = Quaternion.RotateTowards(right.rotation,
                Quaternion.Euler(right.rotation.eulerAngles.x,
                                 rightEndY,
                                 right.rotation.eulerAngles.z), timeManager.WorldDeltaTime * speed);
        }
    }
}

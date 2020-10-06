using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPerson : MonoBehaviour
{
    Animator animator;
    float timeStamp;
    float randTime;
    TimeManager timeManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        timeManager = GameManager.ActiveGameManager.TimeManager;
        randTime = Random.value * 10f;
    }

    private void Update()
    {
        animator.speed = timeManager.Coefficient;

        if (timeManager.WorldTime - timeStamp > randTime)
        {
            float r = Random.value;

            if(r < 0.5f)
            {
                animator.SetBool("dance", false);
                animator.SetBool("stand2", false);
            }
            else if(r < 0.75f)
            {
                animator.SetBool("dance", false);
                animator.SetBool("stand2", true);
            }
            else
            {
                animator.SetBool("dance", true);
                animator.SetBool("stand2", false);
            }

            randTime = timeManager.WorldTime + (Random.value * 5) + 5f;
        }
    }
}

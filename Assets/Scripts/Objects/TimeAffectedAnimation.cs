using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAffectedAnimation : MonoBehaviour
{
    Animator animator;
    TimeManager timeManager;


    void Start()
    {
        animator = GetComponent<Animator>();
        timeManager = GameManager.ActiveGameManager.TimeManager;
    }

    void Update()
    {
        animator.speed = timeManager.Coefficient;
    }

}

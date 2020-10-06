using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingGuy : MonoBehaviour
{
    Animator animator;
    float timeStamp;
    float randTime;
    TimeManager timeManager;
    int targetIndex;

    [SerializeField]
    Vector3[] positions;

    private void Start()
    {
        animator = GetComponent<Animator>();

        timeManager = GameManager.ActiveGameManager.TimeManager;
        randTime = Random.value * 10f;
        targetIndex = Mathf.FloorToInt(Random.value * positions.Length);
    }

    private void Update()
    {
        animator.speed = timeManager.Coefficient;

        if (timeManager.WorldTime - timeStamp > randTime)
        {
            float r = Random.value;
            randTime = timeManager.WorldTime + (Random.value * 5) + 5f;
            targetIndex = (targetIndex + 1) % positions.Length;
        }

        if (Vector3.Distance(transform.position, positions[targetIndex]) > 1f)
        {
            animator.SetFloat("VelY", 1);
            Vector3 direction = transform.position - positions[targetIndex];
            float theta = Mathf.Atan2(direction.z, -direction.x) * Mathf.Rad2Deg + 90;

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, theta, transform.rotation.eulerAngles.z);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, positions[targetIndex], 6f * timeManager.WorldDeltaTime);
        }
        else
        {
            animator.SetFloat("VelY", 0);
        }
    }
}

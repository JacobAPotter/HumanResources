using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneCamera : MonoBehaviour
{
    [SerializeField]
    Transform follow;

    Vector3 offset;

    private void Start()
    {
        offset = transform.position - follow.position;
    }

    private void Update()
    {
        transform.position = follow.position + offset;
        transform.LookAt(follow);
    }
}

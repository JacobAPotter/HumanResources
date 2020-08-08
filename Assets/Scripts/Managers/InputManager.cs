using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector3 PlayerMovent { get; private set; }
    public bool TryRoll { get; private set; }
    public bool PathMousePressed { get; private set; }
    public bool PathMouseHeld { get; private set; }
    public bool AttackMousePressed { get; private set; }
    public bool AttackMouseHeld { get; private set; }
    const int pathMouseIndex = 1;

    private void Update()
    {
        PlayerMovent = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            PlayerMovent += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            PlayerMovent += Vector3.back;
        if (Input.GetKey(KeyCode.A))
            PlayerMovent += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            PlayerMovent += Vector3.right;

        if (PlayerMovent.magnitude > 0.1f)
            PlayerMovent = PlayerMovent.normalized;

        TryRoll = Input.GetKeyDown(KeyCode.Space);

        PathMousePressed = Input.GetMouseButtonDown(pathMouseIndex);
        PathMouseHeld = Input.GetMouseButton(pathMouseIndex);
        AttackMousePressed = Input.GetMouseButtonDown(1 - pathMouseIndex);
        AttackMouseHeld = Input.GetMouseButton(1 - pathMouseIndex);
    }

}

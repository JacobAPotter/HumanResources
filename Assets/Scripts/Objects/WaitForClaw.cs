using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForClaw : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    Player player;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        player = GameManager.ActiveGameManager.Player;
        GameManager.ActiveGameManager.TracksManager.AddWaitForClawPoint(this);
    }

    public Bounds Bounds
    {
        get { return capsuleCollider.bounds; }
    }
}

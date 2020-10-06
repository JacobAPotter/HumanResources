using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClaw : MonoBehaviour
{
    LineRenderer chain;
    float downRatioTarget;
    float downRatio;
    Player player;
    TracksManager tracksManager;
    Claw claw;
    bool playerIsHanging;

    private void Start()
    {
        chain = GetComponent<LineRenderer>();
        player = GameManager.ActiveGameManager.Player;
        tracksManager = GameManager.ActiveGameManager.TracksManager;
        claw = transform.Find("Claw").GetComponent<Claw>();
        chain.positionCount = 2;
    }

    private void LateUpdate()
    {
        Vector3 playerTargetPos = player.transform.position + Vector3.up;

        if (playerIsHanging)
        {
            downRatioTarget -= GameManager.ActiveGameManager.TimeManager.WorldDeltaTime * 0.2f;
            player.transform.position = claw.transform.position + Vector3.down * 2;
            GameManager.ActiveGameManager.PlayerRenderer.ClawYRotation(claw.transform.rotation.eulerAngles.y);
        }
        else
        {
            if (tracksManager.PlayerWaitingForClaw)
            {
                Vector3 pos = transform.position;
                pos.y = 0;
                float xzDistance = Vector3.Distance(playerTargetPos, pos);
                downRatioTarget = Mathf.Clamp(10 - xzDistance, 0, 10) / 10;

                float distToTarget = Vector3.Distance(claw.transform.position, playerTargetPos);

                if (distToTarget < 2f)
                {
                    player.GetComponent<StateManager>().SetPlayerIsHanging(true);
                    playerIsHanging = true;
                }
            }
            else
                downRatioTarget = 0;
        }

        if (downRatioTarget > downRatio)
        {
            downRatio += GameManager.ActiveGameManager.TimeManager.WorldDeltaTime * 5f;
            if (downRatio > downRatioTarget)
                downRatio = downRatioTarget;
        }
        else
        {
            downRatio -= GameManager.ActiveGameManager.TimeManager.WorldDeltaTime * 5f;
            if (downRatio < downRatioTarget)
                downRatio = downRatioTarget;
        }

        claw.transform.position = Vector3.Lerp(transform.position, playerTargetPos, downRatio);

        chain.SetPosition(0, transform.position);
        chain.SetPosition(1, claw.transform.position);
    }
}
 
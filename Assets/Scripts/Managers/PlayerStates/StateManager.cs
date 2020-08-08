using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Ensure that exactly one form of movement is activated at a time
public class StateManager : MonoBehaviour
{
    WalkManager walkManager;
    DashManager dashManager;
    DrawManager drawManager;
    InputManager inputManager;
    MainCamera mainCamera;
    PowerManager powerManager;

    int pathColLayerMask;

    public const float maxDistFromPlayerForDraw = 1.5f;

    public enum PLAYER_STATE
    {
        WALK,
        DASH,
        DRAW
    }

    public PLAYER_STATE CURRENT_STATE { get; private set; }

    private void Start()
    {
        inputManager = GameManager.ActiveGameManager.InputManager;
        walkManager = GetComponent<WalkManager>();
        dashManager = GetComponent<DashManager>();
        dashManager.enabled = false;
        drawManager = GetComponent<DrawManager>();
        powerManager = GetComponent<PowerManager>();
        drawManager.enabled = false;
        mainCamera = GameManager.ActiveGameManager.MainCamera;
        pathColLayerMask = LayerMask.GetMask("DrawCollider");
        CURRENT_STATE = PLAYER_STATE.WALK;
    }

    void Update()
    {
        switch (CURRENT_STATE)
        {
            case PLAYER_STATE.WALK:

                GameManager.ActiveGameManager.DebugText.text = walkManager.CURRENT_WALK_STATE.ToString();

                if (inputManager.PathMousePressed)
                {
                    if (walkManager.CURRENT_WALK_STATE == WalkManager.WALK_STATE.STANDING ||
                        walkManager.CURRENT_WALK_STATE == WalkManager.WALK_STATE.WALKING)
                    {
                        Vector3 point;
                        if (RaycastMouse(out point))
                        {
                            float distFromPlayer = Vector3.Distance(transform.position, point);

                            //Start Draw Mode
                            if (distFromPlayer < maxDistFromPlayerForDraw )
                            {
                                if (powerManager.PowerUnits > 0)
                                {
                                    CURRENT_STATE = PLAYER_STATE.DRAW;
                                    walkManager.enabled = false;
                                    dashManager.enabled = false;
                                    drawManager.enabled = true;
                                    drawManager.Init(point);
                                }
                            }
                            //Start Dash Mode
                            else
                            {
                                if (powerManager.PowerUnits > 0)
                                {
                                    CURRENT_STATE = PLAYER_STATE.DASH;
                                    walkManager.enabled = true;
                                    drawManager.enabled = false;
                                    dashManager.enabled = true;
                                    dashManager.Init(point);
                                }
                            }

                        }
                        else
                            Debug.Log("Path Collider not found");
                    }
                }

                break;
            case PLAYER_STATE.DRAW:

                GameManager.ActiveGameManager.DebugText.text = drawManager.CURRENT_DRAW_STATE.ToString();

                if (drawManager.CURRENT_DRAW_STATE == DrawManager.PATH_STATE.INACTIVE)
                {
                    drawManager.enabled = false;
                    dashManager.enabled = false;
                    walkManager.enabled = true;
                    CURRENT_STATE = PLAYER_STATE.WALK;
                }

                break;
            case PLAYER_STATE.DASH:
                if(dashManager.Complete)
                {
                    CURRENT_STATE = PLAYER_STATE.WALK;
                    dashManager.enabled = false;
                    walkManager.enabled = true;
                    drawManager.enabled = false;
                }
                break;
        }

    }

    public bool RaycastMouse(out Vector3 point)
    {
        Vector3 mousePos = mainCamera.Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));

        if (GetPoint(mainCamera.transform.position, mousePos - mainCamera.transform.position, out point))
            return true;

        return false;
    }

    public bool GetPoint(Vector3 origin, Vector3 forward, out Vector3 point)
    {
        Ray ray = new Ray(origin, forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, pathColLayerMask))
        {
            point = hit.point;
            return true;
        }

        point = Vector3.zero;
        return false;
    }


}

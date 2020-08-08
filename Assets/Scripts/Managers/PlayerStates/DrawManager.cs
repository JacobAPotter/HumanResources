using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{

    public enum PATH_STATE
    {
        DRAWING,
        ON_PATH,
        POST,
        INACTIVE,
    }

    public PATH_STATE CURRENT_DRAW_STATE { get; private set; }

    Player player;
    CharacterController characterController;
    InputManager inputManager;
    int layerMask;

    int drawPointIndex;
    Vector3[] drawPoints;
    const int maxDrawPoints = 300;
    const float maxDrawPointDistance = 2f;
    float drawTimeStamp;
    const float maxDrawTime = 3f;
    float accumDrawDistance;

    int smoothedPointIndex;
    const int maxSmoothedPoints = 600;
    const float minSmoothPointDistance = 0.5f;
    const float minSmoothPointDistanceSquared = minSmoothPointDistance * minSmoothPointDistance;

    //the maximum length we can create a smooth curve for with some wiggle room
    const float maxAccumDrawDistance = maxSmoothedPoints * minSmoothPointDistanceSquared * 0.98f;

    Vector3[] smoothedPoints;
    LineRenderer lineRenderer;
    TimeManager timeManager;
    ColliderManager colliderManager;
    PowerManager powerManager;
    Vector3 colliderStopPosition;
    bool pathCollides;

    float onPathTimeStamp;

    StateManager stateManager;
    MaterialManager materialManager;

    Vector3 pathAboveGround;
    Vector3 teleportPoint;

    //keep track of the decreasing length of the line renderer as
    //the player teleports
    float lineRenderLength;
    const float lineRecedingSpeed = 120f;

    private void Start()
    {
        player = GameManager.ActiveGameManager.Player;
        characterController = GetComponent<CharacterController>();
        layerMask = LayerMask.GetMask("DrawCollider");
        lineRenderer = GetComponent<LineRenderer>();
        smoothedPoints = new Vector3[maxSmoothedPoints];
        drawPoints = new Vector3[maxDrawPoints];
        timeManager = GameManager.ActiveGameManager.TimeManager;
        stateManager = GetComponent<StateManager>();
        inputManager = GameManager.ActiveGameManager.InputManager;
        colliderManager = GameManager.ActiveGameManager.ColliderManager;
        pathAboveGround = Vector3.up * 0.08f;
        materialManager = GameManager.ActiveGameManager.MaterialManager;
        powerManager = GameManager.ActiveGameManager.PowerManager;
        Deactivate();
    }

    private void Update()
    {
        const int numberOfUnitsNeededForMaxDistance = 5;

        //how far we can go depends on how many units we have, not how many maxUnits we have.
        float maxDistanceFromPower = powerManager.PowerUnits * maxAccumDrawDistance / numberOfUnitsNeededForMaxDistance; // powerManager.MaxPowerUnits;

        if (powerManager.PowerUnits == 1)
            maxDistanceFromPower *= 0.4f;

        switch (CURRENT_DRAW_STATE)
        {
            case PATH_STATE.DRAWING:
                {
                    timeManager.SlowTime();

                    //Theyre drawing a path.
                    //Any one of these conditions will finalize the path.
                    if (inputManager.PathMouseHeld &&  //they release the mouse
                        timeManager.WorldTime - drawTimeStamp < maxDrawTime && //they run out of time 
                        drawPointIndex < maxDrawPoints - 1 && //we reach the max number of points on the path 
                        accumDrawDistance < maxDistanceFromPower) //we reach the max distance allowed for this much power
                    {

                        Vector3 newPoint;

                        if (stateManager.RaycastMouse(out newPoint)) //get the point the mouse points to on the plane 
                        {
                            //get the distance between the new point and the last
                            float distFromLastPoint = Vector3.Distance(drawPoints[drawPointIndex], newPoint);

                            //we are far enough from the last point to add a new one 
                            if (distFromLastPoint > maxDrawPointDistance)
                            {
                                //this is the last point we will add before reaching max. 
                                //If we add this point it could allow the path length to 
                                //exceed the max.
                                if (drawPointIndex + 2 >= maxDrawPoints)
                                {
                                    //Make sure it does not cause the path to 
                                    //exceed the maximum distance
                                    if (accumDrawDistance + distFromLastPoint > maxDrawPointDistance)
                                    {
                                        //find the max distance allowed and normalize the difference-vector to that.
                                        float maxdistToNewPoint = maxDrawPointDistance - accumDrawDistance;
                                        Vector3 direction = newPoint - drawPoints[drawPointIndex];
                                        newPoint = drawPoints[drawPointIndex] + (direction.normalized * maxDrawPointDistance);
                                    }
                                }

                                //add the point to the path
                                accumDrawDistance += distFromLastPoint;
                                drawPoints[++drawPointIndex] = newPoint + pathAboveGround;
                                UpdateLineRenderer(drawPoints, drawPointIndex);
                            }
                            else //just so we can see the line renderer point to the current mouse position
                            {
                                //these arent added to the final path, just temporarily used for the 
                                //line renderer
                                drawPoints[drawPointIndex + 1] = newPoint + pathAboveGround;
                                drawPoints[drawPointIndex + 2] = newPoint + pathAboveGround;
                                UpdateLineRenderer(drawPoints, drawPointIndex + 2);
                            }
                        }
                    }
                    else
                    {
                         
                        //create our final path, which has mid points and 
                        //may stop early if the path is blocked.
                        GenerateSmoothPath();
                        CheckForCollisions();

                        lineRenderLength = smoothedPointIndex;

                        //use the last point the mouse was at as our teleport spot. 
                        //If its invalid, use the last point on the path.
                        if (!stateManager.RaycastMouse(out teleportPoint))
                            teleportPoint = smoothedPoints[smoothedPointIndex];

                        //Were ready to begin going down the path
                        CURRENT_DRAW_STATE = PATH_STATE.ON_PATH;
                        onPathTimeStamp = timeManager.WorldTime;
                    }

                }
                break;

            case PATH_STATE.ON_PATH:
                {
                    UpdateLineRenderer(smoothedPoints, smoothedPointIndex + 1);

                    timeManager.SlowTime();

                    //the time it takes to teleport/traverse the path
                    //depends on the length of the path
                    float maxPathTime = Mathf.Min(accumDrawDistance * 0.005f, 0.35f);

                    if (timeManager.WorldTime - onPathTimeStamp > maxPathTime)
                        CURRENT_DRAW_STATE = PATH_STATE.POST;
                }

                break;
            case PATH_STATE.POST:

                //the path clears 
                if (lineRenderer.positionCount > 0)
                {
                    //one per second
                    lineRenderLength -= timeManager.WorldDeltaTime * lineRecedingSpeed;
                    lineRenderer.positionCount = (int)lineRenderLength;
                }
                else
                {
                    Vector3 move;

                    if (pathCollides) //path is blocked, stop early
                        move = ( colliderStopPosition - transform.position);
                    else
                        move = (teleportPoint - transform.position);

                    move.y = 0;

                    characterController.Move(move);

                    int numberOfPowerUnitsUsed =  (int)Mathf.Ceil( (accumDrawDistance / maxAccumDrawDistance ) * numberOfUnitsNeededForMaxDistance);
                    powerManager.UsePower(numberOfPowerUnitsUsed);
                    Deactivate();
                }

                break;
        }
    }

    public void Init(Vector3 startingPoint)
    {
        CURRENT_DRAW_STATE = PATH_STATE.DRAWING;
        drawPointIndex = 0;
        drawPoints[drawPointIndex] = startingPoint;
        drawTimeStamp = timeManager.WorldTime;
        lineRenderer.enabled = true;
        lineRenderer.material = materialManager.blueScribble;
        pathCollides = false;
    }

    void CheckForCollisions()
    {
        Bounds bounds;
        for (int i = 0; i < smoothedPointIndex; i++)
        {
            if (colliderManager.CheckForIntersections(smoothedPoints[i], out bounds))
            {
                pathCollides = true;
                if (i > 0)
                {
                    Vector3 pointOnBounds = bounds.ClosestPoint(smoothedPoints[i - 1]);
                    Vector3 direction = smoothedPoints[i - 1] - pointOnBounds;
                    colliderStopPosition = pointOnBounds + (direction.normalized * player.PlayerBounds.extents.x);
                    colliderStopPosition.y = smoothedPoints[i - 1].y;
                }
                else
                {
                    colliderStopPosition = smoothedPoints[0];
                }

                smoothedPointIndex = i;

                return;
            }
        }
    }

    void GenerateSmoothPath()
    {

        Vector3 newPoint;
        if (stateManager.RaycastMouse(out newPoint)) //get the last point
        {
            if (drawPointIndex + 1 < maxDrawPoints)
                drawPoints[++drawPointIndex] = newPoint;
        }


        smoothedPointIndex = 0;

        Vector3[] build = new Vector3[maxSmoothedPoints];

        build[smoothedPointIndex] = drawPoints[0];

        int i = 1;

        //rebuld the drawn path and 
        //create additional midpoints where necessary, ie where 
        //the distance between two points is to great.
        while (true)
        {
            Vector3 difference = drawPoints[i] - build[smoothedPointIndex];
            float squareMag = difference.sqrMagnitude;

            //the distance is too far. We will add a new point and we will NOT move on to
            //the next drawPoint
            if (squareMag > minSmoothPointDistanceSquared)
            {
                build[smoothedPointIndex + 1] = build[smoothedPointIndex] + difference.normalized * minSmoothPointDistance;
                smoothedPointIndex++;

                if (smoothedPointIndex >= maxSmoothedPoints - 2)
                {
                    Debug.Log("Max Smooth Points Exceeded. Drawn Path was too long?");
                    break;
                }

            }
            else
            {
                build[smoothedPointIndex] = drawPoints[i];

                i++;
                if (i >= drawPointIndex)
                    break;
            }
        }

        //smooth here
        for (int b = 0; b <= smoothedPointIndex; b++)
            smoothedPoints[b] = build[b];

        //for (int i = 0; i <= drawPointIndex; i++)
        //    smoothedPoints[i] = drawPoints[i];

        //smoothedPointIndex = drawPointIndex;
    }

    void Deactivate()
    {
        CURRENT_DRAW_STATE = PATH_STATE.INACTIVE;
        drawPointIndex = -1;
        smoothedPointIndex = -1;
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
        accumDrawDistance = 0;
        this.enabled = false;
        pathCollides = false;
    }

    void UpdateLineRenderer(Vector3[] points, int maxIndex)
    {
        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        Vector3[] newPoints = new Vector3[maxIndex];


        for (int i = 0; i < newPoints.Length; i++)
            newPoints[newPoints.Length - 1 - i] = points[i];

        lineRenderer.positionCount = maxIndex;
        lineRenderer.SetPositions(newPoints);
    }

}

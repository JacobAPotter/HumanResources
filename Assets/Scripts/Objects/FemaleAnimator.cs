using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FemaleAnimator : MonoBehaviour
{
    float actionEndTime;
    NavMeshAgent navAgent;
    Animator animator;
    TimeManager timeManager;
    InputManager inputManager;
    Player player;
    Vector3 destination;
    Vector3 seat;

    public enum FEMALE_ACTION
    {
        WALK,
        WALK_TO_SEAT,
        SIT,
        WALK_TO_PLAYER,
        STAND,
        RUN,
        MOVE_OUT_OF_WAY
    }

    [SerializeField]
    bool followPlayer;

    FEMALE_ACTION CURRENT_FEMALE_ACTION;
    const float walkSpeed = 2.4f;
    const float runSpeed = 9f;
    const float runDistance = 13f;
    const float walkDistance = 6f;
    float seatedRotation;
    float prevSpeed;
    bool overrideAnimator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        timeManager = GameManager.ActiveGameManager.TimeManager;
        navAgent = GetComponent<NavMeshAgent>();

        if (!navAgent)
            navAgent = transform.parent.GetComponent<NavMeshAgent>();

        player = GameManager.ActiveGameManager.Player;
        inputManager = GameManager.ActiveGameManager.InputManager;
    }

    private void Update()
    {
        if (overrideAnimator)
            return;

        float d = 0;
        if (followPlayer)
            d = Vector3.Distance(transform.position, player.transform.position);

        float speed = prevSpeed;

        if (d > runDistance)
        {
            CURRENT_FEMALE_ACTION = FEMALE_ACTION.RUN;

            if (followPlayer)
                navAgent.SetDestination(player.transform.position);
            else
            {
                Vector3 destination;
                //look around for somewhere nearby to stand from the list of 'safe'
                //destinations used by enemies
                if (!GameManager.ActiveGameManager.EnemyManager.GetRandomDestination(out destination))
                    destination = transform.position;

                navAgent.SetDestination(destination);

            }

            speed = runSpeed;
        }
        else if (d > walkDistance)
        {

            CURRENT_FEMALE_ACTION = FEMALE_ACTION.WALK;
            speed = walkSpeed;

            if (followPlayer)
                navAgent.SetDestination(player.transform.position);
            else
            {
                Vector3 destination;

                if (!GameManager.ActiveGameManager.EnemyManager.GetRandomDestination(out destination))
                    destination = transform.position;

                navAgent.SetDestination(destination);
            }
        }
        else
        {
            if (CURRENT_FEMALE_ACTION == FEMALE_ACTION.WALK || CURRENT_FEMALE_ACTION == FEMALE_ACTION.RUN)
            {
                CURRENT_FEMALE_ACTION = FEMALE_ACTION.STAND;
                actionEndTime = timeManager.WorldTime + 4 + (Random.value * 4);
                navAgent.SetDestination(transform.position);
            }
            else
            {
                //here we check if shes standing in our way
                if(followPlayer && 
                    Vector3.Distance(player.transform.position, transform.position) < 2.5f
                    && CURRENT_FEMALE_ACTION == FEMALE_ACTION.STAND 
                    && inputManager.PlayerMovent.magnitude > 0.1f)
                {
                    CURRENT_FEMALE_ACTION = FEMALE_ACTION.MOVE_OUT_OF_WAY;
                }

                if (CURRENT_FEMALE_ACTION == FEMALE_ACTION.MOVE_OUT_OF_WAY)
                {
                    
                    Vector2 playerForward = new Vector2(inputManager.PlayerMovent.x, 
                                                        inputManager.PlayerMovent.z);

                    Vector3 perpendicularToPlayer = Vector2.Perpendicular(playerForward);

                    navAgent.SetDestination(player.transform.position + perpendicularToPlayer * 2);
                    speed = walkSpeed;

                    if(navAgent.remainingDistance < 1f)
                    {
                        CURRENT_FEMALE_ACTION = FEMALE_ACTION.STAND;
                        actionEndTime = timeManager.WorldTime + 4 + (Random.value * 4);
                        navAgent.SetDestination(transform.position);
                    }
                }
                else if (timeManager.WorldTime > actionEndTime)
                {
                    //been standing a while, look for somewhere to sit.
                    if (CURRENT_FEMALE_ACTION == FEMALE_ACTION.STAND)
                    {
                        if (GameManager.ActiveGameManager.ChairManager.GetChair(transform.position, out seat, 12))
                        {
                            CURRENT_FEMALE_ACTION = FEMALE_ACTION.WALK_TO_SEAT;
                            navAgent.SetDestination(seat);
                            speed = walkSpeed;
                        }
                    }
                    else if (CURRENT_FEMALE_ACTION == FEMALE_ACTION.WALK_TO_SEAT)
                    {
                        float dist = navAgent.remainingDistance;

                        if (dist < 1.5f)
                        {
                            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                                                 Quaternion.Euler(transform.eulerAngles.x, seatedRotation, transform.eulerAngles.z),
                                                 timeManager.WorldDeltaTime * 360);
                        }
                        else
                            seatedRotation = transform.rotation.eulerAngles.y + 180;

                        if(dist < 0.2f)
                        { 
                            CURRENT_FEMALE_ACTION = FEMALE_ACTION.SIT;
                            actionEndTime = timeManager.WorldTime + 8 + (Random.value * 10);
                            navAgent.SetDestination(transform.position);
                        }
                    }
                    else if (CURRENT_FEMALE_ACTION == FEMALE_ACTION.SIT)
                    {
                        if (followPlayer)
                            navAgent.SetDestination(player.transform.position);
                        else
                        {
                            Vector3 destination;

                            if (!GameManager.ActiveGameManager.EnemyManager.GetRandomDestination(out destination))
                                destination = transform.position;

                            navAgent.SetDestination(destination);

                        }

                        CURRENT_FEMALE_ACTION = FEMALE_ACTION.WALK_TO_PLAYER;
                        speed = walkSpeed;
                    }
                    else if (CURRENT_FEMALE_ACTION == FEMALE_ACTION.WALK_TO_PLAYER)
                    {
                        if(navAgent.remainingDistance < 5f)
                        {
                            if (Vector3.Distance(transform.position, seat) > 3f)
                            {
                                CURRENT_FEMALE_ACTION = FEMALE_ACTION.STAND;
                                actionEndTime = timeManager.WorldTime + 5 + (Random.value * 5);
                                navAgent.SetDestination(transform.position);
                            }
                            else
                            {
                                navAgent.SetDestination(seat + Vector3.right * 3.1f);
                            }
                        }
                    }
                }

            }
        }

        UpdateAnimator(CURRENT_FEMALE_ACTION);

        prevSpeed = speed;
        navAgent.speed = speed * timeManager.Coefficient;
    }

    void UpdateAnimator(FEMALE_ACTION ACTION)
    {
        switch (ACTION)
        {
            case FEMALE_ACTION.STAND:
                animator.SetBool("sit", false);
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
                break;
            case FEMALE_ACTION.WALK:
            case FEMALE_ACTION.WALK_TO_PLAYER:
            case FEMALE_ACTION.WALK_TO_SEAT:
            case FEMALE_ACTION.MOVE_OUT_OF_WAY:
                animator.SetBool("sit", false);
                animator.SetBool("walk", true);
                animator.SetBool("run", false);
                break;
            case FEMALE_ACTION.RUN:
                animator.SetBool("sit", false);
                animator.SetBool("walk", false);
                animator.SetBool("run", true);
                break;
            case FEMALE_ACTION.SIT:
                animator.SetBool("sit", true);
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
                break;
        }
    }
    public void OverrideAnimator(FEMALE_ACTION OVERRIDE_ACTION)
    {
        overrideAnimator = true;
        UpdateAnimator(OVERRIDE_ACTION);

        if (OVERRIDE_ACTION == FEMALE_ACTION.STAND || OVERRIDE_ACTION == FEMALE_ACTION.SIT)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.speed = 0;
        }
        else if (OVERRIDE_ACTION == FEMALE_ACTION.RUN)
            navAgent.speed = runSpeed;
        else
            navAgent.speed = walkSpeed;
    }

    public void ForceLookAt(Vector3 lookAt)
    {
        transform.LookAt(lookAt);
    }

    public void EndOverride()
    {
        overrideAnimator = false;
    }
}
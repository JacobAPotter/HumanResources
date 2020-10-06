using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFemale : MonoBehaviour
{

    StateManager stateManager;
    GameObject groundRadius;
    InputManager inputManager;
    TimeManager timeManager;
    FemaleDirectAttack directAttack;
    FemaleRadialPower radialPower;
    FemaleAnimator femaleAnimator;
    GameObject chargedAnimation;
    Vector3 plusSignOffset;
    Transform plusSign;

    float randomDoPowerTime;
    float powerTimeStamp;
    const float powerChargeTime = 10f;
    const float maxRange = 15f;
    bool overrideAnimator;
    bool isCharged;
    float chargeAnimationTimeStamp;

    void Start()
    {
        stateManager = GameManager.ActiveGameManager.StateManager;
        groundRadius = transform.Find("Radius").gameObject;
        chargedAnimation = transform.Find("Ani").gameObject;
        plusSign = chargedAnimation.transform.Find("Plus");
        plusSignOffset = plusSign.localPosition;
        chargedAnimation.SetActive(false);
        inputManager = GameManager.ActiveGameManager.InputManager;
        timeManager = GameManager.ActiveGameManager.TimeManager;
        powerTimeStamp = -powerChargeTime;
        randomDoPowerTime = timeManager.WorldTime + powerChargeTime;
        radialPower = GameManager.ActiveGameManager.FemaleRadialPower;
        directAttack = GameManager.ActiveGameManager.FemaleDirectAttack;
        femaleAnimator = GetComponent<FemaleAnimator>();
        isCharged = true;
        chargeAnimationTimeStamp = float.MinValue;
    }

    void Update()
    {
        if (directAttack.gameObject.activeSelf || radialPower.gameObject.activeSelf)
        {
            if (!overrideAnimator)
            {
                femaleAnimator.OverrideAnimator(FemaleAnimator.FEMALE_ACTION.STAND);
                overrideAnimator = true;
            }
        }
        else
        {
            if (overrideAnimator)
            {
                femaleAnimator.EndOverride();
                overrideAnimator = false;
            }
        }

        if (timeManager.WorldTime - powerTimeStamp > powerChargeTime)
        {
            if(!isCharged)
            {
                //just got fully charged this frame.]
                //display charge animation.
                isCharged = true;
                chargeAnimationTimeStamp = timeManager.WorldTime;
            }


            Vector3 mousePos;
            stateManager.RaycastMouse(out mousePos);
            float dist = Vector3.Distance(mousePos, transform.position);

            if (dist < 2f)
            {
                groundRadius.gameObject.SetActive(true);

                if (inputManager.PathMousePressed)
                {
                    powerTimeStamp = timeManager.WorldTime;
                    DoPower(true);
                }
            }
            else
                groundRadius.gameObject.SetActive(false);

            //she randomly decides to use her power.
            //Will only happen if theres enemies in range.
            if (timeManager.WorldTime > randomDoPowerTime)
                DoPower(false);
        }
        else
            groundRadius.gameObject.SetActive(false);

        if (timeManager.WorldTime - chargeAnimationTimeStamp < 1)
        {
            chargedAnimation.SetActive(true);
            chargedAnimation.transform.rotation = Quaternion.identity;
            plusSign.localPosition = plusSignOffset + Vector3.up *
                                    (timeManager.WorldTime - chargeAnimationTimeStamp);
        }
        else
            chargedAnimation.SetActive(false);


    }

    public void SetPosition(Vector3 pos)
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        transform.position = pos;
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
    }

    public void DoPower(bool initByPlayer)
    {
        List<Health> allHealths = Health.AllHealthComponents;
        List<Health> inRange = new List<Health>();

        foreach (Health h in allHealths)
        {
            if (h.gameObject.activeSelf && h.Alive)
            {
                Vector3 diff = transform.position - h.transform.position;
                if (diff.sqrMagnitude < maxRange * maxRange)
                {
                    if (h != GameManager.ActiveGameManager.Player.Health)
                    {
                        if(CanSeeTarget(transform.position + Vector3.up, h))
                            inRange.Add(h);
                    }
                }
            }
        }

        //if this wasnt a command from the player, 
        //and we dont have any enemies, then dont do anything. 
        //but try again in 10 secods
        if (inRange.Count == 0)
            if (!initByPlayer)
            {
                randomDoPowerTime = timeManager.WorldTime + 10;
                return;
            }

        isCharged = false;
        powerTimeStamp = timeManager.WorldTime;
        //when she will randomly decide to use her powers.
        randomDoPowerTime = timeManager.WorldTime + powerChargeTime + 5 + (Random.value * 3);

        //do a direct attack
        if (inRange.Count == 1)
        {
            directAttack.Init(transform.position, inRange[0]);
            femaleAnimator.ForceLookAt(inRange[0].transform.position);
            return;
        }

        radialPower.Init(transform.position);
    }

    public static bool CanSeeTarget(Vector3 myPos, Health target)
    {
        Vector3 direction = target.transform.position - myPos;
        float dist = direction.magnitude * 1.1f;

        RaycastHit hit;

        //only check against walls and the objects layer
        int rayMask = LayerMask.GetMask("Wall", LayerMask.LayerToName(target.gameObject.layer));

        if (Physics.Raycast(new Ray(myPos, direction),
                                            out hit,
                                            dist,
                                            rayMask))
        {
            if (hit.collider.transform == target.transform)
                return true;
        }

        return false;
    }
}
 

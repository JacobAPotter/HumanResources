using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleDirectAttack : MonoBehaviour
{
    Renderer rend;
    float timeInit;
    const float attackTime = 0.75f;
    TimeManager timeManager;
    Health target;
    float rendMaxScale;

    private void Start()
    {
        timeManager = GameManager.ActiveGameManager.TimeManager;
        rend = transform.Find("rend").GetComponent<Renderer>();
        rendMaxScale = rend.transform.localScale.x;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float ratio = (timeManager.WorldTime - timeInit) / attackTime;

        if(ratio > 1f)
        {
            if(MainFemale.CanSeeTarget(transform.position + Vector3.up, target))
                target.DamageHealth(15);

            gameObject.SetActive(false);
        }
        else
        {
            rend.transform.localScale = Vector3.one * ratio * rendMaxScale;
            float theta = -Mathf.Atan2(target.transform.position.z - transform.position.z,
                                    target.transform.position.x - transform.position.x) *
                                   Mathf.Rad2Deg + 270;
            rend.transform.rotation = Quaternion.Euler(270, theta, 0);
        }
    }

    public void Init(Vector3 start, Health target)
    {
        gameObject.SetActive(true);
        transform.position = start;

        Vector3 direction = target.transform.position - start;

        rend.transform.localPosition = Vector3.up * 2f + direction.normalized * 2f;


        rend.transform.localScale = Vector3.one * 0.1f;


        float theta = -Mathf.Atan2( target.transform.position.z - transform.position.z,
                                    target.transform.position.x - transform.position.x) * 
                                   Mathf.Rad2Deg + 270;

        rend.transform.rotation = Quaternion.Euler(270, theta, 0);
        this.target = target;
        
        timeInit = timeManager.WorldTime;
    }
}

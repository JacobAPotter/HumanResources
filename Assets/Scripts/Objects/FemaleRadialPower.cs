using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleRadialPower : MonoBehaviour
{
    const float maxRange = 12;
    const float lifeSpan = 1.5f;
    float timeActivated;
    TimeManager timeManager;
    Renderer rend;
    float maxRendScale;
    EnemyManager enemyManager;
    const int damage = 8;

    private void Start()
    {
        timeManager = GameManager.ActiveGameManager.TimeManager;
        rend = transform.Find("rend").GetComponent<Renderer>();
        maxRendScale = rend.transform.localScale.x;
        rend.transform.localScale = Vector3.one * 0.1f;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        float ratio = (timeManager.WorldTime - timeActivated) / lifeSpan;

        if (ratio > 1f) //end of power, check for enemies to damage
        {
            List<Health> allHealths = Health.AllHealthComponents;

            foreach(Health h in allHealths)
            {
                if (h.gameObject.activeSelf && h.Alive)
                {
                    float sqrMag = (transform.position - h.transform.position).sqrMagnitude;

                    if (sqrMag < maxRange * maxRange)
                    {
                        if (h != GameManager.ActiveGameManager.Player.Health)
                        {
                            if(MainFemale.CanSeeTarget(transform.position + Vector3.up , h))
                                h.DamageHealth(damage);
                        }
                    }
                }
            }

            gameObject.SetActive(false);
        }
        else
        {
            rend.transform.localScale = Vector3.one * Mathf.Max( ratio, 0.02f) * maxRendScale;
        }
    }

    public void Init(Vector3 pos)
    {
        gameObject.SetActive(true);
        pos.y = 0.05f;
        transform.position = pos;
        timeActivated = timeManager.WorldTime;
    }
}

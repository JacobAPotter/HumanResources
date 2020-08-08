using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    float coef;
    const float minCoef = 0.1f;
    const float slowRate = 4;
    bool slowTime;

    private void Start()
    {
        coef = 1;
    }

    private void LateUpdate()
    {
        if (slowTime)
        {
            coef -= Time.deltaTime * slowRate;
            if (coef < minCoef)
                coef = minCoef;
        }
        else
        {
            coef += Time.deltaTime * slowRate;
            if (coef > 1f)
                coef = 1;
        }

        slowTime = false;
    }

    public float WorldDeltaTime
    {
        get { return Time.deltaTime * coef; }
    }

    public float Coefficient
    {
        get { return coef; }
    }

    public void SlowTime()
    {
        slowTime = true;
    }

    public float WorldTime
    {
        get
        {
            return Time.timeSinceLevelLoad;
        }
    }
}

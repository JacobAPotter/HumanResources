using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshExplosion : MonoBehaviour
{
    Transform[] balls;
    float[] ballExpanstionTimes;
    TimeManager timeManager;
    const float explosionTime = 0.5f;
    const float offsetTime = 0.25f;

    public void Start()
    {
        balls = new Transform[transform.childCount];
        ballExpanstionTimes = new float[transform.childCount];
        timeManager = GameManager.ActiveGameManager.TimeManager;
        
        for (int i = 0; i < transform.childCount; i++)
            balls[i] = transform.GetChild(i);
    }

    private void Update()
    {
        bool complete = true;

        //all of the balls expand and retract in one second,
        //starting at different times.
        //once theyve all finished, its complete.
        for (int i = 0; i < balls.Length; i++)
        {
            //I dont know why.. but times are not initialized..
            if (ballExpanstionTimes[i] < 0.1f)
                ballExpanstionTimes[i] = timeManager.WorldTime + Random.value * offsetTime;
               
            float t = (timeManager.WorldTime - ballExpanstionTimes[i]);

            if (t > 0 && t < explosionTime)
            {
                balls[i].transform.localScale = Vector3.one *
                    (Mathf.Sin(t * Mathf.PI * (1.0f/explosionTime)));
            }

            if (t < explosionTime)
            {
                complete = false;
            }
        }

        if (complete)
            gameObject.SetActive(false);
    }

    public void Init(Vector3 pos)
    {
        if (balls == null)
            Start();
         
        transform.position = pos;
        transform.rotation = Random.rotation;
        gameObject.SetActive(true);

        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].transform.localScale = Vector3.one * 0.001f;
            ballExpanstionTimes[i] = timeManager.WorldTime + (Random.value * offsetTime);
        }
    }
}

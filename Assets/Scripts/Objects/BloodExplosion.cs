using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodExplosion : MonoBehaviour
{
    Transform[] splats;
    float[] splatExpanstionTimes;
    TimeManager timeManager;
    const float explosionTime = 0.2f;
    const float offsetTime = 0.15f;
    Material sharedMaterial;
    static float sharedMatCutoff;
    static int lastFrameUpdated;

    public void Start()
    {
        splats = new Transform[transform.childCount];
        splatExpanstionTimes = new float[transform.childCount];
        timeManager = GameManager.ActiveGameManager.TimeManager;

        for (int i = 0; i < transform.childCount; i++)
            splats[i] = transform.GetChild(i);

        if (sharedMaterial == null)
            sharedMaterial = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
        sharedMatCutoff = 0.1f;

    }

    private void Update()
    {
        bool complete = true;

        //all of the balls expand and retract in one second,
        //starting at different times.
        //once theyve all finished, its complete.
        for (int i = 0; i < splats.Length; i++)
        {
            //I dont know why.. but times are not initialized..
            if (splatExpanstionTimes[i] < 0.1f)
                splatExpanstionTimes[i] = timeManager.WorldTime + Random.value * offsetTime;

            float t = (timeManager.WorldTime - splatExpanstionTimes[i]);

            if (t > 0 && t < explosionTime)
            {
                splats[i].transform.localScale = Vector3.one * (t *  (1.0f / explosionTime));
            }

            if (t < explosionTime)
            {
                complete = false;
            }
        }

        if (complete)
            gameObject.SetActive(false);
        else if(Time.frameCount > lastFrameUpdated)
        {
            sharedMaterial.SetFloat("_Cutoff", sharedMatCutoff);
            sharedMatCutoff += timeManager.WorldDeltaTime * 2.5f;

            if (sharedMatCutoff > 1f)
                sharedMatCutoff = 1f;
            lastFrameUpdated = Time.frameCount;
        }
    }

    public void Init(Vector3 pos)
    {
        if (splats == null)
            Start();

        transform.position = pos;
        transform.rotation = Random.rotation;
        gameObject.SetActive(true);

        for (int i = 0; i < splats.Length; i++)
        {
            splats[i].transform.localScale = Vector3.one * 0.001f;
            splatExpanstionTimes[i] = timeManager.WorldTime + (Random.value * offsetTime);
        }
        sharedMatCutoff = 0.1f;
    }
}

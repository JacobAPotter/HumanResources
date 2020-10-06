using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatCube : MonoBehaviour
{
    Renderer[] rats;
    Vector3[] end;
    TimeManager timeManager;
    Bounds bounds;

    private void Start()
    {
        rats = new Renderer[transform.childCount-1];
        end = new Vector3[transform.childCount-1];
        int index = 0;
        for(int i =0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name != "bounds")
            {
                rats[index] = transform.GetChild(i).GetComponent<Renderer>();
                rats[index].enabled = false;
                index++;
            }
        }

        timeManager = GameManager.ActiveGameManager.TimeManager;
        Renderer rend = transform.Find("bounds").GetComponent<Renderer>();
        bounds = rend.bounds;
        rend.enabled = false;
    }

    private void Update()
    {
       for(int i =0;i < rats.Length; i++)
        {
            if(rats[i].enabled)
            {
                rats[i].transform.position = Vector3.MoveTowards(
                                             rats[i].transform.position, end[i],
                                             timeManager.WorldDeltaTime * 5f);

                float dist = Vector3.Distance(rats[i].transform.position, end[i]);

                if (dist < 0.2f)
                    rats[i].enabled = false;

            }
            else
            {
                //once every n seconds
                const int n = 4;
                if(Random.value < timeManager.WorldDeltaTime / n)
                {
                    rats[i].enabled = true;
                    rats[i].transform.position = bounds.ClosestPoint(
                                                 bounds.center + 
                                                 (Random.insideUnitSphere - Random.insideUnitSphere).normalized
                                                 * bounds.size.magnitude);

                    //a vector pointing from the start to a random point in the bounds 
                    Vector3 line = (rats[i].transform.position - bounds.center).normalized * 
                                   bounds.size.magnitude + new Vector3(
                                                                 bounds.extents.x * (Random.value - 0.5f),
                                                                 0,
                                                                 bounds.extents.z * (Random.value - 0.5f)) * 20;

                    end[i] = bounds.ClosestPoint(rats[i].transform.position + line);

                    line = rats[i].transform.position - end[i];

                    rats[i].transform.rotation = Quaternion.Euler(
                            rats[i].transform.rotation.eulerAngles.x,
                            (-Mathf.Atan2(line.z, line.x) * Mathf.Rad2Deg) + 90,
                            rats[i].transform.rotation.eulerAngles.z);

                   // rats[i].transform.rotation = Quaternion.LookRotation(line);
                }
            }
        }
    }
}

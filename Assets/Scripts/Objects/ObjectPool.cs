using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//generic class for using object pools
//for bullets, enemies, etc
public class ObjectPool
{
    protected GameObject[] pool;
    int index;

    public ObjectPool(Transform parentTransform, int count)
    {

        pool = new GameObject[count];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = GameObject.Instantiate(parentTransform.GetChild(i % parentTransform.childCount).gameObject);
            pool[i].transform.parent = parentTransform;
            pool[i].name = i.ToString();
            pool[i].SetActive(false);
        }

        for (int i = 0; i < parentTransform.childCount; i++)
            parentTransform.GetChild(i).gameObject.SetActive(false);
    }

    public ObjectPool(GameObject[] clones, int count, Transform parentTransform)
    {

        pool = new GameObject[count];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = GameObject.Instantiate(clones[i % clones.Length]);
            pool[i].transform.parent = parentTransform;
            pool[i].name = i.ToString();
            pool[i].SetActive(false);
        }

        for (int i = 0; i < clones.Length; i++)
            clones[i].gameObject.SetActive(false);
    }

    public GameObject GetNext()
    {
        index++;

        if (index >= pool.Length)
            index = 0;

        return pool[index];
    }


    public GameObject GetNext(out int objectIndex)
    {
        index++;

        if (index >= pool.Length)
            index = 0;

        objectIndex = index;

        return pool[index];
    }

    public GameObject[] Pool
    {
        get
        {
            return pool;
        }
    }
}

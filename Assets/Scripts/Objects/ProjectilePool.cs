using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool  
{
    protected Projectile[] pool;
    int count;
    int index;

    public ProjectilePool(GameObject[] clones, Transform parentTransform, int count, string prefix)
    {

        this.count = count;

        pool = new Projectile[count];

        for (int i = 0; i < count; i++)
        {
            pool[i] = GameObject.Instantiate(clones[i % clones.Length]).GetComponent<Projectile>();
            pool[i].transform.parent = parentTransform;
            pool[i].name = prefix +  i.ToString();
            pool[i].Start();
            pool[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < clones.Length; i++)
            clones[i].gameObject.SetActive(false);
    }

    public Projectile GetNext()
    {
        index++;

        if (index >= count)
            index = 0;

        return pool[index];
    }
}

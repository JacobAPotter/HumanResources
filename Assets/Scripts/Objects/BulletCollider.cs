using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    Health health;
    protected bool killBullet;

    protected virtual void Start()
    {
        health = GetComponent<Health>();
        killBullet = true;
    }

    public void BulletCollision(Projectile p)
    {
        if (health)
        {
            health.DamageHealth(p.Damage);
             
            if (GetComponent<Enemy>())
                GetComponent<Enemy>().AlertToPlayerPresence();
        }
    }

    public bool KillBullet
    {
        get { return killBullet; }
    }
}

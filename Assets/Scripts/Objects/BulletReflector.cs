using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflector : BulletCollider
{
    protected override void Start()
    {
        base.Start();
        killBullet = false;
    }
}

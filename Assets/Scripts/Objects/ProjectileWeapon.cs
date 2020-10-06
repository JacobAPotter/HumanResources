using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{

    public enum PROJECTILE_TYPE
    {
        BULLET,
        SPIKE
    }
    protected float fireRate;
    protected float fireTimeStamp;
    protected TimeManager timeManager;

    protected PROJECTILE_TYPE PROJECTILE;

    protected Transform firePoint;
    protected float radius;
    protected float firePointY;

    private void Start()
    {
        timeManager = GameManager.ActiveGameManager.TimeManager;
        firePoint = transform.Find("firePoint");
        Init();
    }

    protected virtual void Init()
    {
        fireRate = 0.75f;
        PROJECTILE = PROJECTILE_TYPE.BULLET;
        radius = GetComponent<Collider>().bounds.extents.x * Mathf.Sqrt(2) * 1.01f;
        firePointY = firePoint.transform.position.y;
    }

    protected virtual void Update()
    {
        
    }

    public void TryFire(Vector3 direction)
    {
        TryFire(firePoint.position + (direction.normalized * radius), direction);
    }

    public void TryFire(Vector3 position, Vector3 direction)
    {
        if (timeManager.WorldTime - fireTimeStamp > fireRate)
        {
            fireTimeStamp = timeManager.WorldTime;
            if (PROJECTILE == PROJECTILE_TYPE.BULLET)
                GameManager.ActiveGameManager.ProjectileManager.InitNextBullet(position, direction.normalized, this);
            else if (PROJECTILE == PROJECTILE_TYPE.SPIKE)
                GameManager.ActiveGameManager.ProjectileManager.InitNextSpike(position, direction.normalized, this);

        }
    }

    public void ForceFire(Vector3 position, Vector3 direction)
    {
            fireTimeStamp = timeManager.WorldTime;
            if (PROJECTILE == PROJECTILE_TYPE.BULLET)
                GameManager.ActiveGameManager.ProjectileManager.InitNextBullet(position, direction.normalized, this);
            else if (PROJECTILE == PROJECTILE_TYPE.SPIKE)
                GameManager.ActiveGameManager.ProjectileManager.InitNextSpike(position, direction.normalized, this);
    }

    public void ForceFire( Vector3 direction)
    {
        ForceFire(firePoint.position, direction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    TrailRenderer trailRenderer;
    Rigidbody rigid;
    float trailTime;
    const float lifeSpan = 4f;
    float timeStamp;
    [SerializeField]
    float speed = 15f;
    int damage = 1;
    TimeManager timeManager;

    ProjectileWeapon firerWeapon;

    public void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer)
            trailTime = trailRenderer.time;
        rigid = GetComponent<Rigidbody>();
        timeManager = GameManager.ActiveGameManager.TimeManager;
    }

    void Update()
    {
        if (trailRenderer)
            trailRenderer.time = trailTime / timeManager.Coefficient;

        //kill the bullet if life span is over or speed has dropped.
        //to get actual magnitude needs to be divided by square time coeffiecient 
        //(its squared because using sqrMag)
        if (timeManager.WorldTime - timeStamp > lifeSpan ||
            rigid.velocity.sqrMagnitude / Mathf.Pow(timeManager.Coefficient, 2) < 25f)
            Kill();
    }

    public void Init(Vector3 startPos, Vector3 direction, ProjectileWeapon weapon)
    {
        gameObject.SetActive(true);
        timeStamp = timeManager.WorldTime;

        transform.position = startPos;
        transform.forward = direction;

        TimeAffectedRigidbody ta = GetComponent<TimeAffectedRigidbody>();
        if (ta)
            ta.ResetTimeAffected();

        rigid.velocity = direction * speed;
        this.firerWeapon = weapon;

        if (trailRenderer)
            trailRenderer.Clear();
    }

    public int Damage
    {
        get { return damage; }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletCollider bulletCollider = collision.collider.GetComponent<BulletCollider>();
        
        if (bulletCollider)
        {
            ProjectileWeapon weapon = bulletCollider.GetComponent<ProjectileWeapon>();
            //dont do anything if this projectile collides with
            //the person who shot it
            if (weapon != firerWeapon)
            {
                bulletCollider.BulletCollision(this);

                if (bulletCollider.KillBullet)
                    Kill();
                else
                {
                    //It has a bullet collider but is set to not 'kill' the bullet.
                    //so we assume the bullet should 'Bounce' off the surface
                    foreach (ContactPoint c in collision.contacts) //Find collision point
                    {
                        rigid.velocity = Quaternion.AngleAxis(180, c.normal) * transform.forward * -1;
                        rigid.velocity = rigid.velocity.normalized * speed * timeManager.Coefficient;
                        rigid.transform.forward = rigid.velocity.normalized;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BulletCollider bulletCollider = other.GetComponent<BulletCollider>();

        if (bulletCollider)
        {
            ProjectileWeapon weapon = bulletCollider.GetComponent<ProjectileWeapon>();
            //dont do anything if this projectile collides with
            //the person who shot it
            if (weapon != firerWeapon)
            {
                bulletCollider.BulletCollision(this);

                if (bulletCollider.KillBullet)
                    Kill();
            }
        }
    }

    public void Kill()
    {
        GameManager.ActiveGameManager.FXManager.GetBulletHit(transform.position);
        gameObject.SetActive(false);
    }
}

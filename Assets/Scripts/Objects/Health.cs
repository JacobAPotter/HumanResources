using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    int health;

    [SerializeField]
    int maxHealth;

    static List<Health> allHealth;

    FXManager fxManager;

    private void Start()
    {
        if (allHealth == null)
            allHealth = new List<Health>();

        allHealth.Add(this);

        Init();

        fxManager = GameManager.ActiveGameManager.FXManager;
    }

    public void DamageHealth(int amount)
    {
        if(health > 0)
            fxManager.GetDamageAni(transform);

        health -= amount;

        if(health <= 0)
        {
            if(GetComponent<Death>())
            {
                GetComponent<Death>().Die();
            }
            else 
                gameObject.SetActive(false);
        }
    }

    public void Init(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.health = maxHealth;
    }

    public void Init()
    {
        this.health = maxHealth;
    }

    public bool Alive
    {
        get { return health > 0; }
    }

    public static List<Health> AllHealthComponents
    {
        get { return allHealth;  }
    }

    public bool HasBeenDamaged
    {
        get { return health < maxHealth; }
    }

    private void OnDestroy()
    {
        allHealth.Remove(this);
    }
}

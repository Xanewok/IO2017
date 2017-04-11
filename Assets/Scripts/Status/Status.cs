using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public delegate void HealthChangedHandler(GameObject obj, float health);
    public event HealthChangedHandler healthChanged;

    public static int auraNumber = 1;
    // all auras should be specified there (there might be used only in sth else)
    public static int defenceProc = 0; // Minimizing % of damage. Shouldn't have to big values

    public static float maxHealth = 100f;

    protected float health = maxHealth;
    protected AuraContainer<float>[] simpleAuras = new AuraContainer<float>[auraNumber];

    void Start()
    {
        for (int i=0; i<auraNumber; i++)
        {
            simpleAuras[i] = new AuraFloatMax();
        }
    }

    public virtual float getHealth()
    {
        return health;
    }

    // type - type of damage
    public virtual void hurt(float damage, int type)
    {
        health = Mathf.Max(0f, health - damage);

        healthChanged(gameObject, health);
    }

    public AuraContainer<float> getAuras(int num)
    {
        return simpleAuras[num];
    }
}

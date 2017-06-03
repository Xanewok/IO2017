using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    public delegate void HealthChangedHandler(GameObject obj, float health);
    public event HealthChangedHandler healthChanged;

    /// <summary>
    /// Dash movement speed
    /// </summary>
    public Vector3 dashMovement = Vector3.zero;

    /// <summary>
    /// Maximum health of object
    /// </summary>
    [Tooltip("Maximum health of object")]
    public float maxHealth = 100f;

    protected float health = 1f;
    
    void Start()
    {
        onStart();
    }

    public virtual void onStart()
    {
        health = maxHealth;
    }

    public virtual float getHealth()
    {
        return health;
    }

    public virtual float getMaxHealth()
    {
        return maxHealth;
    }

    // type - type of damage
    public virtual void hurt(float damage, int type)
    {
        health = Mathf.Max(0f, health - damage);

        healthChanged(gameObject, health);
    }
}

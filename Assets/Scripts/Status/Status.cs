using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    public delegate void HealthChangedHandler(GameObject obj, float health);
    public event HealthChangedHandler healthChanged;

    public delegate void AuraChangedHandler(int aura, float oldValue, float newValue);
    public event AuraChangedHandler auraChanged;

    /*
        All auras:
        - for special uses for items, add for which one
    */
    /// <summary>
    /// Ethereal: 0-1
    /// - negating % damage
    /// - makes you unable to use certain items when too low/big
    /// - makes you go through certain walls when aroundd 0.5
    /// </summary>
    public const int Ethereal = 0;
    /// <summary>
    /// Concentration: 0-inf
    /// - adding % damage to certain weapons (mostly precision-like)
    /// - makes you % slower
    /// - might give some other uses for items
    /// - would be nice if it could alse change player's behaviour somehow.
    /// </summary>
    public const int Concentration = 1;
    /// <summary>
    /// Battle Rage: 0-inf
    /// - adding % damage to certain weapons (mostly brutal ones)
    /// - makes you % quicker
    /// - might give some other uses for items.
    /// - would be nice if it could alse change player's behaviour somehow.
    /// </summary>
    public const int BattleRage = 2;
    /// <summary>
    /// Faith: 0-inf
    /// - adding % danage to certain weapons (magic-like, especially good)
    /// - give other uses to weapons and items
    /// </summary>
    public const int Faith = 3;
    /// <summary>
    /// Hope: 0-inf
    /// - adding big % damage to all weapons when low on health
    /// - changes some weapons when low on health
    /// - special. Higher values (more than 0.45) should be used sparingly, on special items.
    /// </summary>
    public const int Hope = 4;
    /// <summary>
    /// Covered in water: 0-1-2-3?
    /// - gives some margin of fire resistance
    /// - special uses for some items
    /// - payer should be literally soaking wet, when more than 
    /// </summary>
    public const int WaterCovered = 5;
    /// <summary>
    /// On fire: 0-1-2-3?
    /// - Damage over time
    /// - Resistance to cold like
    /// - Special uses for items
    /// - visible effects
    /// </summary>
    public const int OnFire = 6;
    /// <summary>
    /// Insanity: 0-inf
    /// - Mostly bad effects (like shuffling inventory, misuse on some items)
    /// - might add % damage on some items (heretic related?)
    /// </summary>
    public const int Insanity = 7;
    /// <summary>
    /// Number of all auras.
    /// </summary>
    public const int auraNumber = 7;

    /// <summary>
    /// Maximum health of object
    /// </summary>
    [Tooltip("Maximum health of object")]
    public float maxHealth = 100f;

    /*
        Damage types:
    */
    /// <summary>
    /// Weird type of damage. Cosmic evergy/void/...
    /// Something that is not to be resisted. Ever.
    /// </summary>
    public const int IrreversibleDamage = 0;
    /// <summary>
    /// Precise damage: slashed, pierced, impaled, shot (mostly)
    /// </summary>
    public const int PreciseDamage = 1;
    /// <summary>
    /// Brutal damage: bludgeoned, smashed, shot (from big weapon), exploded
    /// </summary>
    public const int BrutalDamage = 2;
    /// <summary>
    /// Fire and hot environments.
    /// </summary>
    public const int FireDamage = 3;
    /// <summary>
    /// Ice and cold environments.
    /// </summary>
    public const int IceDamage = 4;
    /// <summary>
    /// Godly light that shines upon us all.
    /// Mainly damaging for everything that is dark be its nature.
    /// </summary>
    public const int HolyDamage = 5;
    /// <summary>
    /// The shadow of darkness, the hatred and fear.
    /// Mainly damaging for everything good. (player included)
    /// </summary>
    public const int DarkDamage = 6;


    protected float health = 1f;
    protected AuraContainer<float>[] simpleAuras = new AuraContainer<float>[auraNumber];

    void Start()
    {
        fillAuras();
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

    protected virtual void fillAura(int aura)
    {
        simpleAuras[aura] = new AuraFloatMax();
    }

    protected virtual void fillAuras()
    {
        for (int i = 0; i < auraNumber; i++)
        {
            fillAura(i);
            addHandler(i);
        }
    }

    protected void addHandler(int aura)
    {
        simpleAuras[aura].auraUpdated += (oldVal, newVal) => auraChanged(aura, oldVal, newVal);
    }
}

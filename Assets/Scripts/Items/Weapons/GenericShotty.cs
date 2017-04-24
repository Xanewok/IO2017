using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic component for shotgun weapon.
/// - brutal - bonus damage on battle rage (specified on weapon script)
/// - manyBullet - shots more than one bullet
/// 
/// </summary>
public class GenericShotty : SimpleItem, AmmoReloadInterface {
    /// <summary>
    /// How many bullets are shot at once
    /// </summary>
    [Tooltip("How many bullets are shot at once.")]
    public int bullets = 3;

    /// <summary>
    /// How big is the spread (angle)
    /// </summary>
    [Tooltip("How big is the spread (angle)")]
    public float spread = 30f;

    /// <summary>
    /// Size of weapons magazine
    /// </summary>
    [Tooltip("Size of weapons magazine")]
    public int magazine = 2;

    /// <summary>
    /// Bullet prefab
    /// </summary>
    [Tooltip("Bullet prefab")]
    public GameObject bullet;

    /// <summary>
    /// Bullet speed
    /// </summary>
    [Tooltip("Bullet speed")]
    public float startingSpeed = 2f;

    /// <summary>
    /// Bullets in magazine
    /// </summary>
    protected int actualMagazine = 0;

    /// <summary>
    /// Counting time between shots.
    /// </summary>
    protected float cooldown = 0f;


    /// <summary>
    /// Triggers on start.
    /// Sets weapon magazine to full.
    /// </summary>
    protected override void onStart()
    {
        base.onStart();
        actualMagazine = magazine;
    }

    /// <summary>
    /// Triggered on updating. Used to count cooldown between shots
    /// </summary>
    protected override void onUpdate()
    {
        base.onUpdate();
        cooldown = Mathf.Max(0f, cooldown - Time.deltaTime);
    }

    /// <summary>
    /// Shooting bullets
    /// </summary>
    public override void onUseStart()
    {
        if (Mathf.Approximately(cooldown, 0f))
        {
            if (actualMagazine > 0)
            {
                actualMagazine--;
                System.Random rng = new System.Random();
                Rigidbody wearers = wearer.GetComponent<Rigidbody>();
                for (int a = 0; a < bullets; a++)
                {
                    Vector3 addPos = Vector3.zero;
                    if (wearers != null) addPos = wearers.velocity * Time.deltaTime;
                    float randomSpread = (float)rng.NextDouble();
                    Quaternion rotation = transform.rotation * Quaternion.Euler(0f, randomSpread * spread - spread / 2f, 0f);
                    GameObject bull = Instantiate(bullet, transform.position + addPos, rotation);

                    Rigidbody rigid = bull.GetComponent<Rigidbody>();
                    rigid.velocity = rotation * Vector3.forward * startingSpeed;
                }
            }
            else
            {
                
            }
        } 
    }

    /// <summary>
    /// Checking if its correct type of ammo
    /// </summary>
    /// <param name="ammo">Ammo class</param>
    /// <returns>true if can be used, false otherwise</returns>
    public bool canUseAmmunition(SimpleAmmunition ammo)
    {
        if (ammo.type == SimpleCollectible.typeShotgunShells)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adding one shell to magazine
    /// </summary>
    /// <param name="ammo">Unused</param>
    public void addAmmunition(AmmoType ammo)
    {
        magazine++;
    }
}

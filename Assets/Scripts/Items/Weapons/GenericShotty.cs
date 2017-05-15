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
public class GenericShotty : AnimatorControlledObject, AmmoReloadInterface {
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
    /// Number of ammo clips on weapon
    /// </summary>
    [Tooltip("Number of ammo clips on weapon. 0 for Infinity")]
    public int magazines = 5;

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
    /// Triggers on start.
    /// Sets weapon magazine to full.
    /// </summary>
    protected override void onStart()
    {
        base.onStart();
        actualMagazine = magazine;
    }

    /// <summary>
    /// Shooting bullet.
    /// Should be called only on animator event
    /// </summary>
    public void shootBullet()
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

    public void reloadWeapon()
    {
        actualMagazine = magazine;
        magazines--;
        if (magazines == 0)
        {
            removeObject();
            Destroy(gameObject);
        }
        else if (magazines < 0) magazines = -1;
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

    public override void onUseStart()
    {
        base.onUseStart();
        if (actualMagazine > 0)
        {
            animator.SetTrigger("Shoot");
        }
        else
        {
            animator.SetTrigger("Reload");
        }
    }

	protected void hideChildren () {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (false);
		}
	}

	protected void showChildren () {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (true);
		}
	}

	public override void onDropDown (GameObject player, Inventory inventory)
	{
		base.onDropDown (player, inventory);
		showChildren ();
	}

	public override void onUnEquip ()
	{
		base.onUnEquip ();
		hideChildren ();
	}

	public override void onEquip (int hand)
	{
		base.onEquip (hand);
		showChildren ();
	}

	public override void onPickUp (GameObject player, Inventory inventory)
	{
		base.onPickUp (player, inventory);
		hideChildren ();
	}
}

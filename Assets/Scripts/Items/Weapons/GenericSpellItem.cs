using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpellItem : AnimatorControlledObject {

    /// <summary>
    /// How big is the spread (angle)
    /// </summary>
    [Tooltip("How big is the spread (angle)")]
    public float spread = 30f;

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
    /// Shooting bullet.
    /// Should be called only on animator event
    /// </summary>
    public void useSpell()
    {
        System.Random rng = new System.Random();
        Rigidbody wearers = wearer.GetComponent<Rigidbody>();
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

    /// <summary>
    /// Destroying spell.
    /// Should be called on animator
    /// </summary>
    public void removeSpell()
    {
        removeObject();
        Destroy(gameObject);
    }

}

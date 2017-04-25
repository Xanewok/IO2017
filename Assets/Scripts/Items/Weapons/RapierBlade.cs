using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapierBlade : MonoBehaviour {

    public float damage = 10f;
    public int damageType = 0;
    [Tooltip("For animator: should the blade damage objects.")]
    public bool damages = false;
    [Tooltip("For animator: what is our forward speed.")]
    public float forwardSpeed = 0f;

    public float speedThreshold = 1f;
    protected int speedToken;
    protected bool givenSpeedToken = false;
    protected Quaternion rotation;
    protected bool equipped = false;

    Collider collid;
    MeshRenderer meshRender;
    GameObject wearer;

    void Start()
    {
        collid = gameObject.GetComponent<Collider>();
        meshRender = gameObject.GetComponent<MeshRenderer>();
    }

    void setDashMovement()
    {
        Rigidbody rigid = wearer.GetComponent<Rigidbody>();
        Vector3 velocity = rigid.velocity;
        wearer.GetComponent<Status>().dashMovement = rigid.transform.rotation * Vector3.forward * forwardSpeed;
    }

    void FixedUpdate()
    {
        if (this.wearer != null && equipped)
        {
            setDashMovement();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (wearer != null && collision.gameObject.Equals(wearer))
            return;
        Status stat = collision.gameObject.GetComponent<Status>();
        if (stat != null)
        {
            stat.hurt(damage, damageType);
        }
    }

    /// <summary>
    /// To be rewritten (probably all script)
    /// Removing durability of weapon.
    /// </summary>
    public void loseDurability()
    {
        transform.parent.parent.gameObject.GetComponent<RapierSword>().loseDurability();
    }

    public void onEquip()
    {
        collid.enabled = true;
        meshRender.enabled = true;
        equipped = true;
    }

    public void onUnEquip()
    {
        collid.enabled = false;
        meshRender.enabled = false;
        equipped = false;
        wearer.GetComponent<Status>().dashMovement = Vector3.zero;
    }

    public void onPick(GameObject wearer)
    {
        collid.enabled = false;
        meshRender.enabled = false;
        this.wearer = wearer;
    }

    public void onDrop()
    {
        collid.enabled = false;
        meshRender.enabled = true;
        this.wearer = null;
    }

    /// <summary>
    /// Used on removing object, to nulify dash movement speed
    /// </summary>
    public void onRemove()
    {
        forwardSpeed = 0f;
        setDashMovement();
    }

}

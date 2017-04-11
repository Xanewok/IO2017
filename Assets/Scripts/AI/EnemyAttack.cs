using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // TODO: Extract more generic interface (i.e. don't rely on bullets)
    public GameObject bullet;
    public float speed = 10.0f;
    public float attackCooldown = 1.0f;

    float lastAttackTime = 0.0f;

    public void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            lastAttackTime = Time.time;

            var bulletObject = Instantiate(bullet, transform.position + transform.forward * 1.5f, transform.rotation);
            Rigidbody rigid = bulletObject.GetComponent<Rigidbody>();
            rigid.velocity = transform.forward * speed;
        }
    }
}

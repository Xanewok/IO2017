using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{

    public float speed = 6;
    public int playerNum = 0;

    Vector3 movement = Vector3.zero;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal_" + playerNum.ToString());
        float v = Input.GetAxis("Vertical_" + playerNum.ToString());
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObstacleMover : MonoBehaviour
{
    public Vector3 movementVector;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	private void FixedUpdate()
    {
        Vector3 movement = movementVector * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
	}
}

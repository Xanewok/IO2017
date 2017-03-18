// Used to test camera follow
// TODO: Remove me
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Rigidbody rb;
	// Use this for initialization
	void Start()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        rb.velocity = Vector3.forward;
	}
}

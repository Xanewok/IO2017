using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantTransform : MonoBehaviour {

	public bool constantLocalPosition;
	public Vector3 localPosition;
	
	// Update is called once per frame
	void LateUpdate () {
		if (constantLocalPosition)
			transform.localPosition = localPosition;
	}
}

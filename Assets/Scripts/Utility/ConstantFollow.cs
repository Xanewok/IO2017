using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantFollow : MonoBehaviour {

	public GameObject follow;
	public bool cameraRotation;

	void LateUpdate () {
		if (follow != null)
			transform.position = follow.transform.position;
		if (cameraRotation) {
			Camera cam = Camera.current;
			if (cam != null) {
				Vector3 rot = cam.transform.rotation.eulerAngles;
				transform.rotation = Quaternion.Euler (rot);
			}
		}
	}
}

using UnityEngine;
using System.Collections

public class Info : MonoBehaviour {
	private String message; 
	private Vector3 location;
	private float time;
	
	void Info(string message, Vector3 location, float time) {
		this.message = message;
		this.location = location;
		this.time = time;
	}
	
	string getMessage() {
		return this.message;
	}
	
	Vector3 getlocation() {
		return this.location;
	}
	
	float date() {
		return this.time;
	}
}

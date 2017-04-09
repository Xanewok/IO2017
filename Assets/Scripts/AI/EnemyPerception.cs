using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Info {
	public string message; 
	public Vector3 location;
	public float time;

	public Info(string message, Vector3 location, float time) {
		this.message = message;
		this.location = location;
		this.time = time;
	}
}

public class EnemyPerception : MonoBehaviour {
	public float hearingDistance;
	public float sightDistance;
	public float sightAngle;
	public float detectionInterval = 0.2f;

	protected GameObject[] players;
	protected List<Info> infos = new List<Info>();
	
	void Start() {
		players =  GameObject.FindGameObjectsWithTag("Player");
		StartCoroutine(Perception());
	}
	
	IEnumerator Perception() {
		while (true) {
			infos.Clear();

			int i = 0;
			foreach(GameObject player in players) {
				Vector3 rayDirection = player.transform.position - transform.position;
				float distance = rayDirection.magnitude;
				float angle = Vector3.Angle(rayDirection, transform.forward);
				// Player is in sight area
				if (distance < sightDistance && angle < sightAngle) {
					RaycastHit hitInfo;
					bool hit = Physics.Raycast(transform.position, rayDirection, out hitInfo, sightDistance);
					// Player is seen
					if (hit && hitInfo.transform.tag == "Player") {
						Info info = new Info("Player " + i + " seen", player.transform.position, Time.time);
						infos.Add(info);
					}
				}
				/* Player is heard */
				else if (distance < hearingDistance) {
					Info info = new Info("Player " + i + " heard", player.transform.position, Time.time);
					infos.Add(info);
				}
				i++;
			}
		}
	}	
	
	List<Info> getInfos() {
		return infos;
	}
}

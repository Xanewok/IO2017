using UnityEngine;
using System.Collections

public class EnemyBasicAI : MonoBehaviour {
	protected GameObject[] Players;
	protected List<Info> Infos;
	protected float hearingDistance; /*Add constants*/
	protected float sightDistance;
	protected float sightAngle;
	protected float detectionInterval = 0.2f;
	
	void Start () {
		Player =  GameObject.FindGameObjectsWithTag("Player");
		Infos = new List<Info>;
		StartCoroutine(Perception());
	}
	
	IEnumerator Perception() {
		for(;;) {
			Infos.Clear();
			int i = 0;
			foreach(player in Players) {
					Vector3 rayDirection = player.transform.position - transform.position;
					float distance = rayDirection.distance();
					float angle = Vector3.Angle(rayDirection, transform.forward);
					/* Player is in sight area */
					if(distance < sightDistance && angle < sightAngle) {
						RaycastHit hit;
						bool hited = Physics.Raycast(transform.position, rayDirection, out hit, sightDistance);
						/* Player is seen */
						if(hited && hit.position.tag == 'Player') {
							Info info("Player " + i + " seen", player.transform.position, Time.time);
							Infos.add(info);
						}
					}
					/* Player is heard */
					else if(distance < hearingDistance) {
						Info info("Player " + i + " heard", player.transform.position, Time.time);
						Infos.add(info);
					}
					++i;
			}
		}
	}	
	
	List<Info> getInfos() {
		return Infos;
	}
}
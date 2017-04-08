using UnityEngine;
using System.Collections

public class EnemyBasicAI : MonoBehaviour {
	public enum PERCEPTION_CASE {NON = 0, HEARED = 1, SIGHTED = 2};
	protected GameObject[] Players;
	/*Co, kiedy, gdzie */
	using Info_t = tuple<int, float, Vector3>;
	protected Info_t[] Info[]; 
	protected float hearingDistance;
	protected float sightDistance;
	protected float sightAngle;
	protected float detectionInterval = 0.2f;
	
	void Start () {
		Player =  GameObject.FindGameObjectsWithTag("Player");
		StartCoroutine(Perception());
	}
	
	IEnumerator Perception() {
		for(;;) {
			int i = 0;
			foreach(player in Players) {
					PERCEPTION_CASE perception = PERCEPTION_CASE.NON;
					Vector3 rayDirection = player.transform.position - transform.position;
					float distance = Vector3.distance(rayDirection, 0);
					float angle = Vector3.Angle(rayDirection, transform.forward);
					/* Player is in sight area */
					if(distance < sightDistance && angle < sightAngle) {
						RaycastHit hit;
						Physics.Raycast(transform.position, rayDirection, hit, sightDistance);
						/* Player is seen */
						if(hit.position.tag == 'Player') {
							perception = PERCEPTION_CASE.SIGHTED;
						}
					}
					/* Player is heared */
					else if(distance < hearingDistance) {
						perception = PERCEPTION_CASE.HEARED;
					}
					/* Add interesting point */
					if(perception != PERCEPTION_CASE.NON) {
						Info_t playerInfo = 
						new Tuple<PERCEPTION_CASE, float, Vector3>(perception, Time.time, player.transform.position);
						PlayersInfo[i] = playerInfo;
						++i;
					}
			}
		}
	}	
	
	Info_t[] getInfo() {
		return Info;
	}
}
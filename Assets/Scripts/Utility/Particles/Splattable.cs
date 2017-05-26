using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Splattable.
/// Let object be covered with decal
/// </summary>
public class Splattable : MonoBehaviour {

	[Tooltip("What container should remember particles on object. On null, looks for one in instance.")]
	public DecalContainer decalContainer;

	// Use this for initialization
	void Start () {
		if (decalContainer == null) {
			decalContainer = GameController.Instance.gameObject.GetComponent<DecalContainer> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnParticleSplat(ParticleCollisionEvent particleCollisionEvent, float size, Gradient colorGradient) {
		if (decalContainer != null) {
			Vector3 position = particleCollisionEvent.intersection;
			Vector3 particleRotationEuler = Quaternion.LookRotation (particleCollisionEvent.normal).eulerAngles;
			particleRotationEuler.z = Random.Range (0, 360);
			Color color = colorGradient.Evaluate (Random.Range (0f, 1f));
			decalContainer.AddDecal (position, size, particleRotationEuler, color);
		}
	}

	public static void SplatOnGlobal(ParticleCollisionEvent particleCollisionEvent, float size, Gradient colorGradient) {
		DecalContainer global = GameController.Instance.gameObject.GetComponent<DecalContainer> ();
		if (global != null) {
			Vector3 position = particleCollisionEvent.intersection;
			Vector3 particleRotationEuler = Quaternion.LookRotation (particleCollisionEvent.normal).eulerAngles;
			particleRotationEuler.z = Random.Range (0, 360);
			Color color = colorGradient.Evaluate (Random.Range (0f, 1f));
			global.AddDecal (position, size, particleRotationEuler, color);
		}
	}
}

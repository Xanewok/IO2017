using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatOnCollision : MonoBehaviour {
	[Tooltip("ParticleSystem that should splat")]
	public ParticleSystem particleLauncher;
	[Tooltip("Colors of splatted particles")]
	public Gradient particleColorGradient;
	[Tooltip("Size on splat.")]
	public float size;

	List<ParticleCollisionEvent> collisionEvents;


	void Start () 
	{
		collisionEvents = new List<ParticleCollisionEvent> ();
		if (particleLauncher == null)
			particleLauncher = gameObject.GetComponent<ParticleSystem> ();
	}

	void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents (particleLauncher, other, collisionEvents);
		int i = 0;
		while (i < numCollisionEvents) 
		{
			Splattable splatObject = collisionEvents [i].colliderComponent.gameObject.GetComponent<Splattable> ();
			if (splatObject != null) {
				splatObject.OnParticleSplat (collisionEvents [i], size, particleColorGradient);
			} else if (collisionEvents [i].colliderComponent.gameObject.GetComponent<Rigidbody> () == null){ //Global only on static things
				Splattable.SplatOnGlobal(collisionEvents [i], size, particleColorGradient);
			}
			i++;
		}

	}

}
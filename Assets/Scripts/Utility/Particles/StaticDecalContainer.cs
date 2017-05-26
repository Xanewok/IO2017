﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticDecalContainer : DecalContainer {
	[Tooltip("Maximum number of particles this container can show.")]
	public int maxDecals = 100;

	public ParticleSystem decalParticleSystem;
	private int particleDecalDataIndex;
	private ParticleDecalData[] particleData;
	private ParticleSystem.Particle[] particles;

	// Use this for initialization
	void Start () {
		if (decalParticleSystem == null)
			decalParticleSystem = gameObject.GetComponent<ParticleSystem> ();
		particles = new ParticleSystem.Particle[maxDecals];
		particleData = new ParticleDecalData[maxDecals];
		for (int i = 0; i < maxDecals; i++) 
		{
			particleData [i] = new ParticleDecalData ();    
		}
	}

	public override void AddDecal(Vector3 position, float size, Vector3 rotationEuler, Color color) {
		if (particleDecalDataIndex >= maxDecals) 
		{
			particleDecalDataIndex = 0;
		}

		particleData [particleDecalDataIndex].position = position;
		particleData [particleDecalDataIndex].rotation = rotationEuler;
		particleData [particleDecalDataIndex].size = size;
		particleData [particleDecalDataIndex].color = color;

		particleDecalDataIndex++;
		DisplayParticles ();
	}

	void DisplayParticles()
	{
		for (int i = 0; i < particleData.Length; i++) 
		{
			particles [i].position = particleData [i].position;
			particles [i].rotation3D = particleData [i].rotation;
			particles [i].startSize = particleData [i].size;
			particles [i].startColor = particleData [i].color;
		}

		decalParticleSystem.SetParticles (particles, particles.Length);
	}
}

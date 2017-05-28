using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecalContainer : MonoBehaviour {
	public abstract void AddDecal(Vector3 position, float size, Vector3 rotationEuler, Color color);
}

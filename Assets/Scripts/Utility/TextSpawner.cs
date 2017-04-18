using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpawner : MonoBehaviour {

    public GameObject textObject;
	
	public void spawnText(string text)
    {
        GameObject spawnedText = Instantiate(textObject);
        spawnedText.GetComponentInChildren<UnityEngine.UI.Text>().text = text;
    }
}

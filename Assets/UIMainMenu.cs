using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}

    public void Play()
    {
        SceneManager.LoadScene("default");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

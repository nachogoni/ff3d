using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}

    void OnGUI()
    {
        GUI.Label(new Rect(270,100,300,300), "Game Over");
        if (GUI.Button(new Rect(150, 150, 300, 100), "RESTART"))
            restart();
    }

    void restart()
    {
        Application.LoadLevel(0);
    }

	// Update is called once per frame
	void Update () {
	
	}
}

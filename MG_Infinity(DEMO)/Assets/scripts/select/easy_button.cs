using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class easy_button : MonoBehaviour {
	SceneController scene = new SceneController();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick(){
		scene.chengeToEasy();
		Debug.Log(scene.difficultNum());
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; 
public class resultUI : MonoBehaviour {

	private int ceilingScore = 100000;
	private int scoreSum = 0;
	public ScoreCount finalScore = ScoreCount();
	
	public GameObject score_object = null;
	// Use this for initialization
	void Start () {
		if (Application.isEditor) {
			finalScore.bad = 0;
			finalScore.good = 0;
			finalScore.great = 0;
			finalScore.perfect = 0;
		} else { 
			finalScore = GameController.GetScoreCount();

		}
		
		Text scoreText = score_object.GetComponent<Text> ();
		scoreText.text = String.Format("{0:D6}", scoreSum);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

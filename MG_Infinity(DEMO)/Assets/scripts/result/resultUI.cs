using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; 
public class resultUI : MonoBehaviour {

	private int ceilingScore = 100000;
	public enum Detection{
		bad,
		good,
		great,
		perfect
	}
	private int totalScoreSum = 0;
	private Dictionary<string, int[]> Score;
	private Dictionary<string, int> ScoreSum = {
		{"Hit",20},
		{"Long",5},
		{"Swipe",3}
	};
	public GameObject score_object = null;
	// Use this for initialization
	void Start () {
		if (Application.isEditor) {
			Score.Add("Hit",{0,3,7,10});
			Score.Add("Long",{0,1,2,2});
			Score.Add("Swipe",{0,0,2,1});
		} else { 
			Score = GameController.GetScoreCount();
			//したいこと：ScoreSumを取得するためにScoreの各キーの和を求める
			ScoreSum["Hit"] = Score["Hit"].Sum();
			ScoreSum["Long"] = Score["Long"].Sum();
			ScoreSum["Swipe"] = Score["Swipe"].Sum();

		}
		
		Text scoreText = score_object.GetComponent<Text> ();
		scoreText.text = String.Format("{0:D6}", totalScoreSum);
		//したいこと各ノーツごとに結果の割合を詳細表示

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

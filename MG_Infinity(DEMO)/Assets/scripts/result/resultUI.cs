using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class resultUI : MonoBehaviour {

	private int SCORECEILING = 100000;
	public enum Detection {
		bad,
		good,
		great,
		perfect
	}
	private float totalScoreSum = 0F;
	private float totalScoreAccuracy = 0F;
	public Text scoreText;

	private Dictionary<string, int[]> Score = new Dictionary<string, int[]> ();
	private Dictionary<int, float> SCOREWEIGHT =  new Dictionary<int, float> ()
	{
		{(int)Detection.bad, 0.0F},
		{(int)Detection.good, 0.3F},
		{(int)Detection.great, 0.8F},
		{(int)Detection.perfect, 1.0F}
	};
	private Dictionary<string, int> ScoreSum = new Dictionary<string, int> ()
	{ { "Hit", 20 }, { "Long", 5 }, { "Swipe", 3 }
	};

	public GameObject score_object = null;
	// Use this for initialization
	void Start () {
		if (Application.isEditor) {
			Score.Add ("Hit", new int[] { 0, 3, 7, 10 });
			Score.Add ("Long", new int[] { 0, 1, 2, 2 });
			Score.Add ("Swipe", new int[] { 0, 0, 2, 1 });
		} else {
			Score = GameController.GetScoreCount ();
			//したいこと：ScoreSumを取得するためにScoreの各キーの和を求める
			ScoreSum["Hit"] = calcScoreSum (Score["Hit"]);
			ScoreSum["Long"] = calcScoreSum (Score["Long"]);
			ScoreSum["Swipe"] = calcScoreSum (Score["Swipe"]);
		}
		calcTotalScoreSum ();
		StartCoroutine(ScoreAnimator(totalScoreSum, 4F));
		//したいこと各ノーツごとに結果の割合を詳細表示

	}

	// Update is called once per frame
	void Update () {

	}

	int calcScoreSum (int[] arr) {
		int sum = 0;
		foreach (int temp_score in arr) {
			sum += temp_score;
		}
		return sum;
	}
	void calcTotalScoreSum () {
		totalScoreSum = 0F;
		float ScoreSumWeighted = 0F;
		int ScoreNotesCountAll = ScoreSum["Hit"] + ScoreSum["Long"] + ScoreSum["Swipe"];
		
		foreach (KeyValuePair<string, int[]> pair in Score){
			for (int det = 0; det < 4; det++){
				ScoreSumWeighted += SCOREWEIGHT[det] * pair.Value[det];
			}
		}
		totalScoreAccuracy = ScoreSumWeighted / ScoreNotesCountAll * SCOREWEIGHT[(int)Detection.perfect];
		totalScoreSum = (float)SCORECEILING * totalScoreAccuracy;
	}

	public void onClickReplay(){
 		SceneManager.LoadScene("play");
	}
	public void onClickContinue(){
 		SceneManager.LoadScene("select");
	}
	private IEnumerator ScoreAnimator(float generalScore, float duration)
	{

		scoreText = score_object.GetComponent<Text> ();
		// 
		// 開始時間
		float startFrame = Time.time;
 
		// 終了時間
		float endFrame = startFrame + duration;
 
		while (Time.time < endFrame){
			// 現在の時間の割合
			float timeRate = (Time.time - startFrame) / duration;
			float updateScore = generalScore * easeInOut(timeRate);// + generalScore; 
			// テキストの更新
 			scoreText.text = string.Format ("{0:D6}", (int) updateScore);
			// 1フレーム待つ
			yield return null;
			
		} 
 
		// 最終的な着地のスコア
		scoreText.text = string.Format ("{0:D6}", (int) generalScore);
	}
	private float easeInOut(float t){
		return - (Mathf.Cos(Mathf.PI*t) - 1.0F)/2.0F;
	}
}
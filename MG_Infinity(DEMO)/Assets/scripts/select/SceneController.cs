using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {

	songsInformation si = new songsInformation();

	[System.Serializable]
	public class songsInformation {
		public float hit_decision;
		public float speed;
		public listSongsInformation[] list;
	}

	[System.Serializable]// <-JSONへの変換に必要
	public class listSongsInformation
	{
		public int id;
		public string composer;
		public string title;
		public int play_count;
		public int[] difficulty;
	}

	//他シーンに渡す変数
	public static int musicID;
	public static int difficulty;
	public static string composer;
	public static string title;
	public static float speed, hit_decision;

	//このシーン内で使う変数
	private static int sceneDif; //scene内の難易度を扱う変数　easy:0,medium:1,hard:2,infinity:3
	private static int sortMode;
	 //sortした時のモード選択　sortのモードいくつあったっけ？（それぞれ割り当てるのじゃ）
	private static int musicSID;
	private static int path;
	private static int[] listPath;


	//他シーンで変数を読み込むとき用関数
	public static int getID(){
		return musicID;
	}
	public static int getDifficulty(){
		return difficulty;
	}
	public static string getComposer(){
		return composer;
	}
	public static string getTitle(){
		return title;
	}

	public static float getSpeed() {
		return speed;
	}

	public static float getDecision() {
		return hit_decision;
	}

	//returnボタン
	public void moveStart(){
		SceneManager.LoadScene("start");
	}

	//playボタン
	public void movePlay(){
		SceneManager.LoadScene("play");
	}

	//一つ上の曲
	public void upMusic(){
		path = path - 1;
	}

	//一つ下の曲
	public void downMusic(){
		path = path + 1;
	}

	//easyボタン
	public void chengeToEasy(){
		sceneDif = 0;
		Debug.Log(sceneDif);
	}
	//mediumボタン
	public void chengeToMedium(){
		sceneDif = 1;
		Debug.Log(sceneDif);
	}
	//hardボタン
	public void chengeToHard(){
		sceneDif = 2;
		Debug.Log(sceneDif);
	}
	//infinityボタン
	public void changeToInfinity(){
		sceneDif = 3;
		Debug.Log(sceneDif);
	}
	//sortボタン
	public void musicNameSort(){
		string[] name = new string[si.list.Length];
		listPath = new int[si.list.Length];
		for(int i = 0; i < si.list.Length; i++){
			name[i] = si.list[i].title;
		}
		Array.Sort(name);
		for(int i = 0; i < si.list.Length; i++){
			listPath[i] = Array.IndexOf(si.list,name[i]);
		}
		sortMode = 1;
		//nameに順番にtitle入れてソート
		//nameの文字列をsi.listで検索して、index順番にlistPathにぶち込む（これでlistPathを参照すれば元のindexがわかる...はず）
		//ちなみに、同じ名前があったら死ぬ。

		//必要な処理
		/*
		sortする。以上
		*/
	}

	public int difficultNum(){
		return sceneDif;
	}

	// Use this for initialization
	void Start () {

		string json = Resources.Load("all_preference").ToString();

		JsonUtility.FromJsonOverwrite(json, si);

		// Debug.Log(si.hit_decision);
		// Debug.Log(si.speed);
		// Debug.Log(si.list[0].id);

		
		//GameObject image_object = GameObject.Find(String.Format("{0}",title));//画像のロード　正直わからん
		//Image image_component = image_object.GetComponent<Image>();

		musicSID = 0;
		sceneDif = 0;

		sortMode = 0;
		path = 0;

		/* insert values in each variable to allow other variables to refer to them */
		/* スタート段階でselectedMusic, sceneDifの値は決まらないからこれはテストとして使う */
		

		//他に必要な処理
	}

	// Update is called once per frame
	void Update () {
		switch (sortMode){
			case 0:
				musicSID = path;
				break;
			case 1:
				musicSID = listPath[path];
				break;//ここバグでる可能性ある。
		}
		selectMusic(musicSID, sceneDif);

		//Debug.Log(string.Format("title:{0}",title));
		//title:ugokuugoku
		//必要な処理
		/*
		マカロン食べること。
		*/
	}
	

	public void selectMusic(int n, int dif){
		//00001
		musicID = si.list[n].id;
		difficulty = si.list[n].difficulty[dif];
		composer = si.list[n].composer;
		title = si.list[n].title;
		speed = si.speed;
		hit_decision = si.hit_decision;
	}
}

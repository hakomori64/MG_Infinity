using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour {
	public Image thumbnail;
	public GameObject easyButton;
	public GameObject mediumButton;
	public GameObject hardButton;
	public GameObject infinityButton;
	public GameObject playButton;
	public GameObject backButton;
	public GameObject optionButton;
	public GameObject sortButton;
	public GameObject centerButton;
	public GameObject oneUpButton;
	public GameObject twoUpButton;
	public GameObject threeUpButton;
	public GameObject oneDownButton;
	public GameObject twoDownButton;

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
	private static int musicID;
	private static int stars;
	private static string composer;
	private static string title;
	private static float speed, hit_decision;
	private static int sceneDif=0; //scene内の難易度を扱う変数　easy:0,medium:1,hard:2,infinity:3 scene内じゃなくて全体になりました。

	//このシーン内で使う変数
	private static string thumbnailName;
	private static int sortMode=0;
	//sortした時のモード選択　sortのモードいくつあったっけ？（それぞれ割り当てるのじゃ）
	private static int path=0;
	private static int[] listPath;
	private static string[] titleList;

	// Use this for initialization
	void Start () {

		string json = Resources.Load("all_preference").ToString();

		JsonUtility.FromJsonOverwrite(json, si);

		selectMusic(path,sceneDif);

		thumbnailName = si.list[path].title;

		/* insert values in each variable to allow other variables to refer to them */
		/* スタート段階でselectedMusic, sceneDifの値は決まらないからこれはテストとして使う */

		loadThumbnail(thumbnailName);

		displayTitle();

		displaySortMode(sortMode);

		//他に必要な処理
	}

	// Update is called once per frame
	void Update () {
		//title:ugokuugoku
		//どうせここ画面遷移するまで曲を流すだけだと思うんだ。
	}
	

	public void selectMusic(int n, int dif){
		//00001
		musicID = si.list[n].id;
		stars = si.list[n].difficulty[dif];
		composer = si.list[n].composer;
		title = si.list[n].title;
		speed = si.speed;
		hit_decision = si.hit_decision;
		Debug.Log("ID"+musicID);
		Debug.Log("stars"+stars);
		Debug.Log("composer"+composer);
		Debug.Log("title"+title);
		Debug.Log("speed"+speed);
		Debug.Log("hitDecision"+hit_decision);
	}

	public void displayTitle(){
		this.centerButton.GetComponentInChildren<Text>().text = title;
		this.threeUpButton.GetComponentInChildren<Text>().text = si.list[(si.list.Length+path-3)%si.list.Length].title;
		this.twoUpButton.GetComponentInChildren<Text>().text = si.list[(si.list.Length+path-2)%si.list.Length].title;
		this.oneUpButton.GetComponentInChildren<Text>().text = si.list[(si.list.Length+path-1)%si.list.Length].title;
		this.oneDownButton.GetComponentInChildren<Text>().text = si.list[(si.list.Length+path+1)%si.list.Length].title;
		this.twoDownButton.GetComponentInChildren<Text>().text = si.list[(si.list.Length+path+2)%si.list.Length].title;
	}

		//他シーンで変数を読み込むとき用関数
	public static int getID(){
		return musicID;
	}
	public static int getDifficulty(){
		return sceneDif;
	}
	public static int getSters(){
		return stars;
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

	public void moveOption(){
		SceneManager.LoadScene("option");
	}

	//一つ上の曲
	public void upMusic(){
		path = (si.list.Length + path - 1)%si.list.Length;
		Debug.Log(path);
		selectMusic(path,sceneDif);
		loadThumbnail(title);
		displayTitle();
	}

	//一つ下の曲
	public void downMusic(){
		path = (si.list.Length+path + 1)%si.list.Length;
		Debug.Log(path);
		selectMusic(path,sceneDif);
		loadThumbnail(title);
		displayTitle();
	}

	//thumbnail読み込み
	public void loadThumbnail(string thumbnailName){
		this.thumbnail.sprite = Resources.Load<Sprite>("images/"+thumbnailName);
	}

	//easyボタン
	public void chengeToEasy(){
		sceneDif = 0;
		selectMusic(path,sceneDif);
		Debug.Log(sceneDif);
	}
	//mediumボタン
	public void chengeToMedium(){
		sceneDif = 1;
		selectMusic(path,sceneDif);
		Debug.Log(sceneDif);
	}
	//hardボタン
	public void chengeToHard(){
		sceneDif = 2;
		selectMusic(path,sceneDif);
		Debug.Log(sceneDif);
	}
	//infinityボタン
	public void changeToInfinity(){
		sceneDif = 3;
		selectMusic(path,sceneDif);
		Debug.Log(sceneDif);
	}
	//NEET系sortボタン~NEATを目指して~
	public void musicNameSort(){
		titleList = new string[si.list.Length];
		listPath = new int[si.list.Length];
		for(int i = 0; i < si.list.Length; i++){
			titleList[i] = si.list[i].title;
		}
		Array.Sort(titleList);
		for(int i = 0; i < si.list.Length; i++){
			listPath[i] = Array.IndexOf(si.list,titleList[i]);
		}
		sortMode = 1;
		//nameに順番にtitle入れてソート
		//nameの文字列をsi.listで検索して、index順番にlistPathにぶち込む（これでlistPathを参照すれば元のindexがわかる...はず）
		//ちなみに、同じ名前があったら死ぬ。
		displaySortMode(sortMode);
		//必要な処理
		/*
		sortする。以上
		*/
	}
	public void displaySortMode(int sortMode){
		string sortText = "";
		switch(sortMode){
			case 0:
				sortText = "ID";
				break;
			case 1:
				sortText = "Title";
				break;
		}
		this.sortButton.GetComponentInChildren<Text>().text = "Sorted by"+sortText;
	}
}

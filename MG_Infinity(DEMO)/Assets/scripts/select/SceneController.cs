using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour {
	public Image thumbnail;
	public AudioSource bgm;
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
		public listSongsInformation[] list;
	}

	[System.Serializable]// <-JSONへの変換に必要
	public class listSongsInformation
	{
		public int id;
		public string composer;
		public string title;
		public int play_count;
		public int[] highscore;
		public int[] difficulty;
	}

	[System.Serializable]
	public class MusicDateList
	{
		public string[] musicTitleList;
		public string[] composerList;
		public int[] play_countList;
	}

	//他シーンに渡す変数
	private static int musicID;
	private static int stars;
	private static string composer;
	private static string title;
	private static int highscore;
	private static int sceneDif=0; //scene内の難易度を扱う変数　easy:0,medium:1,hard:2,infinity:3 scene内じゃなくて全体になりました。

	//このシーン内で使う変数
	private static string thumbnailName;
	private static int sortMode=0;
	//sortした時のモード選択　sortのモードいくつあったっけ？（それぞれ割り当てるのじゃ）
	private static int sortModeSum = 2;
	private static int path=0;
	private int[] listPath;
	private string[] titleList;

	// Use this for initialization
	void Start () {

		string json = Resources.Load("all_preference").ToString();

		JsonUtility.FromJsonOverwrite(json, si);
		listPath = new int[si.list.Length];
		titleList = new string[si.list.Length];

		setListPath(sortMode);

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
		musicID = si.list[listPath[n]].id;
		stars = si.list[listPath[n]].difficulty[dif];
		composer = si.list[listPath[n]].composer;
		title = si.list[listPath[n]].title;
		highscore = si.list[listPath[n]].highscore[dif];

		Debug.Log("ID"+musicID);
		Debug.Log("stars"+stars);
		Debug.Log("composer"+composer);
		Debug.Log("title"+title);
		Debug.Log("highscore"+highscore);
	}

	public void setListPath(int sortMode){
		switch(sortMode){
			case 0:
				musicIDSort();
				break;
			case 1:
				musicNameSort();
				break;
		}
	}

	//titleをボタンの場所に表示する関数
	public void displayTitle(){
		this.centerButton.GetComponentInChildren<Text>().text = title;
		this.threeUpButton.GetComponentInChildren<Text>().text = si.list[listPath[(si.list.Length+path-3)%si.list.Length]].title;
		this.twoUpButton.GetComponentInChildren<Text>().text = si.list[listPath[(si.list.Length+path-2)%si.list.Length]].title;
		this.oneUpButton.GetComponentInChildren<Text>().text = si.list[listPath[(si.list.Length+path-1)%si.list.Length]].title;
		this.oneDownButton.GetComponentInChildren<Text>().text = si.list[listPath[(si.list.Length+path+1)%si.list.Length]].title;
		this.twoDownButton.GetComponentInChildren<Text>().text = si.list[listPath[(si.list.Length+path+2)%si.list.Length]].title;
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

	public static int getHighscore(){
		return highscore;
	}


	//returnボタン
	public void moveStart(){
		SceneManager.LoadScene("start");
	}

	//playボタン
	public void movePlay(){
		SceneManager.LoadScene("play");
	}
	//option画面への画面遷移
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
	//ここから下すべてsortボタン用(まだ動いてない)
	public void sortMusic(){
		sortMode = (sortMode+1)%sortModeSum;
		setListPath(sortMode);
	}
	public void musicIDSort(){
		for(int i = 0; i < si.list.Length; i++){
			listPath[i] = i;
			Debug.Log("ID"+listPath[i]);
		}
		Debug.Log("IDのパス一覧"+listPath[0]);
		displaySortMode(sortMode);
	}
	public void musicNameSort(){
		for(int i = 0; i < si.list.Length; i++){
			titleList[i] = si.list[i].title;
		}
		Debug.Log("titleリスト"+titleList[0]+" "+titleList[1]+" "+titleList[2]+" "+titleList[3]);
		Array.Sort(titleList);
		Debug.Log("sorted"+titleList[0]+" "+titleList[1]+" "+titleList[2]+" "+titleList[3]);//ちゃんと動いた
		/*for(int i = 0; i < si.list.Length; i++){ここで名前検索してidをlistPathに入れる
		}*/
		sortMode = 1;
		//nameに順番にtitle入れてソート
		//nameの文字列をsi.listで検索して、index順番にlistPathにぶち込む（これでlistPathを参照すれば元のindexがわかる...はず）
		//ちなみに、同じ名前があったら死ぬ。←なくても死んだ。←まずIndexOf関数の使い方を間違えてただけだった
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

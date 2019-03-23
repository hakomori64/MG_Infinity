using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionController : MonoBehaviour {
	public GameObject upSpeedButton;
	public GameObject commaUpSpeedButton;
	public GameObject downSpeedButton;
	public GameObject commaDownSpeedButton;
	public GameObject upSizeButton;
	public GameObject downSizeButton;
	public GameObject upThicknessButton;
	public GameObject commaUpThicknessButton;
	public GameObject downThicknessButton;
	public GameObject commaDownThicknessButton;
	public GameObject upMusicVolButton;
	public GameObject downMusicVolButton;
	public GameObject upBGMVolButton;
	public GameObject downBGMVolButton;
	public GameObject upAdjustmentButton;
	public GameObject commaUpAdjustmentButton;
	public GameObject downAdjustmentButton;
	public GameObject commaDownAdjustmentButton;
	public GameObject returnButton;
	public GameObject saveAndMoveSelectButton;

	MusicOption mo;

	private string filePath = "Assets/Resources/MusicOption.json";

	private int maxSpeed = 20*10;
	private int maxSize = 10;
	private int maxThickness = 2*10;
	private int minThickness = (int)(0.5*10);
	private int maxMusicVol = 100;
	private int maxBGMVol = 100;

	private int sp;
	private int th;
	private int ad;

	private string json;


	[Serializable]
	public class MusicOption
	{
		public float speed;
		public int size;
		public float thickness;
		public int musicVol;
		public int BGMVol;
		public float adjustment;
	}

	// Use this for initialization
	void Start () {
		json = Resources.Load("MusicOption").ToString();
		mo = JsonUtility.FromJson<MusicOption>(json);
		Debug.Log("json"+json.ToString());

		Debug.Log("mo"+ mo.ToString());

		Debug.Log("value");
		Debug.Log("speed"+mo.speed);
		Debug.Log("size"+mo.size);
		Debug.Log("thickness"+mo.thickness);
		Debug.Log("musicVol"+mo.musicVol);
		Debug.Log("BGMVol"+mo.BGMVol);
		Debug.Log("adjustment"+mo.adjustment);

		sp = (int)(mo.speed*10);
		th = (int)(mo.thickness*10);
		ad = (int)(mo.adjustment*10);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void upSpeed(){
		if(sp <= maxSpeed-10){
			sp += 10;
		}else{
			sp = maxSpeed;
		}
		Debug.Log("speed "+ sp);
	}

	public void commaUpSpeed(){
		if(sp <= maxSpeed-1){
			sp += 1;
		}else{
			sp = maxSpeed;
		}
		Debug.Log("speed "+ sp);
	}

	public void downSpeed(){
		if(sp >= 20){
			sp -= 10;
		}else{
			sp = 10;
		}
		Debug.Log("speed "+ sp);
	}

	public void commaDownSpeed(){
		if(sp >= 11){
			sp -= 1;
		}else{
			sp = 10;
		}
		Debug.Log("speed "+ sp);
	}

	public void upSize(){
		if(mo.size < maxSize){
			mo.size += 1;
		}else{
			mo.size = maxSize;
		}
		Debug.Log("size "+ mo.size);
	}

	public void downSize(){
		if(mo.size > 1){
			mo.size -= 1;
		}else{
			mo.size = 1;
		}
		Debug.Log("size "+ mo.size);
	}

	public void upThickness(){
		if(th <= maxThickness-10){
			th += 10;
		}else{
			th = maxThickness;
		}
		Debug.Log("thickness "+ th);
	}

	public void commaUpThickness(){
		if(th <= maxThickness-1){
			th += 1;
		}else{
			th = maxThickness;
		}
		Debug.Log("thickness "+ th);
	}

	public void downThickness(){
		if(th >= minThickness+10){
			th -= 10;
		}else{
			th = minThickness;
		}
		Debug.Log("thickness "+ th);
	}

	public void commaDownThickness(){
		if(th >= minThickness+1){
			th -= 1;
		}else{
			th = minThickness;
		}
		Debug.Log("thickness "+ th);
	}

	public void upMusicVol(){
		if(mo.musicVol <= maxMusicVol-1){
			mo.musicVol += 1;
		}else{
			mo.musicVol = maxMusicVol;
		}
		Debug.Log("musicVolume "+ mo.musicVol);
	}

	public void downMusicVol(){
		if(mo.musicVol > 0){
			mo.musicVol -= 1;
		}else{
			mo.musicVol = 0;
		}
		Debug.Log("musicVolume "+ mo.musicVol);
	}

	public void upBGMVol(){
		if(mo.BGMVol <= maxBGMVol-1){
			mo.BGMVol += 1;
		}else{
			mo.BGMVol = maxBGMVol;
		}
		Debug.Log("BGMVolume "+ mo.BGMVol);
	}

	public void downBGMVol(){
		if(mo.BGMVol > 0){
			mo.BGMVol -= 1;
		}else{
			mo.BGMVol = 0;
		}
		Debug.Log("BGMVolume "+ mo.BGMVol);
	}
	
	public void upAdjustment(){
		ad += 10;
		Debug.Log("adjustment "+ ad);
	}

	public void commaUpAdjustment(){
		ad += 1;
		Debug.Log("adjustment "+ ad);
	}

	public void downAdjustment(){
		ad -= 10;
		Debug.Log("adjustment "+ ad);
	}

	public void commaDownAdjustment(){
		ad -= 1;
		Debug.Log("adjustment "+ ad);
	}

	public void returnSelect(){
		SceneManager.LoadScene("select");
	}

	public void saveAndMoveSelect(){
		mo.speed = (float)sp/10;
		mo.thickness = (float)th/10;
		mo.adjustment = (float)ad/10;
		string serialisedItemJson = JsonUtility.ToJson(mo);
		Debug.Log("serialisedItemJson " + serialisedItemJson);
		File.WriteAllText(filePath, json);
		SceneManager.LoadScene("select");
	}
}

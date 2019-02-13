using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	MusicOption mo = new MusicOption();
	private string filePath = "Resources/MusicOption.json";
	private float maxSpeed = 20.0f;
	private int maxSize = 5;

	private float maxThickness = 2;
	private float minThickness = 0.5f;
	private int maxMusicVol = 100;
	private int maxBGMVol = 100;

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
		JsonUtility.FromJsonOverwrite(json,mo);

		Debug.Log(mo.speed);
		Debug.Log(mo.size);
		Debug.Log(mo.thickness);
		Debug.Log(mo.musicVol);
		Debug.Log(mo.BGMVol);
		Debug.Log(mo.adjustment);

		File.WriteAllText(filePath, json);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void upSpeed(){
		if(mo.speed <= maxSpeed-1){
			mo.speed += 1.0f;
		}else{
			mo.speed = maxSpeed;
		}
		File.WriteAllText(filePath, json);
	}

	public void commaUpSpeed(){
		if(mo.speed <= maxSpeed-0.1f){
			mo.speed += 0.1f;
		}else{
			mo.speed = maxSpeed;
		}
		File.WriteAllText(filePath, json);
	}

	public void downSpeed(){
		if(mo.speed >= 2.0){
			mo.speed -= 1.0f;
		}else{
			mo.speed = 1.0f;
		}
		File.WriteAllText(filePath, json);
	}

	public void commaDownSpeed(){
		if(mo.speed >= 1.1){
			mo.speed -= 0.1f;
		}else{
			mo.speed = 1.0f;
		}
		File.WriteAllText(filePath, json);
	}

	public void upSize(){
		if(mo.size < 10){
			mo.size += 1;
		}else{
			mo.size = 10;
		}
		File.WriteAllText(filePath, json);
	}
	public void downSize(){
		if(mo.size > 1){
			mo.size -= 1;
		}else{
			mo.size = 1;
		}
		File.WriteAllText(filePath, json);
	}

	public void upThickness(){
		if(mo.thickness <= maxThickness-1){
			mo.thickness += 1.0f;
		}else{
			mo.thickness = maxThickness;
		}
		File.WriteAllText(filePath, json);
	}

	public void commaUpThickness(){
		if(mo.thickness <= maxThickness-0.1f){
			mo.thickness += 0.1f;
		}else{
			mo.thickness = maxThickness;
		}
		File.WriteAllText(filePath, json);
	}

	public void downThickness(){
		if(mo.thickness >= minThickness+1.0f){
			mo.thickness -= 1.0f;
		}else{
			mo.thickness = minThickness;
		}
		File.WriteAllText(filePath, json);
	}

	public void commaDownThickness(){
		if(mo.thickness >= minThickness+0.1f){
			mo.thickness -= 0.1f;
		}else{
			mo.thickness = minThickness;
		}
		File.WriteAllText(filePath, json);
	}

	public void upMusicVol(){
		if(mo.musicVol >= maxMusicVol-1){
			mo.musicVol += 1;
		}else{
			mo.musicVol = maxMusicVol;
		}
		File.WriteAllText(filePath, json);
	}

	public void downMusicVol(){
		if(mo.musicVol > 0){
			mo.musicVol -= 1;
		}else{
			mo.musicVol = 0;
		}
		File.WriteAllText(filePath, json);
	}

	public void upBGMVol(){
		if(mo.BGMVol >= maxBGMVol-1){
			mo.BGMVol += 1;
		}else{
			mo.BGMVol = maxBGMVol;
		}
		File.WriteAllText(filePath, json);
	}

	public void downBGMVol(){
		if(mo.BGMVol > 0){
			mo.BGMVol -= 1;
		}else{
			mo.BGMVol = 0;
		}
		File.WriteAllText(filePath, json);
	}
	
	public void upAdjustment(){
		mo.adjustment += 1.0f;
		File.WriteAllText(filePath, json);
	}

	public void commaUpAdjustment(){
		mo.adjustment += 0.1f;
		File.WriteAllText(filePath, json);
	}

	public void downAdjustment(){
		mo.adjustment -= 1.0f;
		File.WriteAllText(filePath, json);
	}

	public void commaDownAdjustment(){
		mo.adjustment -= 0.1f;
		File.WriteAllText(filePath, json);
	}
}

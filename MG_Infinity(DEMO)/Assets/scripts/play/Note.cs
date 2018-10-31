using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	private double startTime;
	private double speed;
	private int[] direction = new int[2];
	public int id;
	public static bool isTouched;

	public static double touchedTime;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//isTouched = Input.touchCount > 0;
	}

	public void Create (double startTime, double speed, int[] direction) {
		this.startTime = startTime;
		this.speed = speed;
		this.direction = direction;
	}
}

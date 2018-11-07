using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	private string route;
	private int id;
	private double start, end;
	private float radius, speed;
	private float time = 0;
	private bool head;
	private int kindOfNote;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (head) {

		} else {

		}
		time += Time.deltaTime;
	}

	public void Create (string route, int id, double start, double end, float radius, float speed, int kindOfNote, bool head) {
		this.route = route;
		this.id = id;
		this.start = start;
		this.end = end;
		this.radius = radius;
		this.speed = speed;
		this.kindOfNote = kindOfNote;
		this.head = head;
	}
}

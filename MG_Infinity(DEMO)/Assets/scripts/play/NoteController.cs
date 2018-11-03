using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is responsible for a note (hit-note, swipe-note, long-note). one NoteController corresponds one note.
// this class administrate note score, time, instantiating and destroying.
public class NoteController : MonoBehaviour {

	public int id;
	private string route;
	private double start, end;
	private float radius, speed;

	private float time = 0;
	private GameObject notePrefab;
	void Start () {
		analyzeRoute();
		//notePrefab = (GameObject)Resources.Load("prefabs/note");
	}
	
	// Update is called once per frame
	void Update () {
		
		time += Time.deltaTime;
	}
	public void Create (int id, string route, double start, double end, float radius, float speed) {
		this.id = id;
		this.route = route;
		this.start = start;
		this.end = end;
		this.radius = radius;
		this.speed = speed;
	}

	//
	void analyzeRoute() {

	}
}

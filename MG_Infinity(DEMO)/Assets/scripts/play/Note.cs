using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	private string route;
	private int id;
	private double start, end;
	private float radius, speed;
	private float noteTime = 0;
	private bool head;
	private int kindOfNote;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (head) { // head of note
					switch (this.kindOfNote) {
						case 0: // hit-note
							//add some function which controls hit-note here!!
							controlHitNote();
							break;
						case 1: // long-note
							//add some function which controls head of long-note here!!
							break;
						case 2: // swipe-note
							//add some function which controls head of swipe-note here!!
							break;
						default:
							break;
					}
				} else { // tail of note
					switch (this.kindOfNote) {
						case 1: // long-note
							//add some function which controls head of long-note here!!
							break;
						case 2: // swipe-note
							//add some function which controls head of swipe-note here!!
							break;
						default:
							break;
					}
				}
		noteTime += Time.deltaTime;
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

	void controlHitNote(){

	}
}

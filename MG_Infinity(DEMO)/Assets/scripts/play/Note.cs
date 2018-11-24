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
	private Dictionary<string, int[]> dictionaryForHitNote = new Dictionary<string, int[]>()
	{
		{"0", new int[2]{1, 180}},
		{"1", new int[2]{1, 135}},
		{"2", new int[2]{1, 90}},
		{"3", new int[2]{1, 45}},
		{"4", new int[2]{1, 0}},
		{"5", new int[2]{1, 315}},
		{"6", new int[2]{1, 270}},
		{"7", new int[2]{1, 225}},
		{"8", new int[2]{0, 45}},
		{"9", new int[2]{0, 90}},
		{"A", new int[2]{0, 135}},
		{"B", new int[2]{0, 180}},
		{"C", new int[2]{0, 225}},
		{"D", new int[2]{0, 270}},
		{"E", new int[2]{0, 315}},
		{"F", new int[2]{0, 0}}

	};

	private int[] degForHitNote = new int[2];
	private float rad;
	private float center;
	private float distance = 0;

	// Use this for initialization
	void Start () {
		if (head) { // head of note
			switch (this.kindOfNote) {
				case 0: // hit-note
					this.degForHitNote = this.dictionaryForHitNote[route];
					this.rad = this.degForHitNote[1] * Mathf.Deg2Rad;
					this.center = this.degForHitNote[0] == 1 ? this.radius : -this.radius;
					break;
				case 1: // long-note
							
					break;
				case 2: // swipe-note
				
					break;
				default:
		
					break;
			}
		
		} else { // tail of note
			switch (this.kindOfNote) {
				case 1: // long-note
					
					break;
				case 2: // swipe-note
					
					break;
				default:
					
					break;
			}
		}

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
		this.transform.position = new Vector2(this.center + this.distance * Mathf.Cos(this.rad), this.distance * Mathf.Sin(this.rad));
		this.distance = this.noteTime * speed;
	}
}

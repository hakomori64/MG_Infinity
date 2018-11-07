using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is responsible for a note (hit-note, swipe-note, long-note). one NoteController corresponds one note.
// this class administrate note score, time, instantiating and destroying.
public class NoteController : MonoBehaviour {

	enum KindsOfNote {
		HitNotes,
		LongNotes,
		SwipeNotes
	}

	public int id;
	private string route;
	private double start, end;
	private float radius, speed;
	private KindsOfNote kindOfNote;

	private float time = 0;
	public GameObject notePrefab;
	private GameObject[] notes;
	void Start () {
		detectKindsOfNote();
		
		//notePrefab = (GameObject)Resources.Load("prefabs/note");
	}
	
	// Update is called once per frame
	void Update () {
		if (time == 0) {
			notes[0].SetActive(true);
		}

		if (this.kindOfNote != KindsOfNote.HitNotes) {
			if (time >= this.end - this.start && time < (this.end - this.start) + Time.deltaTime) {
				notes[1].SetActive(true);
			}
		}

		if (time >= this.end - this.start + this.radius / this.speed) {
			this.notes[0].SetActive(false);
			if (this.notes.Length == 2) {
				this.notes[1].SetActive(false);
			}
			this.gameObject.SetActive(false);
		}
		
		time += Time.deltaTime;
	}
	public void Create (int id, string route, double start, double end, float radius, float speed) {
		this.id = id;
		this.route = route;
		this.start = start;
		this.end = end;
		this.radius = radius;
		this.speed = speed;
		Debug.Log((int)KindsOfNote.HitNotes);
		Debug.Log((int)KindsOfNote.LongNotes);
		Debug.Log((int)KindsOfNote.SwipeNotes);
	}

	//
	void detectKindsOfNote() {
		if (Mathf.Abs((float)(this.end - this.start)) < Mathf.Epsilon) { //hitnotes
			this.kindOfNote = KindsOfNote.HitNotes;
			this.notes = new GameObject[1];
			this.notes[0] = Instantiate(notePrefab, transform.position, transform.rotation);
			Note _note = this.notes[0].GetComponent<Note>();
			_note.Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)this.kindOfNote, true);
		} else { //long | swipe note
			this.notes = new GameObject[2];
			this.notes[0] = Instantiate(notePrefab, transform.position, transform.rotation);
			Note _note = this.notes[0].GetComponent<Note>();
			_note.Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)kindOfNote, true);
			this.notes[1] = Instantiate(notePrefab, transform.position, transform.rotation);
			_note = this.notes[1].GetComponent<Note>();
			_note.Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)this.kindOfNote, false);
			
			if (this.route.Length == 1) {
				this.kindOfNote = KindsOfNote.LongNotes;
			} else {
				this.kindOfNote = KindsOfNote.SwipeNotes;
			}
		}
	}
}

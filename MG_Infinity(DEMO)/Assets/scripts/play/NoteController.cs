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
	private int scorePerOneNote;

	private float time = 0;
	private int score = 0;
	public GameObject notePrefab;
	private GameObject[] notes;

	public bool isTouched;
	private bool touchSuccessful = true;
	private double ttl = 0.001;
	private double goodBoundary = 0.08;
	private float goodFactor = 0.3f;
	private double greatBoundary = 0.05;
	private float greatFactor = 0.5f;
	private double perfectBoundary = 0.028;
	private float perfectFactor = 1.0f;
	void Start () {
		notePrefab = (GameObject)Resources.Load("prefabs/Note");
		detectKindsOfNote();

	}

	// Update is called once per frame
	void Update () {
		if (time == 0) {
			notes[0].SetActive(true);
			notes[1].SetActive(false);
		}

		if (this.kindOfNote != KindsOfNote.HitNotes) {
			if (time >= this.end - this.start && time < (this.end - this.start) + Time.deltaTime) {
				notes[1].SetActive(true);
			}
		}

		if (time >= this.end - this.start + this.radius / this.speed + this.goodBoundary) {
			this.notes[0].SetActive(false);
			if (this.notes.Length == 2) {
				this.notes[1].SetActive(false);
			}
			this.gameObject.SetActive(false);
			GameController.scoreValue += this.score;
		}

		isTouchSucceeded();
		time += Time.deltaTime;
	}
	public void Create (int id, string route, double start, double end, float radius, float speed, int scorePerOneNote) {
		this.id = id;
		this.route = route;
		this.start = start;
		this.end = end;
		this.radius = radius;
		this.speed = speed;
		this.scorePerOneNote = scorePerOneNote;
		// Debug.Log((int)KindsOfNote.HitNotes);
		// Debug.Log((int)KindsOfNote.LongNotes);
		// Debug.Log((int)KindsOfNote.SwipeNotes);
	}

	//
	void detectKindsOfNote() {
		// assign KindsOfNote to kindOfNote and instantiate notes

		if (Mathf.Abs((float)(this.end - this.start)) < Mathf.Epsilon) { //hitnotes
			this.kindOfNote = KindsOfNote.HitNotes;
			this.notes = new GameObject[1];
			this.notes[0] = Instantiate(notePrefab, transform.position, transform.rotation);
			Note _note = this.notes[0].GetComponent<Note>();
			_note.Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)this.kindOfNote, true);
		} else { //long | swipe note
			if (this.route.Length == 1) {
				this.kindOfNote = KindsOfNote.LongNotes;
			} else {
				this.kindOfNote = KindsOfNote.SwipeNotes;
			}
			this.notes = new GameObject[2];
			this.notes[0] = Instantiate(notePrefab, transform.position, transform.rotation);
			Note _note = this.notes[0].GetComponent<Note>();
			_note.Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)this.kindOfNote, true);
			this.notes[1] = Instantiate(notePrefab, transform.position, transform.rotation);
			_note = this.notes[1].GetComponent<Note>();
			_note.Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)this.kindOfNote, false);
			notes[1].transform.parent = notes[0].transform;// 紐づけ


		}
		this.notes[0].transform.parent = this.transform;
	}

	void isTouchSucceeded() {
		// タッチの状態から得点を算出し、scoreにセットする
		// GameControllerのscoreCountの値も更新する


		/* 
		switch ((int)(this.kindOfNote)) {
				public class Score	{
					public int missed;
					public int good;
					public int great;
					public int perfect;}
	        case 0:
	            //早くタッチした場合はこれでいける。遅くタッチした場合、これじゃダメ。
	            //なぜなら、(end - start)秒後にノートのSetActiveはfalseになりスクリプトは実行されないから。
	            if (Mathf.Abs(time - radius / speed) > 0.080) {
	                // score.missed++;
	                return;
	            } else if (Mathf.Abs(time - radius / speed) > 0.050) {
	                //score 100000 / (number of note) * 0.4
	                //change color
					//score.good++;
	            } else if (Mathf.Abs(time - radius / speed) > 0.028) {
	                //score 100000 / (number of note) * 0.7
	                //change color
					//score.great++;
	            } else {
	                //score 100000 / (number of note)
	                //change color
					//score.perfect++;
	            }
	            break;
	        case 1:
	            if (this.touchSuccessful == false) return;

	            if (Mathf.Abs(time - radius / speed) > 0.080) {
	                this.touchSuccessful = false;
	            }
	            break;
  		}
		*/
	}
}



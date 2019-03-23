using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

// this class is responsible for a note (hit-note, swipe-note, long-note). one NoteController corresponds one note.
// this class administrate note score, time, instantiating and destroying.
public class NoteController : MonoBehaviour {
	enum KindsOfNote {
		HitNotes,
		LongNotes,
		SwipeNotes
	}

	[System.Serializable]
	public class MusicOption {
		public double speed;
		public int size;
		public double thickness;
		public int musicVol;
		public int BGMVol;
		public double adjustment;
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
	private TouchPointController TouchPointController;

	private MusicOption chart = new MusicOption();
	public GameObject[] notes;
	private bool isTouchDetectionDone = false; 
	public Note[] noteComponents;
	private bool touchSuccessful = true;
	private double ttl = 0.001;
	private double goodBoundary = 0.08;
	private float goodFactor = 0.3f;
	private double greatBoundary = 0.05;
	private float greatFactor = 0.5f;
	private double perfectBoundary = 0.028;
	private float perfectFactor = 1.0f;

	private List<List<TouchPhase>> touchPhaseList = new List<List<TouchPhase>>();
	void Start () {
		notePrefab = (GameObject)Resources.Load("prefabs/Note");
		TouchPointController = GameObject.Find("TouchPointController").GetComponent<TouchPointController>();
		initChart();
		this.speed = (float)chart.speed;
		detectKindsOfNote();
		initTouchPhaseList();
	}

	// Update is called once per frame
	void Update () {
		if (time == 0) {
			notes[0].SetActive(true);
			if (this.kindOfNote != KindsOfNote.HitNotes) notes[1].SetActive(false);
		}

		if (this.kindOfNote != KindsOfNote.HitNotes) {
			if (time >= this.end - this.start && time < (this.end - this.start) + Time.deltaTime) {
				notes[1].SetActive(true);
			}
		}

		if (time >= this.end - this.start + this.radius / this.speed + this.goodBoundary + this.ttl) {
			this.notes[0].SetActive(false);
			if (this.notes.Length == 2) {
				this.notes[1].SetActive(false);
			}
			this.gameObject.SetActive(false);
			GameController.scoreValue += this.score;
		}

		detectAccuracy();
		updateTouchPhaseList();


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
	}

	//
	void detectKindsOfNote() {
		// assign KindsOfNote to kindOfNote and instantiate notes

		if (Mathf.Abs((float)(this.end - this.start)) < Mathf.Epsilon) { //hitnotes
			this.noteComponents = new Note[1];
			this.kindOfNote = KindsOfNote.HitNotes;
			this.notes = new GameObject[1];
			this.notes[0] = Instantiate(notePrefab, transform.position, transform.rotation);
			this.noteComponents[0] = this.notes[0].GetComponent<Note>();
			this.noteComponents[0].Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)this.kindOfNote, true);
		} else { //long | swipe note
			if (this.route.Length == 1) {
				this.kindOfNote = KindsOfNote.LongNotes;
			} else {
				this.kindOfNote = KindsOfNote.SwipeNotes;
			}
			this.notes = new GameObject[2];
			this.noteComponents = new Note[2];
			this.notes[0] = Instantiate(notePrefab, transform.position, transform.rotation);
			this.noteComponents[0] = this.notes[0].GetComponent<Note>();
			this.noteComponents[0].Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)this.kindOfNote, true);
			this.notes[1] = Instantiate(notePrefab, transform.position, transform.rotation);
			this.noteComponents[1] = this.notes[1].GetComponent<Note>();
			this.noteComponents[1].Create(this.route, this.id, this.start, this.end, this.radius, this.speed, (int)this.kindOfNote, false);
			notes[1].transform.parent = notes[0].transform;// 紐づけ


		}
		this.notes[0].transform.parent = this.transform;
	}

	void detectAccuracy() {
		// タッチの状態から得点を算出し、scoreにセットする
		// GameControllerのscoreCountの値も更新する
		if (isTouchDetectionDone) return;


		switch ((int)(this.kindOfNote)) {
	        case 0:

				float timeDifference = this.time - this.radius / speed + (float)chart.adjustment;
				if (timeDifference > this.goodBoundary + this.ttl) {
					GameController.score["Hit"][3]++;
					this.isTouchDetectionDone = true;
				}
				timeDifference = Mathf.Abs(timeDifference);
		    	if ((touchPhaseList[Convert.ToInt32(this.route, 16)][0] == TouchPhase.Ended && touchPhaseList[Convert.ToInt32(this.route, 16)][1] == TouchPhase.Moved)
				  || touchPhaseList[Convert.ToInt32(this.route, 16)][0] == TouchPhase.Began) {
					if (timeDifference < this.goodBoundary + this.ttl) {
						if (timeDifference > this.goodBoundary) { //bad
							GameController.score["Hit"][3]++;
						} else if (timeDifference > this.greatBoundary) { //good
							GameController.score["Hit"][2]++;
						} else if (timeDifference > this.perfectBoundary) { //great
							GameController.score["Hit"][1]++;
						} else {
							GameController.score["Hit"][0]++; //perfect	
						}
						this.isTouchDetectionDone = true;
					} else {
						Debug.Log(id + " 遠すぎて判定外");
					}
					Debug.Log("id: " + id + " " + GameController.score["Hit"][0] + GameController.score["Hit"][1] + GameController.score["Hit"][2] + GameController.score["Hit"][3]);
				}
				break; 
	        case 1:
	            if (this.touchSuccessful == false) return;

	            if (Mathf.Abs(time - radius / speed) > 0.080) {
	                this.touchSuccessful = false;
	            }
	            break;
  		}
	}

	void initChart() {
		string path = "MusicOption";
		string json = Resources.Load(path).ToString();

		chart = JsonMapper.ToObject<MusicOption>(json);
	}

	void initTouchPhaseList() {
		for (int i = 0; i < 15; i++) touchPhaseList.Add(new List<TouchPhase> {TouchPhase.Ended, TouchPhase.Ended});
	}

	void updateTouchPhaseList() {
		for (int i = 0; i < 15; i++) {
			touchPhaseList[i].RemoveAt(0);
			touchPhaseList[i].Add(TouchPointController.touchComponent[i].touchPhase);
		}
	}

	void debugTouchPhaseList() {
		Debug.Log(touchPhaseList[8][0] + " " + touchPhaseList[8][1]);
	}

	
}



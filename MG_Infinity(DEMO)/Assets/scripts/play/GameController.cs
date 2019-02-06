using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;

public class GameController : MonoBehaviour {
	public class ScoreCount {
		public int bad = 0, good = 0, great = 0, perfect = 0;
	}
	public static ScoreCount scoreCount;
	public static float scoreValue;
	private float time = 0f; //used to measure the time.
	private int id, difficulty; //used to load specified musical score.
	private string composer, title; //used to load specified musical score.
	private float speed, hit_decision;
	public int numberOfInstantiatedNotes = 0;
	private Phase phase; //used to express the progress status.
	ChartDataBody chart = new ChartDataBody();
	public float radius; //radius of the two circles.
	public GameObject pauseScene;
	public GameObject pauseButton;
	public GameObject touchToStart;
	public GameObject timerController;
	public GameObject timer;
	public GameObject startScene;
	// public GameObject audioObject;
	public AudioSource audioSource;
	public Text scoreLabel;
	public GameObject noteController;

	private List<GameObject> generatedNoteControllers;

	private int ceilingScore = 100000;
	public static int scorePerOneNote;
	public static int baseScore;
	public static int total; // sum of virtual hit notes
	enum Phase {
		beforeTouchToStart,
		afterTouchToStart,
		playing,
		pausing,
		ending,
	}

	[System.Serializable]
	public class ChartDataBody {
		public string version;             // s.t. "1#1.0.1":"devteamによる譜面の仕様ver#各譜面のversion"
		public int[] difficulty;           //[(0=easy | 1=medium | 2=hard | 3=infinity)(int), 1-10(int)]
		public double[][] notesTime;        //[[starttime, endtime], ...]
		public string[] route;             //[0123456789ABCDEF] \hex, 0==F
		public int bpm;                    //[1-320]
		public int[][] setBpm;
		public float offset; // [](単位s)
	}


	void Start () {
		initStaticVariables();

		// init chart
		initChart();

		// calculate point per one note
		calculatePointOfANote();

		// init audioSource
		initAudioSource();

		// init scoreLabel
		initScoreLabel();

		// init noteControllers
		initNoteControllers();
		pauseScene.SetActive(false);
		pauseButton.SetActive(false);
		timerController.SetActive(false);
		timer.SetActive(false);

		phase = Phase.beforeTouchToStart;
	}

	// Update is called once per frame
	void Update () {
		switch (phase)
		{
			case Phase.beforeTouchToStart:
				if (Application.isEditor) {
					if (Input.GetButtonUp("Fire1")) {
						beforeTouchToStart();
					}
				} else {
					Touch[] touches = Input.touches;

					if (touches != null) {
						beforeTouchToStart();
					}
				}
				break;
			case Phase.afterTouchToStart:
				afterTouchToStart();
				break;
			case Phase.playing:
				playing();
				break;
			case Phase.pausing:
				break;
			case Phase.ending:
				ending();
				break;
		}
	}

	void beforeTouchToStart() {
		touchToStart.SetActive(false);
		timerController.SetActive(true);
		timer.SetActive(true);
		phase = Phase.afterTouchToStart;
	}

	void afterTouchToStart() {

		if (timerController.activeInHierarchy == false) {
			time += Time.deltaTime;
		}

		int processedNotesCount = 0;
		float gap = radius/speed; // time between InstantiatedNotes and Touched time
		int scanningRange;
		if (chart.notesTime.Length - numberOfInstantiatedNotes >= 10) {
			scanningRange = 10;
		} else {
			scanningRange = chart.notesTime.Length - numberOfInstantiatedNotes;
		}

		for (int i = 0; i < scanningRange; i++) {
			if (chart.notesTime[numberOfInstantiatedNotes + i][0] - gap <= time - chart.offset &&
			    chart.notesTime[numberOfInstantiatedNotes + i][1] + Time.deltaTime - gap >= time - chart.offset) {
				generatedNoteControllers[numberOfInstantiatedNotes + i].SetActive(true);
				processedNotesCount++;
			}
		}

		numberOfInstantiatedNotes += processedNotesCount;


		if (time > chart.offset) {
			//time = 0;
			pauseButton.SetActive(true);
			phase = Phase.playing;
			audioSource.Play();
			time = 0;
		}
	}

	void playing() {

		int processedNotesCount = 0;
		float gap = radius/speed; // time between InstantiatedNotes and Touched time
		//Debug.Log("notes speed is:" + speed);
		//Debug.Log("number of notes:" + chart.notesTime.Length);

		int scanningRange;
		if (this.chart.notesTime.Length - this.numberOfInstantiatedNotes >= 10) {
			scanningRange = 10;
		} else {
			scanningRange = chart.notesTime.Length - numberOfInstantiatedNotes;
		}

		for (int i = 0; i < scanningRange; i++) {
			if (chart.notesTime[numberOfInstantiatedNotes + i][0] - gap <= time - chart.offset &&
			    chart.notesTime[numberOfInstantiatedNotes + i][1] + Time.deltaTime - gap >= time - chart.offset) 
			{
				generatedNoteControllers[numberOfInstantiatedNotes + i].SetActive(true);
				processedNotesCount++;
			}
		}

		numberOfInstantiatedNotes += processedNotesCount;

		if (time >= audioSource.clip.length) {
			audioSource.Stop();
			phase = Phase.ending;
		}
		time += Time.deltaTime;
	}

	void ending() {
		Debug.Log(time);
		Debug.Log(audioSource.clip.length);
		SceneManager.LoadScene("result", LoadSceneMode.Single);
	}

	public void onclickPause() {
		phase = Phase.pausing;
		if (audioSource.isPlaying) {
			audioSource.Pause();
		}
		pauseScene.SetActive(true);

	}
	public void onclickSelect() {
		SceneManager.LoadScene("select", LoadSceneMode.Single);
	}

	public void onclickRetry() {
		SceneManager.LoadScene("play", LoadSceneMode.Single);
	}

	public void onclickContinue() {
		phase = Phase.playing;
		audioSource.Play();
		pauseScene.SetActive(false);
	}

	void initChart() {
		if (Application.isEditor) {
			difficulty = 0;
			id = 1;
			composer = "2bnsn";
			title = "ugokuugoku";
			speed = 1;
			hit_decision = 0.1f;
		} else { 

			difficulty = SceneController.getDifficulty();
			id = SceneController.getID();
			composer = SceneController.getComposer();
			title = SceneController.getTitle();
			speed = SceneController.getSpeed();
			hit_decision = SceneController.getDecision();
		}


		string difficultyFolder = "";

		switch (difficulty) {
			case 0:
				difficultyFolder = "easy";
				break;
			case 1:
				difficultyFolder = "medium";
				break;
			case 2:
				difficultyFolder = "hard";
				break;
			case 3:
				difficultyFolder = "infinity";
				break;
		}

		string path = "chart/" + difficultyFolder + "/" + id.ToString("D5") + "_" + composer + "_" + title;
		string json = Resources.Load(path).ToString();

		chart = JsonMapper.ToObject<ChartDataBody>(json);

		Debug.Log("chart in json format:" + json);
		Debug.Log("difficulty: " + chart.difficulty[0] + chart.difficulty[1]);
		Debug.Log("offset to start: " + chart.offset);
		Debug.Log("route: " + chart.route[0]);
		Debug.Log("number of notes: " + chart.notesTime.Length);

	}

	void initAudioSource() {
		//audioSource = audioObject.GetComponent<AudioSource>();
		string path = "music/" + id.ToString("D5") + "_" + composer + "_" + title;
		audioSource.clip = Resources.Load<AudioClip>(path);
	}

	void initScoreLabel() {
		GameController.scoreValue = 0;
		scoreLabel.text = ((int)(GameController.scoreValue)).ToString("D6");
	}

	void initNoteControllers() {
		generatedNoteControllers = new List<GameObject>();

		for (int i = 0; i < chart.notesTime.Length; i++) {
			GameObject _noteController = Instantiate(noteController, transform.position, transform.rotation) as GameObject;
			_noteController.SetActive(false);
			NoteController noteControllerComponent = _noteController.GetComponent<NoteController>();
			noteControllerComponent.Create(i, chart.route[i], chart.notesTime[i][0], chart.notesTime[i][1], this.radius, this.speed, GameController.scorePerOneNote);
			generatedNoteControllers.Add(_noteController);
		}
	}

	void calculatePointOfANote() {
		for (int i = 0; i < chart.route.Length; i++) {
			total += chart.route[i].Length;
		}

		Debug.Log(GameController.total);
		GameController.scorePerOneNote = (int)(this.ceilingScore / GameController.total);
		GameController.baseScore = this.ceilingScore - GameController.scorePerOneNote * GameController.total;
	}

	public static int getScorePerOneNote() {
		return scorePerOneNote;
	}

	public static int getBaseScore() {
		return baseScore;
	}

	public static float getScoreValue() {
		return scoreValue;
	}

	public static ScoreCount GetScoreCount() {
		return scoreCount;
	}

	public static int getTotal() {
		return total;
	}

	void initStaticVariables() {
		GameController.scorePerOneNote = 0;
		GameController.total = 0;
		GameController.scoreCount = new ScoreCount();
		GameController.baseScore = 0;
		GameController.scoreValue = 0;
	}

}

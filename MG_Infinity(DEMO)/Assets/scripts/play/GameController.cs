using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System;

public class GameController : MonoBehaviour {

	//このクラスのインスタンスに判定を数えさせる
	

	//{perfect, great, good, bad}の順に、ノーツの数を数える
	public static Dictionary<string, int[]> score = new Dictionary<string, int[]>() 
	{
			{"Hit", new int[4] {0, 0, 0, 0}},
			{"Long", new int[4] {0, 0, 0, 0}},
			{"Swipe", new int[4] {0, 0, 0, 0}},
	};
	

	//public static Score scoreCount; //これに判定を数えさせる
	public static float scoreValue; //点数を保持する。NoteControllerからこれに得点を加算させる
	private float time = 0f; //スタートが消えてからの秒数が保持される
	private int id, difficulty; //曲のid、難易度が保持される
	private string composer, title; //作曲者、タイトルが保持される
	private float speed, hit_decision; //ノーツの移動スピード、パーフェクトの時間のずれが保持される
	public int numberOfInstantiatedNotes = 0; //生成したノーツの数を数える変数
	private Phase phase; //曲の進行状況を表す F12で定義に移動してみてください
	ChartDataBody chart = new ChartDataBody(); //譜面データ
	public float radius; //円の半径
	public GameObject pauseScene; //ポーズモーダル
	public GameObject pauseButton; //ポーズモーダルを出すためのボタン
	public GameObject touchToStart; //touch to start と表示されるボタン。これを押すとカウントダウンが始まる
	public GameObject timerController; //タイマーを管理する空のゲームオブジェクト。これがカウントダウンを動かす
	public GameObject timer; //時間を表示するゲームオブジェクト

	public AudioSource audioSource; //曲の再生、停止、音量調整などを統括するオブジェクト
	public Text scoreLabel; //点数を表示するラベル
	public GameObject noteController; //noteControllerのプレハブ

	private List<GameObject> generatedNoteControllers; //生成されたnoteControllerのインスタンスたち

	private int ceilingScore = 100000; //天井点
	public static int scorePerOneNote; //すべてのノーツをヒットノーツに換算しなおした後の一つ当たりのノートの点数
	public static int baseScore; // 底上げ点数、リザルト画面で判定をつかって得点に追加する
	public static int total; // 仮想ヒットノーツの合計数
	enum Phase {
		beforeTouchToStart, // touch to startと表示されている段階
		afterTouchToStart, // カウントダウンがされている段階
		playing, // 曲が再生されている段階
		pausing, // ポーズボタンが押されてポーズモーダルが表示されている段階
		ending, // 曲が終わった後の後処理の段階
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
		initStaticVariables(); //staticで宣言されている変数の初期化を行う

		// init chart
		initChart(); //変数chartを設定して、それ経由でデータにアクセスできるようにする

		// calculate point per one note
		calculatePointOfANote(); //仮想ヒットノーツ一つ当たりの得点を計算し、scorePerOneNoteとbaseScoreを設定する

		// init audioSource
		initAudioSource(); //audioSourceに曲を設定

		// init scoreLabel
		initScoreLabel(); //scoreValueにゼロを設定、scoreLabelにそれを文字列にしたものを設定

		// init noteControllers
		initNoteControllers(); //最初にまとめてgeneratedNoteControllersを設定。時間になったらオンしていく
		pauseScene.SetActive(false); //ポーズモーダルをオフにする
		pauseButton.SetActive(false); //ポーズボタンをオフにする
		timerController.SetActive(false); //タイマーをコントロールするオブジェクトをオフにする
		timer.SetActive(false); //タイマーを非表示にする

		phase = Phase.beforeTouchToStart; //フェーズをタッチ前に設定
	}

	// Update is called once per frame
	void Update () {
		switch (phase) //phaseによって処理を振り分け
		{
			case Phase.beforeTouchToStart:

				if (Application.isEditor) { // UnityEditorで起動したとき
					if (Input.GetButtonUp("Fire1")) { //左クリックされたら
						beforeTouchToStart(); //タイマーをオン
					}
				} else {
					Touch[] touches = Input.touches;

					if (touches != null) {
						beforeTouchToStart(); //タイマーをオン
					}
				}
				break;
			case Phase.afterTouchToStart:
				/*
				時を刻む。
				時間が過ぎたら、
				 */
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
			if (chart.notesTime[numberOfInstantiatedNotes + i][0] + 0.001 - gap <= time - chart.offset &&
			    chart.notesTime[numberOfInstantiatedNotes + i][1] + 0.001 + Time.deltaTime - gap >= time - chart.offset) 
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
		scoreValue = 0;
		scoreLabel.text = ((int)(scoreValue)).ToString("D6");
	}

	void initNoteControllers() {
		generatedNoteControllers = new List<GameObject>();

		for (int i = 0; i < chart.notesTime.Length; i++) {
			GameObject _noteController = Instantiate(noteController, transform.position, transform.rotation) as GameObject;
			_noteController.SetActive(false);
			NoteController noteControllerComponent = _noteController.GetComponent<NoteController>();
			noteControllerComponent.Create(i, chart.route[i], chart.notesTime[i][0], chart.notesTime[i][1], this.radius, this.speed, scorePerOneNote);
			generatedNoteControllers.Add(_noteController);
		}
	}

	void calculatePointOfANote() {
		for (int i = 0; i < chart.route.Length; i++) {
			if (Mathf.Abs((float)chart.notesTime[i][1] - (float)chart.notesTime[i][0]) > Mathf.Epsilon && chart.route[i].Length == 1) {
				total += 1;
			} 
			total += chart.route[i].Length;
		}

		Debug.Log(total);
		scorePerOneNote = (int)(this.ceilingScore / total);
		baseScore = this.ceilingScore - scorePerOneNote * total;
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

	
	public static Dictionary<string, int[]> GetScoreCount() {
		return score;
	}
	

	public static int getTotal() {
		return total;
	}

	void initStaticVariables() {
		// staticで宣言されている変数を0で初期化する
		scorePerOneNote = 0;
		total = 0;
		score = new Dictionary<string, int[]>()
		{
			{"Hit", new int[4] {0, 0, 0, 0}},
			{"Long", new int[4] {0, 0, 0, 0}},
			{"Swipe", new int[4] {0, 0, 0, 0}},
		};
		baseScore = 0;
		scoreValue = 0;
	}

}

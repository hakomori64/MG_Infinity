using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{

    private GameObject lineObject;
    private LineRenderer lineRenderer;
    private string route;
    private int id;
    private double start, end;
    private float radius, speed;
    private float noteTime = 0;
    private bool head;
    private int kindOfNote;
    private Dictionary<string, int[]> dictionaryForHitandLongNote = new Dictionary<string, int[]>()
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

    private int[] degForSwipeNote;
    private int[] degForHitandLongNote = new int[2];
    private int degForSwipeNoteFormer;
    private int degForSwipeNoteLatter;
    private int startIndexOfSwipeNote = 0;
    public float rad;
    public float center;
    public float distanceOfHead = 0, distanceOfTail = 0;
    private int routeLength;
    private Note parentNote;

    // Use this for initialization
    void Start()
    {
        if (head)
        { // head of note
            switch (this.kindOfNote)
            {
                case 0: // hit-note
                    this.degForHitandLongNote = this.dictionaryForHitandLongNote[route];
                    this.rad = this.degForHitandLongNote[1] * Mathf.Deg2Rad;
                    this.center = this.degForHitandLongNote[0] == 1 ? this.radius : -this.radius;
                    break;

                case 1: // long-note
                    this.degForHitandLongNote = this.dictionaryForHitandLongNote[route];
                    this.rad = this.degForHitandLongNote[1] * Mathf.Deg2Rad;
                    this.center = this.degForHitandLongNote[0] == 1 ? this.radius : -this.radius;
                    lineObject = Instantiate((GameObject)Resources.Load("prefabs/Line"), transform.position, transform.rotation);
                    lineObject.transform.parent = transform;
                    lineRenderer = lineObject.GetComponent<LineRenderer>();
                    lineRenderer.startWidth = 0.3f;
                    lineRenderer.endWidth = 0.3f;
                    lineRenderer.positionCount = 2;
                    break;

                case 2: // swipe-note
                    this.routeLength = this.route.Length;
                    this.degForSwipeNote = this.dictionaryForHitandLongNote[route[0].ToString()];
                    this.rad = this.degForSwipeNote[1] * Mathf.Deg2Rad;
                    this.center = this.degForSwipeNote[0] == 1 ? this.radius : -this.radius;
                    lineObject = Instantiate((GameObject)Resources.Load("prefabs/Line"), transform.position, transform.rotation);
                    lineObject.transform.parent = transform;
                    lineRenderer = lineObject.GetComponent<LineRenderer>();
                    lineRenderer.startWidth = 0.3f;
                    lineRenderer.endWidth = 0.3f;
                    lineRenderer.positionCount = 2;
                    this.distanceOfTail = radius;
                    break;
                default:
                    break;
            }
        }
        else
        { // tail of note
            switch (this.kindOfNote)
            {
                case 1: // long-note
                    this.degForHitandLongNote = this.dictionaryForHitandLongNote[route];
                    this.rad = this.degForHitandLongNote[1] * Mathf.Deg2Rad;
                    this.center = this.degForHitandLongNote[0] == 1 ? this.radius : -this.radius;
                    break;
                case 2: // swipe-note
                    this.parentNote = this.transform.parent.GetComponent<Note>();
                    this.rad = this.parentNote.rad + Mathf.PI;
                    this.distanceOfTail = radius;
                    break;
                default:

                    break;
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

        //move note
        if (head)
        { // head of note
            switch ((int)this.kindOfNote)
            {

                case 0: // hit-note
                    controlHitNote();
                    break;

                case 1: // long-note
                    controlHeadOfLongNote();
                    break;

                case 2: // swipe-note
                    controlHeadOfSwipeNote();
                    break;
                default:

                    break;
            }
        }
        else
        { // tail of note
            switch ((int)this.kindOfNote)
            {
                case 1: // long-note
                    controlTailOfLongNote();
                    break;
                case 2: // swipe-note
                    controlTailOfSwipeNote();
                    break;
                default:
                    break;
            }
        }


        
        noteTime += Time.deltaTime;
    }

    public void Create(string route, int id, double start, double end, float radius, float speed, int kindOfNote, bool head)
    {
        this.route = route;
        this.id = id;
        this.start = start;
        this.end = end;
        this.radius = radius;
        this.speed = speed;
        this.kindOfNote = kindOfNote;
        this.head = head;
    }

    void controlHitNote()
    {
        this.transform.position = new Vector2(this.center + this.distanceOfHead * Mathf.Cos(this.rad), this.distanceOfHead * Mathf.Sin(this.rad));
        this.distanceOfHead = this.noteTime * speed;
    }
    void controlHeadOfLongNote()
    {
        if (noteTime <= radius / speed)
        {
            controlHitNote();
        }
        else
        {
            this.transform.position = new Vector2(this.center + this.radius * Mathf.Cos(this.rad), this.radius * Mathf.Sin(this.rad));
        }

        if (noteTime >= end - start &&
        noteTime <= radius / speed + (end - start))
        {
            this.distanceOfTail = speed * (noteTime - ((float)end - (float)start));
        }

        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, new Vector2(this.center + this.distanceOfTail * Mathf.Cos(this.rad), this.distanceOfTail * Mathf.Sin(this.rad)));
    }
    void controlTailOfLongNote()
    {
        //Debug.Log("controlTailOfLongNote function is called");
        this.transform.position = new Vector2(this.center + this.distanceOfTail * Mathf.Cos(this.rad), this.distanceOfTail * Mathf.Sin(this.rad));
        this.distanceOfTail = this.noteTime * speed;
    }
    void controlHeadOfSwipeNote()
    {

        if (noteTime < radius / speed)
        {
            controlHitNote();
        }
        else if (noteTime > radius / speed && noteTime < radius / speed + (end - start))
        {


            string routePresent = this.route.Substring(startIndexOfSwipeNote, 2);//->"0123456"->"34"
                                                                                 //Debug.Log(routePresent);
            if (routePresent.IndexOf("0") >= 0 || routePresent.IndexOf("F") >= 0 || routePresent.IndexOf("4") >= 0)
            {
               if (routePresent.IndexOf("8") >= 0 || routePresent.IndexOf("E") >= 0) {
				   this.center = -radius;
			   } else {
				   this.center = radius;
			   }

			   switch (routePresent) {
					case "01":
					case "F1":
						translateNote(180, 135);
						break;
					case "10":
					case "1F":
						translateNote(135, 180);
						break;
					case "07":
					case "F7":
						translateNote(180, 225);
						break;
					case "70":
					case "7F":
						translateNote(225, 180);
						break;
					case "08":
					case "F8":
						translateNote(0, 45);
						break;
					case "80":
					case "8F":
						translateNote(45, 0);
						break;
					case "0E":
					case "FE":
						translateNote(360, 315);
						break;
					case "E0":
					case "EF":
						translateNote(315, 360);
						break;
					case "43":
						translateNote(0, 45);
						break;
					case "34":
						translateNote(45, 0);
						break;
					case "54":
						translateNote(315, 360);
						break;
					case "45":
						translateNote(360, 315);
						break;
			   }
            }
            else
            {
                this.center = this.dictionaryForHitandLongNote[routePresent[0].ToString()][0] == 1 ? radius : -radius;
                //this.degForSwipeNoteFormer = this.dictionaryForHitandLongNote[routePresent[0].ToString()][1]; //e.g. 45
                //this.degForSwipeNoteLatter = this.dictionaryForHitandLongNote[routePresent[1].ToString()][1]; //e.g. 0
				translateNote(this.dictionaryForHitandLongNote[routePresent[0].ToString()][1], this.dictionaryForHitandLongNote[routePresent[1].ToString()][1]);
            }


            //Debug.Log("currentAngle: " + currentAngle);
            //Debug.Log("center: " + center);

            if (noteTime > radius / speed + (end - start) / (routeLength - 1) * (startIndexOfSwipeNote + 1))
            {
                startIndexOfSwipeNote++;
            }

        }

        if (noteTime > end - start && noteTime <= radius / speed + (end - start))
        {
            this.distanceOfTail = (float)(this.radius - speed * (noteTime - (end - start)));
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, new Vector2(this.transform.position.x + this.distanceOfTail * Mathf.Cos(rad + Mathf.PI), this.transform.position.y + this.distanceOfTail * Mathf.Sin(rad + Mathf.PI)));
        } else {
            lineRenderer.SetPosition(0, this.transform.position);
            lineRenderer.SetPosition(1, new Vector2(center, 0));
        }


    }
    void controlTailOfSwipeNote()
    {
        this.rad = this.parentNote.rad + Mathf.PI;
        this.distanceOfTail = radius - noteTime * speed;

        this.transform.position = new Vector2(this.transform.parent.transform.position.x + this.distanceOfTail * Mathf.Cos(this.rad), this.transform.parent.transform.position.y + this.distanceOfTail * Mathf.Sin(this.rad));

    }
    void translateNote(float startDeg, float endDeg)
    {
        double currentAngle;
        double diff = 45 * (routeLength - 1) / (end - start) * (noteTime - (end - start) / (routeLength - 1) * startIndexOfSwipeNote - radius / speed);
        if (startDeg > endDeg)
        {
            currentAngle = startDeg - diff;
        }
        else
        {
            currentAngle = startDeg + diff;
        }
		rad = (float)currentAngle * Mathf.Deg2Rad;
        this.transform.position = new Vector2(this.center + this.radius * Mathf.Cos(this.rad), this.radius * Mathf.Sin(this.rad));

    }
    public bool onTouch() { //if this object is touched by user, this function returns true, else returns false
        if (Application.isEditor) {
            if (Input.GetMouseButtonDown(0)) {
                return true;
            }

            if (Input.GetMouseButton(0)) {
                return true;
            }
        } else {
            if (Input.touchCount > 0) {
                for (int i = 0; i < Input.touchCount; i++) {
                    Touch t = Input.GetTouch(i);

                    if(t.phase == TouchPhase.Began) {
                        Ray ray = Camera.main.ScreenPointToRay(t.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                            if (hit.collider.gameObject == this.gameObject) {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}
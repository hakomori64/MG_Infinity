using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {
  
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
  private int   startIndexOfSwipeNote　= 0;
  private float rad;
  private float center;
  private float distanceOfHead = 0, distanceOfTail = 0;
  private int routeLength;
  
  // Use this for initialization
  void Start () {
    if (head) { // head of note
      switch (this.kindOfNote) {
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
          break;
          
        default:
        
        	break;
      }
      
    } else { // tail of note
      switch (this.kindOfNote) {
        case 1: // long-note
        	this.degForHitandLongNote = this.dictionaryForHitandLongNote[route];
        	this.rad = this.degForHitandLongNote[1] * Mathf.Deg2Rad;
        	this.center = this.degForHitandLongNote[0] == 1 ? this.radius : -this.radius;
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
        	controlHeadOfLongNote();
        	break;
        
        case 2: // swipe-note
        //add some function which controls head of swipe-note here!!
        	controlHeadOfSwipeNote();
        	break;
        default:
        
        	break;
      }
    } else { // tail of note
      switch (this.kindOfNote) {
        case 1: // long-note
        //add some function which controls head of long-note here!!
        	controlTailOfLongNote();
        	break;
        case 2: // swipe-note
        //add some function which controls head of swipe-note here!!
        	controlTailOfSwipeNote();
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
    this.transform.position = new Vector2(this.center + this.distanceOfHead * Mathf.Cos(this.rad), this.distanceOfHead * Mathf.Sin(this.rad));
    this.distanceOfHead = this.noteTime * speed;
  }
  
  void controlHeadOfLongNote(){
    if (noteTime <= radius / speed) {
      controlHitNote();
    } else {
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
  
  void controlTailOfLongNote(){
	//Debug.Log("controlTailOfLongNote function is called");
    this.transform.position = new Vector2(this.center + this.distanceOfTail * Mathf.Cos(this.rad), this.distanceOfTail * Mathf.Sin(this.rad));
    this.distanceOfTail = this.noteTime * speed;
  }
  
  void controlHeadOfSwipeNote(){
	double currentAngle;
    if (noteTime < radius / speed) {
      controlHitNote();
    } else if (noteTime > radius / speed && noteTime < radius / speed + (end - start)) {


        string routePresent = this.route.Substring(startIndexOfSwipeNote, 2);//->"0123456"->"34"
		Debug.Log(routePresent);
		if (routePresent.IndexOf("0") >= 0) {
			int index = (routePresent.IndexOf("0") + 1) % 2;
			this.center = this.dictionaryForHitandLongNote[routePresent[index].ToString()][0] == 1 ? radius : -radius;
			if (index == 1) {
				if (this.dictionaryForHitandLongNote[routePresent[index].ToString()][0] == 1) {
					this.degForSwipeNoteFormer = 180; 
				} else {
					this.degForSwipeNoteFormer = 0;
				}
				this.degForSwipeNoteLatter = this.dictionaryForHitandLongNote[routePresent[1].ToString()][1];
			} else {
				if (this.dictionaryForHitandLongNote[routePresent[index].ToString()][0] == 0) {
					this.degForSwipeNoteLatter = 0; 
				} else {
					this.degForSwipeNoteLatter = 180;
				}
				this.degForSwipeNoteFormer = this.dictionaryForHitandLongNote[routePresent[0].ToString()][1];
			}
					
		} else if (routePresent.IndexOf("F") >= 0){
			int index = (routePresent.IndexOf("F") + 1) % 2;
			this.center = this.dictionaryForHitandLongNote[routePresent[index].ToString()][0] == 1 ? radius : -radius;
			if (index == 1) {
				if (this.dictionaryForHitandLongNote[routePresent[index].ToString()][0] == 1) {
					this.degForSwipeNoteFormer = 180; 
				} else {
					this.degForSwipeNoteFormer = 360;
				}
				this.degForSwipeNoteLatter = this.dictionaryForHitandLongNote[routePresent[1].ToString()][1];
			} else {
				if (this.dictionaryForHitandLongNote[routePresent[index].ToString()][0] == 0) {
					this.degForSwipeNoteLatter = 360; 
				} else {
					this.degForSwipeNoteLatter = 180;
				}
				this.degForSwipeNoteFormer = this.dictionaryForHitandLongNote[routePresent[0].ToString()][1];
			}
					
		} else {
			this.center = this.dictionaryForHitandLongNote[routePresent[0].ToString()][0] == 1 ? radius : -radius;
			this.degForSwipeNoteFormer = this.dictionaryForHitandLongNote[routePresent[0].ToString()][1]; //e.g. 45
    		this.degForSwipeNoteLatter = this.dictionaryForHitandLongNote[routePresent[1].ToString()][1]; //e.g. 0
		}
        
        double diff = 45 * (routeLength - 1) / (end - start) * (noteTime - (end - start) / (routeLength - 1) * startIndexOfSwipeNote - radius / speed);
        if (this.degForSwipeNoteFormer > this.degForSwipeNoteLatter) {
          currentAngle = this.degForSwipeNoteFormer - diff;
        } else {
          currentAngle = this.degForSwipeNoteFormer + diff;
        }
		Debug.Log("currentAngle: " + currentAngle);
		Debug.Log("center: " + center);
        rad = (float)currentAngle * Mathf.Deg2Rad;
        this.transform.position = new Vector2(this.center + this.radius * Mathf.Cos(this.rad), this.radius * Mathf.Sin(this.rad));
        
        if (noteTime > radius / speed + (end - start) / (routeLength - 1) * (startIndexOfSwipeNote + 1)){
          startIndexOfSwipeNote++;
        }
      
    }

    if (noteTime > end - start && noteTime <= radius / speed + (end - start)) {
			this.distanceOfTail = (float)(this.radius - speed * (noteTime - (end - start)));
    }
    
	lineRenderer.SetPosition(0, this.transform.position);
    lineRenderer.SetPosition(1, new Vector2(this.center + this.distanceOfTail * Mathf.Cos(this.rad), this.distanceOfTail * Mathf.Sin(this.rad)));
		
	lineRenderer.SetPosition(0, this.transform.position);
    lineRenderer.SetPosition(1, new Vector2(this.center, 0));
  }
  
  void controlTailOfSwipeNote(){
    // スワイプノーツがどちゃくそながかったとき，distanceOfTailはhogehogeタイムまで定数になっちゃうよ〜〜〜
    // hogehogeタイムを求める
    //  先頭ノーツの時計:end - start　+ this.noteTime
    //  これが基本パターンになるはず，
    /*if (this.noteTime < radius / velocity){
        this.distanceOfTail = radius;
    } else {
        this.distanceOfTail = radius - this.noteTime * speed;
    }*/
    // radius / velocity
    
		//this.transform.localposition = new Vector2(distanceOfTail * cos((rad + pi)), distanceOfTail * sin(rad + pi))
        // 
  }
  
}
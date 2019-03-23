using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoint : MonoBehaviour {

	//use this for initialization
	public int id;
    public TouchPhase touchPhase;
	void Start () {
		touchPhase = TouchPhase.Ended;
	}
	
	// Update is called once per frame
	void Update () {
		if(onTouch()) {
		}	
	}

		public void Create (int id) {
		this.id = id;
	}

	public bool onTouch() { //if this object is touched by user, this function returns true, else returns false
        if (Application.isEditor) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

			if (hit.collider != null && hit.collider.transform == this.transform) {
                if (Input.GetMouseButtonDown(0)) touchPhase = TouchPhase.Began;
                if (Input.GetMouseButton(0)) touchPhase = TouchPhase.Moved;
                if (Input.GetMouseButtonUp(0)) touchPhase = TouchPhase.Ended;
				return true;
			}
        } else {
            if (Input.touchCount > 0) {
                for (int i = 0; i < Input.touchCount; i++) {
                    Touch t = Input.GetTouch(i);

                    Ray ray = Camera.main.ScreenPointToRay(t.position);
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                    if (hit.collider != null && hit.collider.transform == this.transform) {
                        touchPhase = t.phase;
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
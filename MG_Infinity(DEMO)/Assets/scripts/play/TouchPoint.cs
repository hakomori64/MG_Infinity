using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPoint : MonoBehaviour {

	//use this for initialization
	public int id;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(onTouch()) {
			Debug.Log(this.id);
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

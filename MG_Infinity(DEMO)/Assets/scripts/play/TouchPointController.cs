using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPointController : MonoBehaviour {

	// Use this for initialization
	public GameObject TouchPointPrefab;
	public float radius;
	private GameObject[] touchPoints = new GameObject[15];
	public TouchPoint[] touchComponent = new TouchPoint[15];
	void Start () {
		for (int i = 0; i < 8; i++) {
			touchPoints[i] = Instantiate(TouchPointPrefab, new Vector3(radius + radius * Mathf.Cos(45 * i * Mathf.Deg2Rad), radius * Mathf.Sin(45 * i * Mathf.Deg2Rad), 0), Quaternion.identity);
			touchComponent[i] = touchPoints[i].GetComponent<TouchPoint>();
			touchComponent[i].Create(i - 4 > 0 ? 12 - i : 4 - i);
			touchPoints[i].transform.parent = this.transform;
		}

		for (int i = 1; i < 8; i++) {
			touchPoints[i + 7] = Instantiate(TouchPointPrefab, new Vector3(-radius + radius * Mathf.Cos(45 * i * Mathf.Deg2Rad), radius * Mathf.Sin(45 * i * Mathf.Deg2Rad), 0), Quaternion.identity);
			touchComponent[i + 7] = touchPoints[i + 7].GetComponent<TouchPoint>();
			touchComponent[i + 7].Create(i + 7);
			touchPoints[i + 7].transform.parent = this.transform;
		}

	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
			for (int i = 0; i < touchComponent.Length; i++) {
				Debug.Log("i: " + i + ": " + touchComponent[i].touchPhase);
			}
		}
		*/
	}


}

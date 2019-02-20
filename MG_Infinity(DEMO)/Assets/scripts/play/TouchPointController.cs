using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPointController : MonoBehaviour {

	// Use this for initialization
	public GameObject TouchPointPrefab;
	public float radius;
	private GameObject[] touchPoints = new GameObject[15];
	void Start () {
		for (int i = 0; i < 8; i++) {
			touchPoints[i] = Instantiate(TouchPointPrefab, new Vector3(radius + radius * Mathf.Cos(45 * i * Mathf.Deg2Rad), radius * Mathf.Sin(45 * i * Mathf.Deg2Rad), 0), Quaternion.identity);
			touchPoints[i].GetComponent<TouchPoint>().Create(i - 4 > 0 ? 12 - i : 4 - i);
			touchPoints[i].transform.parent = this.transform;
		}

		for (int i = 1; i < 8; i++) {
			touchPoints[i + 7] = Instantiate(TouchPointPrefab, new Vector3(-radius + radius * Mathf.Cos(45 * i * Mathf.Deg2Rad), radius * Mathf.Sin(45 * i * Mathf.Deg2Rad), 0), Quaternion.identity);
			touchPoints[i + 7].GetComponent<TouchPoint>().Create(i + 7);
			touchPoints[i + 7].transform.parent = this.transform;
		}

		for (int i = 0; i < touchPoints.Length; i++) {
			Debug.Log("touchpoint-id:" + touchPoints[i].GetComponent<TouchPoint>().id);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}

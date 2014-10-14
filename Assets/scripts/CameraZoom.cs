using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.orthographicSize *= (Input.GetAxis ("Mouse ScrollWheel")*0.5f)+1;
	}
}

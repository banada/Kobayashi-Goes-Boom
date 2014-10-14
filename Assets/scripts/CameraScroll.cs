using UnityEngine;
using System.Collections;

public class CameraScroll : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine){
			Camera.main.transform.position = transform.position;
			Camera.main.transform.Translate(Vector3.back);
		}
	}
}

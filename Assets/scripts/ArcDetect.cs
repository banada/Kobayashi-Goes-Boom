using UnityEngine;
using System.Collections;

public class ArcDetect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnTriggerEnter2D (Collider2D col){
		if (col.gameObject.tag == "ShipHitBox"){
			transform.parent.GetComponent<Gunfire>().enable = true;
		}
	}

	void OnTriggerExit2D (Collider2D col){
		if (col.gameObject.tag == "ShipHitBox"){
			transform.parent.GetComponent<Gunfire>().enable = false;
		}
	}
}

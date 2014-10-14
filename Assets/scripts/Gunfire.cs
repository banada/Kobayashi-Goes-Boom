using UnityEngine;
using System.Collections;
//public GameObject Laser;

public class Gunfire : MonoBehaviour {
	
	public GameObject Lazer;
	public float timer;
	public float fireRate = 0.1f;
	public bool enable;
	public int fireCone = 15;
	public float speed = 10;
	public float speedVariance = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine && enable) {
			timer += Time.deltaTime;
			if(timer > fireRate && !transform.parent.GetComponent<RoomStatus>().IsDestroyed()){
				//fire cone
				Quaternion bulletRotation = transform.rotation;
				Vector3 eulerRotation = bulletRotation.eulerAngles;
				eulerRotation.z += (float)(Random.value-0.5)*2*fireCone;
				bulletRotation = Quaternion.Euler(eulerRotation);

				//instantiate
				GameObject lazerClone = (GameObject)Network.Instantiate(Lazer, transform.position, bulletRotation, 0);

				//set speed
				float lazerSpeed = (float)(Random.value-1)*speedVariance + speed;
				lazerClone.SendMessage("SetSpeed", lazerSpeed);

				timer = 0;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CrewMemberMovement))]

public class CrewMemberFixRoom : MonoBehaviour {
	
	[SerializeField] int fix_amount = 1;
	[SerializeField] float fix_rate = 2.0f;
	public GameObject ship;
	public GameObject[] rooms;
	public GameObject current_room;
	public RoomStatus current_room_status;
	public RoomBehavior current_room_behavior;
	public GameObject[] members;
	public GameObject current_member;
	private float timer = 0.0f;

	void Start ()
	{
		ship = transform.root.gameObject;
	}

	void Update ()
	{
		// Crew member stays in a room
		GameObject member = gameObject;
		CrewMemberMovement member_mov = member.GetComponent<CrewMemberMovement>();

		// Interface with new room
		rooms = GameObject.FindGameObjectsWithTag("Room");
		foreach (GameObject room in rooms) {
			RoomBehavior room_behavior = room.GetComponent<RoomBehavior>();
			RoomStatus room_status = room.GetComponent<RoomStatus>();
			//Debug.Log("Current room exposed: " + CrewMemberMovement.current_room_exposed);
			if (room_status.room_id == member_mov.current_room_exposed) {
				current_room_status = room_status;
				//current_room_behavior = room_behavior;
				//Debug.Log ("CHANGED ROOM");
			}
		}
		timer += Time.deltaTime;
		if (timer > fix_rate) {
			current_room_status.ChangeDamage(fix_amount);
			timer = 0;
		}
	}
}

using UnityEngine;
using System.Collections;

public class RoomBehavior : MonoBehaviour {

	[SerializeField] public int room_id = 1;

	public GameObject target_room;
	public GameObject ship;
	public GameObject[] members;
	public GameObject current_member;

	// Use this for initialization
	void Start ()
	{
		// Rooms are children of ship
		ship = transform.root.gameObject;
		transform.parent = ship.transform;
	}

	void OnMouseDown()
	{
		// Move selected crew member
		if (networkView.isMine) {
			members = GameObject.FindGameObjectsWithTag ("Crew");
			foreach (GameObject member in members) {
				if (member.transform.IsChildOf (ship.transform)) {
					CrewMemberMovement member_mov = member.GetComponent<CrewMemberMovement> ();
					if (member_mov.selected == true) {
						current_member = member;
						member_mov.move_instruction = room_id;
					}
				}
			}
		}
	}
}
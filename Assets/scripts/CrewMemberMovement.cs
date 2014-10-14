using UnityEngine;
using System.Collections;

public class CrewMemberMovement : MonoBehaviour {

	[SerializeField] int crew_id = 1; 
	public int crew_spot = 0;
	//[SerializeField] static public int selected = 0;
	[SerializeField] public int current_room;
	// expose current room
	public int current_room_exposed;
	public GameObject ship;
	public GameObject current_room_obj;
	public GameObject target_room_obj;
	public GameObject[] members;
	public int target_room = 0;
	public int move_instruction = 0;
	[SerializeField] int blink_time = 3;
	bool has_blinked = false;
	public GameObject[] rooms;
	public RoomBehavior current_room_cs;
	int[][] paths;
	public int total_rooms;
	public bool selected = false;
	public float crew_spacing = 0.4f;

	// CrewMember told to move
	private IEnumerator Moving(int crew_id) {
		//Debug.Log ("Crew member " + crew_id + " moving!");
		// Flash CrewMember icon until arrival
		for (int i=0; i<blink_time; i++) {
			renderer.enabled = true;
			yield return new WaitForSeconds (0.5f);
			renderer.enabled = false;
			yield return new WaitForSeconds (0.5f);
			renderer.enabled = true;
		}
		has_blinked = true;
	}

	//Call Moving() from another script
	public void CallMoving(int crew_id) {
		StartCoroutine(Moving(crew_id));
	}

	// Use this for initialization
	void Start () {
		renderer.material.SetColor("_Color", Color.black);
		current_room_exposed = current_room;

		ship = transform.root.gameObject;

		// Iterate over rooms to place crew member in initial room
		if (current_room != 0)
		{
			rooms = GameObject.FindGameObjectsWithTag("Room");
			// Count total rooms
			foreach (GameObject room in rooms)
			{
				if (room.transform.IsChildOf (ship.transform))
				{
					total_rooms++;
					RoomBehavior room_obj = room.GetComponent<RoomBehavior>();
					RoomStatus room_status = room.GetComponent<RoomStatus>();
					//Debug.Log (room_obj.room_id);
					if (room_obj.room_id == current_room)
					{
						for (int i=0; i<5; i++) {
							//Debug.Log("AAAA: " + room_status.Getter(i));
							if (room_status.Getter(i) == 0) 
							{
								room_status.Setter (i, 1);
								crew_spot = i;
								break;
							}
						}
						Debug.Log ("Crew member in room: " + room_obj.room_id);
						current_room_obj = room;
						current_room_cs = room_obj;
						transform.position = room_obj.transform.position;
					}
				}
			}
		}
		else
		{
			Debug.Log("SET THE CURRENT ROOM");
		}
	}

	void OnMouseDown() {
		// Left Click on CrewMember
		Debug.Log("Selected crew member " + crew_id);
		members = GameObject.FindGameObjectsWithTag("Crew");
		// Room that you are leaving
		foreach (GameObject member in members)
		{
			if (member.transform.IsChildOf (ship.transform))
			{
				CrewMemberMovement crew_mem_mov = member.GetComponent<CrewMemberMovement>();
				crew_mem_mov.renderer.material.SetColor("_Color", Color.black);
				crew_mem_mov.selected = false;
			}
		}
		renderer.material.SetColor ("_Color", Color.white);
		selected = true;
	}

	// Update is called once per frame
	void Update () {
		//if (selected != crew_id) {
		//	renderer.material.SetColor("_Color", Color.yellow);
		//}

		// If current room is destroyed, crew member dies
		rooms = GameObject.FindGameObjectsWithTag("Room");
		foreach (GameObject room in rooms)
		{
			if (room.transform.IsChildOf (ship.transform))
			{
				RoomStatus room_status = room.GetComponent<RoomStatus>();
				if ((room_status.room_id == current_room) && (room_status.IsDestroyed() == true))
				{
					Destroy(gameObject);
				}
			}
		}

		// Crew member told to move
		if ((move_instruction != 0) && (move_instruction != current_room)) {
			Debug.Log("Moving crew member to room " + target_room);
			Debug.Log("Current room: " + current_room);
			CallMoving(crew_id);
			target_room = move_instruction;
			move_instruction = 0;
		}
		// Finished transit blinking
		if (has_blinked == true) {
			has_blinked = false;
			rooms = GameObject.FindGameObjectsWithTag("Room");
			// Room that you are leaving
			foreach (GameObject room in rooms)
			{
				if (room.transform.IsChildOf (ship.transform))
				{
					RoomBehavior room_obj = room.GetComponent<RoomBehavior>();
					RoomStatus room_status = room.GetComponent<RoomStatus>();
					// deduct crew member from starting room
					if (room_obj.room_id == current_room)
					{
						for (int i=0; i<5; i++)
						{
							if (i == crew_spot)
							{
								room_status.Setter(i, 0);
								//Debug.Log("REDUCED CREW");
								break;
							}
						}
					}
				}
			}
			// Room that you are entering
			foreach (GameObject room in rooms)
			{
				if (room.transform.IsChildOf (ship.transform))
				{
					RoomBehavior room_obj = room.GetComponent<RoomBehavior>();
					RoomStatus room_status = room.GetComponent<RoomStatus>();
					// move up one room
					if ((target_room > current_room) && (room_obj.room_id == (current_room + 1)) && (room_status.IsDestroyed() == false)) {
						Debug.Log ("Moving to room " + room_obj.room_id);
						transform.position = room_obj.transform.position;
						current_room = room_obj.room_id;
						current_room_exposed = current_room;
						// Multiple crew in a room
						for (int i=0; i<5; i++) {
							//Debug.Log("SEAT STATUS: " + room_status.Getter(i));
							if (room_status.Getter(i) == 0) {
								room_status.Setter(i, 1);
								crew_spot = i;
								//Debug.Log("CREW SPOT: " + crew_spot);
								break;
							}
						}
						Debug.Log (crew_spacing);
						if (crew_spot == 0) {
							//stay middle
						} else if (crew_spot == 1) {
							transform.Translate(-crew_spacing, 0, crew_spacing,Space.Self);
						} else if (crew_spot == 2) {
							transform.Translate(-crew_spacing, 0, -crew_spacing,Space.Self);
						} else if (crew_spot == 3) {
							transform.Translate(crew_spacing, 0, -crew_spacing,Space.Self);
						} else if (crew_spot == 4) {
							transform.Translate(crew_spacing, 0, crew_spacing,Space.Self);
						}
						// final destination
						if (current_room == target_room) {
							target_room = 0;
							renderer.material.SetColor("_Color", Color.black);
						// keep going
						} else {
							move_instruction = target_room;
						}
						break;
					// move back one room
					} else if ((target_room < current_room) && (room_obj.room_id == (current_room - 1)) && (room_status.IsDestroyed() == false)) {
						Debug.Log ("Moving to room " + room_obj.room_id);
						transform.position = room_obj.transform.position;
						current_room = room_obj.room_id;
						current_room_exposed = current_room;
						// Multiple crew in a room
						for (int i=0; i<5; i++) {
							if (room_status.Getter (i) == 0) {
								room_status.Setter (i, 1);
								crew_spot = i;
								Debug.Log("CREW SPOT: " + crew_spot);
								break;
							}
						}
						Debug.Log (crew_spacing);
						if (crew_spot == 0) {
							//stay middle
						} else if (crew_spot == 1) {
							transform.Translate(-crew_spacing, 0, crew_spacing,Space.Self);
						} else if (crew_spot == 2) {
							transform.Translate(-crew_spacing, 0, -crew_spacing,Space.Self);
						} else if (crew_spot == 3) {
							transform.Translate(crew_spacing, 0, -crew_spacing,Space.Self);
						} else if (crew_spot == 4) {
							transform.Translate(crew_spacing, 0, crew_spacing,Space.Self);
						}
						// final destination
						if (current_room == target_room) {
							target_room = 0;
							renderer.material.SetColor("_Color", Color.black);
						// keep going
						} else {
							move_instruction = target_room;
						}
						break;
					} else if (target_room == current_room) {
						target_room = 0;
						renderer.material.SetColor("_Color", Color.black);
					} else if (room_status.IsDestroyed() == true) {
						renderer.material.SetColor("_Color", Color.black);
					}
				}
			}
		}
	}
}

	
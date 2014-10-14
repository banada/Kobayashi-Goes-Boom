using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MouseMovement))]

public class ShipController : MonoBehaviour
{
	MouseMovement controller;
	Dictionary<int, Room> room_dict;
	// Which crew member is selected
	public int selected = 0;

	void HideFamily ()
	{
		Renderer[] family = GetComponentsInChildren<Renderer> ();
		
		for (int i = 0; i < family.Length; i++)
		{
			family[i].enabled = false;
		}
		
		renderer.enabled = false;
	}

	// Use this for initialization
	void Start () 
	{
		controller = transform.GetComponent<MouseMovement> ();
		room_dict = new Dictionary<int, Room> ();
	}

	public void ConnectRoom (int id, RoomType type, Damage.DamageState status)
	{
		Room this_room = new Room (type, status);
		room_dict.Add (id, this_room);

		ProcessConnection (type);
	}

	/**
	 * 
	 */
	public void UpdateStatus (int id, Damage.DamageState state)
	{
		// Checks to make sure room is registered
		if (room_dict.ContainsKey(id))
		{
			// Retrieves room and processes changes
			Room this_room = room_dict[id];
			ProcessChange (this_room, state);

			// Updates dictionary
			this_room.status = state;
			room_dict[id] = this_room;
		}
		else
		{
			Debug.LogError ("Attempt to modify room which is not registered with the ship.");
		}
	}

	/**
	 * Updates the current ship status relative to which room has changed
	 */
	private void ProcessChange (Room room, Damage.DamageState newState)
	{
		int change = (int)newState - (int)room.status;

		switch (room.type)
		{
			case RoomType.BRIDGE:
				if (newState == Damage.DamageState.DESTROYED)
				{
					transform.root.GetComponent<MouseMovement>().is_ship_destroyed = true;
					HideFamily();
				}
				break;
			case RoomType.ENGINE:
				controller.ChangEngineStatus (change);
				break;
			case RoomType.WEAPONS:
				break;
			default:
				break;
		}
	}

	/**
	 * Modifies the ship status to reflect the room being attached to it
	 */
	public void ProcessConnection (RoomType type)
	{
		//Processes changes from new room
		switch (type)
		{
		case RoomType.BRIDGE:
			break;
		case RoomType.ENGINE:
			controller.RegisterEngine();
			break;
		case RoomType.WEAPONS:
			break;
		default:
			break;
		}
	}

	/**
	 * Represents a room on the ship.
	 */
	private struct Room
	{
		public RoomType type;
		public Damage.DamageState status;

		public Room (RoomType t, Damage.DamageState s)
		{
			type = t;
			status = s;
		}
	}
}
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RoomBehavior))]

public class RoomStatus : MonoBehaviour
{
	private ShipController ship;
	private Damage room_damage;
	public RoomType type;
	public int room_id;
	public int damage_threshold = 100;
	int[] crew_present = new int[] {0,0,0,0,0};
	public GameObject overlay;
	
	// #NEVERFORGET2014
	public int Getter(int index)
	{
		return crew_present [index];
	}

	public void Setter(int index, int input)
	{
		crew_present [index] = input;
	}

	// Use this for initialization
	void Start ()
	{
		// Sets the room's health
		room_damage = new Damage (damage_threshold);
		ship = transform.root.GetComponent<ShipController> ();

		// Finds id
		room_id = transform.GetComponent<RoomBehavior> ().room_id;

		// Registers room with ship
		ship.ConnectRoom (room_id, type, room_damage.GetCurrentState ());
	}

	public bool IsDestroyed ()
	{
		return (room_damage.GetCurrentState () == Damage.DamageState.DESTROYED);
	}

	public void ChangeDamage (int change)
	{
		// Calculates status before and after damage
		Damage.DamageState pre_state = room_damage.GetCurrentState ();
		Damage.DamageState state = room_damage.ChangeDamage (change);

		// Updates status if room status has changed
		if (pre_state != state)
		{
			// Updates overall ship
			ship.UpdateStatus (room_id, state);

			if(state == Damage.DamageState.DESTROYED){
				Debug.Log ("DESTROYED");
				GetComponent<BoxCollider2D>().isTrigger = false;
			}

			// Updates room display
			Color overlay_color = overlay.renderer.material.color;
			overlay_color.a = (int)state / (float)Damage.DamageState.PERFECT;
			overlay.renderer.material.color = overlay_color;
		}
	}
}
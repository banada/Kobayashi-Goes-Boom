using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RoomStatus))]
[RequireComponent(typeof(BoxCollider2D))]

public class RoomCollision : MonoBehaviour
{
	private RoomStatus room_status;

	void Start ()
	{
		room_status = transform.gameObject.GetComponent<RoomStatus> ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Projectile")
		{
			int damage = -(col.gameObject.GetComponent<WeaponSpecs>().GetDamage ());

			room_status.ChangeDamage(damage);
		}
	}
}
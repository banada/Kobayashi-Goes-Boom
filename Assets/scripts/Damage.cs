using UnityEngine;
using System.Collections;

public class Damage
{
	private const int DAMAGE_STATES = 4;
	private int max_health;
	private int current_health;

	public Damage (int health)
	{
		max_health = health;
		current_health = health;
	}

	public DamageState ChangeDamage (int change)
	{
		// Calculates damage
		current_health += change;

		// Bounds damage
		if (current_health > max_health)
		{
			current_health = max_health;
		}
		else if (current_health < 0)
		{
			current_health = 0;
		}

		//Debug.Log (current_health);

		// Returns state
		return GetCurrentState ();
	}

	public float GetHealthPercent ()
	{
		return current_health / (float)max_health;
	}

	public DamageState GetCurrentState ()
	{
		if (GetHealthPercent () <= (int)DamageState.DESTROYED / (float)DAMAGE_STATES)
		{
			return DamageState.DESTROYED;
		}
		if (GetHealthPercent () <= (int)DamageState.BAD / (float)DAMAGE_STATES)
		{
			return DamageState.BAD;
		}
		if (GetHealthPercent () <= (int)DamageState.OKAY / (float)DAMAGE_STATES)
		{
			return DamageState.OKAY;
		}
		if (GetHealthPercent () <= (int)DamageState.FINE / (float)DAMAGE_STATES)
		{
			return DamageState.FINE;
		}

		return DamageState.PERFECT;
	}

	public enum DamageState
	{
		PERFECT   = 4,
		FINE      = 3,
		OKAY      = 2,
		BAD       = 1,
		DESTROYED = 0
	}
}
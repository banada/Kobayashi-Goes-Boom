using UnityEngine;
using System.Collections;

public class MathAngle
{
	/**
	 * Calculates the angle to a specified point, from the current position of the instance.
	 */
	public static float FindAngleToPoint (Quaternion angle, Vector3 pos, Vector3 target)
	{
		float x_dist = pos.x - target.x;
		float y_dist = pos.y - target.y;
		return RoundAngle ((Mathf.Atan2 (x_dist, y_dist)) * Mathf.Rad2Deg + angle.eulerAngles.z);
	}

	/**
	 * Rounds an angle, such that -360 < angle < 360
	 */
	public static float RoundAngle (float angle)
	{
		if (angle > 360)
		{
			return angle - 360;
		}
		if (angle < -360)
		{
			return angle + 360;
		}
		else
		{
			return angle;
		}
	}
}

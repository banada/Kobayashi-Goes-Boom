using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour
{
	private LineRenderer lr;
	private float accel;
	private Vector3 target_pos;
	private float tolerance = 0.01f;
	private float angle_to_turn = 0;
	private float current_speed = 0;
	private float thrust_percent = 0;
	public int total_engine_status = 0;
	public int current_engine_status = 0;
	public float turn_speed;
	public float max_speed;
	public float accel_percent = 0.10f;
	public GUISkin skin;
	public GameObject[] ships;
	public bool is_ship_destroyed = false;
	public GameObject NetworkManager;

	
	// Use this for initialization
	void Start ()
	{
		NetworkManager = Camera.main.gameObject;
		target_pos = transform.position;
		accel = accel_percent;
		
		// Creates line renderer
		lr = (LineRenderer)gameObject.AddComponent ("LineRenderer");
		lr.material = new Material (Shader.Find ("Particles/Additive"));
		lr.SetColors (Color.blue, Color.blue);
		lr.SetWidth (0.03f, 0.03f);
		lr.SetVertexCount (1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (networkView.isMine)
		{
			if (Input.GetMouseButtonDown (1)) 
			{
				// Check for GUIElement under click
				if (GUIUtility.hotControl == 0)
				{
					target_pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					angle_to_turn = MathAngle.FindAngleToPoint (transform.rotation, transform.position, target_pos);
				}
			}
			
			if (NotFacingTarget ())
			{
				Rotate ();
				
				if (NotFacingTarget ())
				{
					DrawUiLine ();
				} else
				{
					ClearUiLine ();
				}
			}
			
			Accelerate ();
			
			transform.Translate (Vector3.down * current_speed * GetEngineStatus () * Time.deltaTime);
		}
	}
	
	/**
	 * Determines if this object is facing the target
	 */
	bool NotFacingTarget ()
	{
		return Mathf.Abs (angle_to_turn) > turn_speed * Time.deltaTime * (1 + tolerance);
	}
	
	/**
	 * Accelerates speed of ship to current speed
	 */
	void Accelerate ()
	{
		float target_speed = max_speed * thrust_percent;
		
		if (current_speed < target_speed)
		{
			current_speed = Mathf.Min (target_speed, current_speed + accel * Time.deltaTime);
		}
		else if (current_speed > target_speed)
		{
			current_speed = Mathf.Max (target_speed, current_speed - accel * Time.deltaTime);
		}
	}
	
	/**
	 * Updates the angle of the ship torwards the specified angle
	 */
	void Rotate ()
	{
		// Modifies path to account for movement
		angle_to_turn = MathAngle.FindAngleToPoint (transform.rotation, transform.position, target_pos);
		
		// Calculates how much to change the current trajectory
		float change_in_angle;
		
		if (angle_to_turn < -180 || (angle_to_turn < 180 && angle_to_turn > 0))
		{
			change_in_angle = -turn_speed * Time.deltaTime * GetEngineStatus ();
		}
		else
		{
			change_in_angle = turn_speed * Time.deltaTime * GetEngineStatus ();
		}
		
		//Updates trajectory with new values
		transform.RotateAround (transform.position, Vector3.forward, change_in_angle);
		
		angle_to_turn = MathAngle.RoundAngle (angle_to_turn + change_in_angle);
	}

	// Draws the ship control GUI
	void OnGUI ()
	{
		if (networkView.isMine)
		{
			GUI.skin = skin;
			
			GUI.BeginGroup (new Rect (10, 10, 500, 400));
			
			// GUI Box
			GUI.Box (new Rect (0, 0, 150, 100), "Engine Feedback");
			
			// Engine status
			float current_percent = current_speed / max_speed;
			
			GUI.Label (new Rect (0, 25, 150, 21), "Power to Engine");
			
			thrust_percent = GUI.HorizontalSlider (new Rect (25, 48, 100, 15), thrust_percent, 0, 1.0f);
			GUI.enabled = false;
			GUI.HorizontalSlider (new Rect (25, 48, 100, 15), current_percent, 0, 1.0f);
			GUI.enabled = true;
			
			// Engine efficiency
			float engine_status = GetEngineStatus ();
			
			GUI.Label (new Rect (0, 63, 150, 21), "Engine Status");
			
			GUI.enabled = false;
			GUI.HorizontalSlider (new Rect (25, 83, 100, 15), engine_status, 0, 1.0f);
			GUI.enabled = true;
			
			ships = GameObject.FindGameObjectsWithTag("Ship");
			// Check if my ship is destroyed
			
			if (this.is_ship_destroyed == true) {
				// Display reset button with loss condition
				if (GUI.Button (new Rect(0, 100, 400, 200), "You Lost!")) {
					// Reset game
					Network.Destroy(gameObject);
					NetworkManager.GetComponent<NetworkManager>().SpawnPlayer();
				}
			} 
			// Check if the enemy ship is destroyed
			else {
				foreach (GameObject ship in ships) {
					MouseMovement mouse = ship.GetComponent<MouseMovement>();
					if ((!ship.networkView.isMine) && (mouse.is_ship_destroyed == true)) {
						// Display reset button with victory condition
						if (GUI.Button (new Rect(0, 100, 400, 200), "You Won!")) {
							// Reset game
							Network.Destroy(gameObject);
							NetworkManager.GetComponent<NetworkManager>().SpawnPlayer();
						}
					}
				}
			}
			
			
			GUI.EndGroup ();
		}
	}
	
	float GetEngineStatus ()
	{
		if (total_engine_status == 0)
		{
			return 1f;
		}
		
		return current_engine_status / (float)total_engine_status;
	}
	
	// Draws a GUI line to the target location
	void DrawUiLine ()
	{
		//Draws line representing path
		lr.SetVertexCount (2);
		lr.SetPosition (0, transform.position);
		lr.SetPosition (1, target_pos);
	}
	
	// Stops drawing the line
	void ClearUiLine ()
	{
		lr.SetVertexCount (1);
	}
	
	public void RegisterEngine ()
	{
		total_engine_status += (int)Damage.DamageState.PERFECT;
		current_engine_status = total_engine_status;
	}
	
	public void ChangEngineStatus (int change)
	{
		current_engine_status += change;
	}
}
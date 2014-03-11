using UnityEngine;

/// <summary>
/// Player controller and behavior.
/// </summary>

public class PlayerScript : MonoBehaviour 
{
	/// <summary>
	/// 1 - The speed of the ship
	/// </summary>
	public Vector2 speed = new Vector2(50, 50);

	// 2 - Store the movement
	private Vector2 movement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// 3 - Retrieve axis direction
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// 4 - Movement per direction
		movement = new Vector2 (speed.x * inputX, speed.y * inputY);

		// 5 - Shooting
		bool shoot = Input.GetButtonDown ("Fire1");
		shoot |= Input.GetButtonDown ("Fire2");

		if (shoot) {
						WeaponScript weapon = GetComponent<WeaponScript> ();
						if (weapon != null) {
								weapon.Attack (false);
						}
				}
	}

	void FixedUpdate()
	{
		// 5 - Move the game object
		rigidbody2D.velocity = movement;
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Parallax scrolling script that should be assigned to a layer
/// </summary>
public class ScrollingScript : MonoBehaviour {

	/// <summary>
	/// Scrolling speed
	/// </summary>
	public Vector2 speed = new Vector2 (2, 2);

	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2 (-1, 0);

	/// <summary>
	/// Movement should be applied to camera
	/// </summary>
	public bool isLinkedToCamera = false;

	/// <summary>
	/// 1 - background is infinite
	/// </summary>
	public bool isLooping = false;

	/// <summary>
	/// 2 - List of children with a renderer.
	/// </summary>
	private List<Transform> backgroundPart;

	// 3 - Get all the children
	void Start()
	{
		// For inifinite background only
		if (isLooping) {
				// Get all the children of the layer with a renderer
				backgroundPart = new List<Transform> ();

				for (int i = 0; i<transform.childCount; i++) {
						Transform child = transform.GetChild (i);

						// Add only the visible children
						if (child.renderer != null) {
								backgroundPart.Add (child);
						}
				}
				
				// Sort by position.
				backgroundPart = backgroundPart.OrderBy(t => t.position.x).ToList();
		}
	}

	void Update()
	{
		//Movement
		Vector3 movement = new Vector3 (speed.x * direction.x, speed.y * direction.y, 0);

		movement *= Time.deltaTime;
		transform.Translate (movement);

		// Move the camera
		if (isLinkedToCamera) {
				Camera.main.transform.Translate (movement);
		}

		if (isLooping) {
			Transform firstChild = backgroundPart.FirstOrDefault();

			if (firstChild != null)
			{
				// Check if the child is already (partly) before the camera.
				// We test the position first because the IsVisibleFrom
				// method is a bit heavier to execute
				if (firstChild.position.x < Camera.main.transform.position.x)
				{
					// If the child is already on the left of the camera,
					// we test if is's completely outside and needs to be
					// recycled.
					if (firstChild.renderer.IsVisibleForm(Camera.main) == false)
					{
						// get the last child position
						Transform lastChild = backgroundPart.LastOrDefault();
						Vector3 lastposition = lastChild.transform.position;
						Vector3 lastSize = (lastChild.renderer.bounds.max - lastChild.renderer.bounds.min);

						// Set the position of the recyled one to be AFTER
						// the last child.
						// Note: Only work for horizontal scrolling currently.
						firstChild.position = new Vector3(lastposition.x + lastSize.x, firstChild.position.y, firstChild.position.z);

						// Set the recycled child to the last position
						// of the backgroundpart list.
						backgroundPart.Remove(firstChild);
						backgroundPart.Add(firstChild);
					}
				}
			}
		}
	}
}

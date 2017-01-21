using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour
{
	public Transform[] backgrounds;				// Array of all the backgrounds to be parallaxed.
	public float parallaxScale;					// The proportion of the camera's movement to move the backgrounds by.
	public float parallaxReductionFactor;		// How much less each successive layer should parallax.
	public float smoothing;						// How smooth the parallax effect should be.


	private Transform cam;						// Shorter reference to the main camera's transform.
	private Vector3 previousCamPos;				// The postion of the camera in the previous frame.

	


	void Awake ()
	{
		// Setting up the reference shortcut.
		cam = Camera.main.transform;
	}

	void Start ()
	{
		// The 'previous frame' had the current frame's camera position.
		previousCamPos = cam.position;
		
	}

	void Update ()
	{
		// The parallax is the opposite of the camera movement since the previous frame multiplied by the scale.
		//float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;

		MoveWawes();

		// Set the previousCamPos to the camera's position at the end of this frame.
		previousCamPos = cam.position;
	}

	void MoveWawes ()
	{

	}
}

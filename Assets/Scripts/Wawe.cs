using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;

public class Wawe : MonoBehaviour {

	public float MCircleRadius = 0.1f;
	public float MCircleRotateSpeed = 5f;
	private Vector2 _centre;
	private float _angle;

	public float MRotateSpeed = 1f;
	public int MRotateLimit = 30;
	private float _rotation;

	private SpriteRenderer _srend;


	// Use this for initialization
	void Start () {
		_centre = transform.position;
		_srend = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		_angle += MCircleRotateSpeed * Time.deltaTime;
		var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * MCircleRadius;
		transform.position = _centre + offset;


		_rotation = MRotateSpeed * Time.deltaTime;

		_rotation = transform.rotation.z < MRotateLimit
				? _rotation
				: -_rotation;

		transform.Rotate(0, 0, (transform.rotation.z + _rotation));


		float alfa = Mathf.Max(0.3f, Mathf.Sin(Mathf.Deg2Rad * transform.rotation.z) );

		_srend.color = new Color(_srend.color.r, _srend.color.g, _srend.color.b, alfa);
	}
}

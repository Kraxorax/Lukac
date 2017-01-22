using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering;

public class Wawe : MonoBehaviour {

	public float MCircleRadius = 0.1f;
	public float MCircleRotateSpeed = 5f;
	private Vector2 _centre;
	private float _angle;
	private Vector2 _offset;

	public Vector2 StrongDirection;

	public float MRotateSpeed = 1f;
	public int MRotateLimit = 30;
	private float _rotation;

	private SpriteRenderer _srend;


	// Use this for initialization
	void Start () {
		_centre = transform.position;
		_srend = gameObject.GetComponent<SpriteRenderer>();

		float n = Random.Range(1, 101);
		n = n / 100;
		StrongDirection = new Vector2(n, 1 - n);
	}

	
	public Vector2 getVector()
	{
		return _offset;
	}
	
	// Update is called once per frame
	void Update () {
		_angle += MCircleRotateSpeed * Time.deltaTime;
		_offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * MCircleRadius;
		transform.position = _centre + _offset;


		_rotation = MRotateSpeed * Time.deltaTime;

		_rotation = transform.rotation.z < MRotateLimit
				? _rotation
				: -_rotation;

		//transform.Rotate(0, 0, (transform.rotation.z + _rotation));

		//rigidbody.

		Vector2 on = new Vector2(_offset.x, _offset.y);
		on.Normalize();
		Vector2 res = StrongDirection - on;
		float norm = res.magnitude / 2;

		float alfa = (norm);

		_srend.color = new Color(_srend.color.r, _srend.color.g, _srend.color.b, alfa);
	}
}

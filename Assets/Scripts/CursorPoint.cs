using UnityEngine;
using System.Collections;

public class CursorPoint : MonoBehaviour
{
	public GameObject particle;
	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


		if (Physics.Raycast(ray))
			Instantiate(particle, ray.GetPoint(0), transform.rotation);
	}
}

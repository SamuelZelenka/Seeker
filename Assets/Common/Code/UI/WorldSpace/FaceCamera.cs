using UnityEngine;

public class FaceCamera : MonoBehaviour
{
	void Update()
	{
		Vector3 direction = Camera.main.transform.position - transform.position;
		transform.forward = direction;
	}
}
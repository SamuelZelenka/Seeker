using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager Instance => _instance;
    private static ArtifactManager _instance;

    [SerializeField]
    private Transform artifactPosition;

	private void Awake()
    {
        if (Instance != null ) 
        {
            Destroy(gameObject); 
            return;
        }
        _instance = this;
    }


	public static Quaternion GetRotationToArtifact(Transform origin)
	{;
		var dir = (_instance.artifactPosition.position - origin.position).normalized;
		var forward = origin.forward;
		forward.y = 0;
		dir.y = 0;

		var angle = Vector3.Angle(dir, forward);

		Vector3 crossProduct = Vector3.Cross(origin.forward, dir);
		float dotProduct = Vector3.Dot(Vector3.up, crossProduct);

		if (dotProduct < 0)
		{
			angle = -angle;
		}

		return Quaternion.Euler(-90, 0, angle + 90);
	}

}

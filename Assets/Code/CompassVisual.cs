using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CompassVisual : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private Transform compassNeedle;

    void Update()
    {
        compassNeedle.transform.localRotation = ArtifactManager.GetRotationToArtifact(playerMovement.transform);
	}
}

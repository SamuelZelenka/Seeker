using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
	[HideInInspector]
	public bool IsFacingLedge;

	[HideInInspector]
	public Vector3 Ledge;

	[Header("Climbing")]
	[SerializeField]
	private GameObject _ledgeMarker;

	private Ray _ray;

	private void UpdateLedgeDetection()
	{
		IsFacingLedge = CheckFacingLedge(out var hit);
		if (IsFacingLedge)
		{
			_ledgeMarker.SetActive(true);
		}
		else
		{
			_ledgeMarker.SetActive(false);
		}
	}

	private void SetupClimbable()
	{
		TriggerEnter += (c) =>
		{
			if (c.CompareTag("Climbable"))
			{
				SetClimbing(c.gameObject);
			}
		};

		TriggerExit += (c) =>
		{
			if (c.CompareTag("Climbable"))
			{
				SetClimbing(null);
			}
		};
	}

	private void SetClimbing(GameObject climbable)
	{
		_currentClimbable = climbable;
		if (climbable != null)
		{
			velocity = Vector3.zero;
			SetControllerState<ClimbState>();
		}
		else
		{
			SetControllerState<DefaultState>();
		}
	}

	private bool CheckFacingLedge(out RaycastHit hit)
	{
		var isFacing = false;
		_ray = new Ray(_cameraController.transform.position, _cameraController.transform.forward);

		var range = _characterConfig.LedgeDetectionRange;

		Physics.Raycast(_ray, out hit, range, LayerMask.GetMask("Climbable"));

		if (Vector3.Angle(_cameraController.transform.forward, _characterController.transform.forward) > _characterConfig.LedgeDetectionAngle)
		{
			return false;
		}

		if (hit.transform != null)
		{
			var ledgePos = hit.collider.ClosestPoint(hit.point + new Vector3(0, 2, 0)); ;
			var deltaSqrDist = Vector3.SqrMagnitude(ledgePos - transform.position);
			var isInRange = deltaSqrDist < range * range;

			if (isInRange)
			{
				_ledgeMarker.transform.position = ledgePos - new Vector3(0, 0.1f, 0);
				Ledge = ledgePos;
				isFacing = true;
			}
		}

		return isFacing;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(_ray.origin, _ray.GetPoint(_characterConfig.LedgeDetectionRange));
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(Ledge, 0.05f);
	}
}

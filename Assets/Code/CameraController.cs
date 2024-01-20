using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private GameObject _player;

	[SerializeField]
	private Transform _cameraParent;

	[SerializeField]
	private float _mouseSensitivity;

	[SerializeField]
	private Vector3 _offset;

	private float _verticalRotation;

	[SerializeField]
	private Animator _animator;

	// Start is called before the first frame update
	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void SetBobbingSpeed(float speed)
	{
		_animator.speed = speed;
	}

	// Update is called once per frame
	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Locked : CursorLockMode.None;
		}

		// Camera rotation
		float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
		float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

		_verticalRotation -= mouseY;
		_verticalRotation = Mathf.Clamp(_verticalRotation, -90, 90);

		_cameraParent.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
		_player.transform.Rotate(Vector3.up * mouseX);
		_cameraParent.localPosition = _offset;
	}
}
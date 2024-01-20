using System;
using Unity.VisualScripting;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
	public Action<Collider> OnEnter { get; private set; }
	public Action<Collider> OnExit { get; private set; }

	public Vector3 velocity;

	private GameObject _currentClimbable;
	private CharacterController _characterController;
	private CharacterMovementInfo _moveInfo;

	public GameObject CurrentClimbable => _currentClimbable;

	[SerializeField]
	private CharacterConfig _characterConfig;

	[Header("Camera")]
	[SerializeField]
	private CameraController _cameraController;


	public ControllerState controllerState;

	private void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_characterController.radius = _characterConfig.Radius;
		_moveInfo = new CharacterMovementInfo(_characterController);

		controllerState = new DefaultState(_moveInfo, this, _cameraController, _characterConfig, _characterController);
	}

	private void Update()
	{
		controllerState.EarlyUpdate();
		UpdateGravity();
		controllerState.Update();
		Friction();
		MoveCharacter();
		controllerState.LateUpdate();
	}

	private void UpdateGravity()
	{
		var enableGravity = !_characterController.isGrounded && _currentClimbable == null;
		if (_characterController.velocity.y > 0 && enableGravity)
		{
			if (_characterController.collisionFlags == CollisionFlags.Above)
				velocity.y = -1f;
		}
		velocity += Physics.gravity * Time.deltaTime;
	}

	private void Friction()
	{
		velocity.x = Mathf.Lerp(velocity.x, 0, _characterConfig.GroundFriction * Time.deltaTime);
		velocity.z = Mathf.Lerp(velocity.z, 0, _characterConfig.GroundFriction * Time.deltaTime);
	}

	private void MoveCharacter()
	{
		_characterController.Move(velocity * Time.deltaTime);

		if (_characterController.isGrounded)
		{
			velocity.y = 0;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		OnEnter(other);
		if (other.CompareTag("Climbable"))
		{
			SetClimbing(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		OnExit(other);
		if (other.CompareTag("Climbable"))
		{
			SetClimbing(null);
		}
	}

	private void SetClimbing(GameObject climbable)
	{
		_currentClimbable = climbable;
		_moveInfo.IsClimbing = climbable != null;
	}
}

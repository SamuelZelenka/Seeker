using UnityEngine;

public partial class PlayerMovement : MonoBehaviour
{
	private GameObject _currentClimbable;
	private Vector3 _velocity;
	private CharacterController _characterController;
	private InputData _inputData;

	[SerializeField]
	private CharacterConfig _characterConfig;

	[Header("Camera")]
	[SerializeField]
	private CameraController _cameraController;

	private void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_inputData = new InputData(_characterController);
	}

	private void Update()
	{
		UpdateMoveVelocity();
		UpdateGravity();
		Jump();
		Crouch();
		Friction();
		MoveCharacter();
		UpdateGrounded();
	}

	private void UpdateMoveVelocity()
	{
		float horizontalInput = _inputData.HorizontalInput;
		float verticalInput = _inputData.VerticalInput;
		bool isRunning = _inputData.IsRunning;
		float crouching = _inputData.Crouching;

		Vector3 inputDirection = transform.right * horizontalInput;

		if (!_inputData.IsClimbing || _characterController.isGrounded)
			inputDirection += transform.forward * verticalInput;

		//TODO: Look into movement states
		float moveSpeedMultiplier = isRunning ? _characterConfig.RunSpeedMultiplier : _characterConfig.WalkSpeedMultiplier;
		moveSpeedMultiplier = crouching > 0 ? _characterConfig.CrouchSpeedMultiplier : moveSpeedMultiplier;

		float bobbingSpeed = inputDirection.magnitude > 0.01f ? moveSpeedMultiplier : 0;

		_cameraController.SetBobbingSpeed(bobbingSpeed);

		inputDirection.Normalize();

		HandleClimbing(verticalInput);

		_velocity.x += inputDirection.x * moveSpeedMultiplier * _characterConfig.GroundAcceleration * Time.deltaTime;
		_velocity.z += inputDirection.z * moveSpeedMultiplier * _characterConfig.GroundAcceleration * Time.deltaTime;
	}

	private void HandleClimbing(float verticalInput)
	{
		if (_inputData.IsClimbing)
		{
			_velocity.y = 0;

			var climbVelocity = (transform.forward.y + _cameraController.transform.forward.y + 0.2f) * _characterConfig.ClimbSpeed;

			if (verticalInput < 0)
				_velocity.y += Mathf.Abs(climbVelocity) * verticalInput;
			else
				_velocity.y += climbVelocity * verticalInput;
		}
	}

	private void UpdateGravity()
	{
		var enableGravity = !_characterController.isGrounded && _currentClimbable == null;
		if (_characterController.velocity.y > 0 && enableGravity)
		{
			if (_characterController.collisionFlags == CollisionFlags.Above)
				_velocity.y = -1f;
		}
		else
		{
			_velocity += Physics.gravity * Time.deltaTime;
		}
	}

	private void Jump()
	{
		var allowJump = _inputData.IsGrounded || _inputData.IsClimbing;

		if (_inputData.JumpInput > 0 && allowJump)
		{
			if (_inputData.IsClimbing)
			{
				var isFacingClimbable = Vector3.Dot(transform.forward, transform.position - _currentClimbable.transform.position) < -0.3f;

				if (isFacingClimbable)
					_velocity += -transform.forward * _characterConfig.JumpImpulse * 0.15f;
				else
					_velocity += transform.forward * _characterConfig.JumpImpulse * 0.15f;
				return;
			}
			_velocity.y = _characterConfig.JumpImpulse;
		}
	}

	private void Crouch()
	{
		_characterController.height = Mathf.Lerp(_characterConfig.StandHeight, _characterConfig.CrouchHeight, _inputData.Crouching);
	}

	private void Friction()
	{
		_velocity.x = Mathf.Lerp(_velocity.x, 0, _characterConfig.GroundFriction * Time.deltaTime);
		_velocity.z = Mathf.Lerp(_velocity.z, 0, _characterConfig.GroundFriction * Time.deltaTime);
	}

	private void MoveCharacter()
	{
		_characterController.Move(_velocity * Time.deltaTime);
	}

	private void UpdateGrounded()
	{
		if (_characterController.isGrounded)
		{
			_velocity.y = 0;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Climbable"))
		{
			SetClimbing(other.gameObject);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Climbable"))
		{
			SetClimbing(null);
		}
	}

	private void SetClimbing(GameObject climbable)
	{
		_currentClimbable = climbable;
		_inputData.IsClimbing = climbable != null;
	}
}
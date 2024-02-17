using UnityEngine;

public class DefaultState : ControllerState
{
	public DefaultState(ControllerState state) : base(state)
	{
		Setup();
	}

	public DefaultState(
		PlayerInput moveInfo,
		PlayerController playerController,
		CameraController cameraController,
		CharacterConfig characterConfig,
		CharacterController characterController) : base(
		 moveInfo,
		 characterConfig,
		 playerController,
		 characterController,
		 cameraController)
	{
		Setup();
	}

	private void Setup()
	{
		_earlyActions.Add(Move);
		_earlyActions.Add(UpdateGravity);
		_actions.Add(Jump);
		_actions.Add(Crouch);
		_lateActions.Add(Friction);
		_lateActions.Add(MoveCharacter);
	}

	public override void Move()
	{
		float hInput = _moveInfo.HorizontalInput;
		float vInput = _moveInfo.VerticalInput;

		Transform playerTrans = _playerController.transform;
		Vector3 inputDir = playerTrans.right * hInput;
		inputDir += playerTrans.forward * vInput;

		//TODO: Move Bobbing Speed somewhere else
		CameraBobbing(inputDir);
		//TODO: Move Bobbing Speed somewhere else

		inputDir.Normalize();

		var controllerState = _playerController.controllerState;
		var moveSpeedMultiplier = controllerState.GetSpeedMultiplier(_moveInfo, _characterConfig);

		var xSpeed = inputDir.x * moveSpeedMultiplier * _characterConfig.GroundAcceleration;
		var zSpeed = inputDir.z * moveSpeedMultiplier * _characterConfig.GroundAcceleration;

		_playerController.velocity.x += xSpeed * Time.deltaTime;
		_playerController.velocity.z += zSpeed * Time.deltaTime;
	}

	//TODO: Move Bobbing Speed somewhere else
	private void CameraBobbing(Vector3 inputDir)
	{
		float bobSpeed = inputDir.magnitude > 0.01f ? 1 : 0;
		bobSpeed = _moveInfo.IsRunning ? 1.4f : bobSpeed;
		_cameraController.SetBobbingSpeed(bobSpeed);
	}

	public override void Jump()
	{
		var allowJump = _moveInfo.IsGrounded;

		if (_moveInfo.JumpInput > 0 && allowJump)
		{
			_playerController.velocity.y = _characterConfig.JumpImpulse;
		}
	}

	public override void Crouch()
	{
		var standHeight = _characterConfig.StandHeight;
		var crouchHeight = _characterConfig.CrouchHeight;
		var crouchT = _moveInfo.CrouchingInput * Time.time;
		_characterController.height = Mathf.Lerp(standHeight, crouchHeight, crouchT);
	}

	public override float GetSpeedMultiplier(PlayerInput moveInfo, CharacterConfig CharConfig)
	{
		float moveSpeedMultiplier = moveInfo.IsRunning ? CharConfig.RunSpeedMultiplier : CharConfig.WalkSpeedMultiplier;
		moveSpeedMultiplier = moveInfo.CrouchingInput > 0 ? CharConfig.CrouchSpeedMultiplier : moveSpeedMultiplier;
		moveSpeedMultiplier = moveInfo.IsClimbing ? CharConfig.ClimbSpeedMultiplier : moveSpeedMultiplier;
		return moveSpeedMultiplier;
	}

	private void UpdateGravity()
	{
		var enableGravity = !_characterController.isGrounded && !_moveInfo.IsClimbing;
		if (enableGravity)
		{
			if (_characterController.velocity.y > 0)
			{
				if (_characterController.collisionFlags == CollisionFlags.Above)
				{
					_playerController.velocity.y = -1f;
				}
			}
			_playerController.velocity += Physics.gravity * Time.deltaTime;
		}
		_moveInfo.IsGrounded = _characterController.isGrounded;
	}

	private void Friction()
	{
		_playerController.velocity.x = Mathf.Lerp(_playerController.velocity.x, 0, _characterConfig.GroundFriction * Time.deltaTime);
		_playerController.velocity.z = Mathf.Lerp(_playerController.velocity.z, 0, _characterConfig.GroundFriction * Time.deltaTime);
	}

	private void MoveCharacter()
	{
		base.MoveCharacter();
		if (_characterController.isGrounded)
		{
			_playerController.velocity.y = 0;
		}
	}
}
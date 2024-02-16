using UnityEngine;

public class ClimbState : ControllerState
{
	public ClimbState(ControllerState controllerState) : base(controllerState) 
	{
		Setup();
	}

	public ClimbState(
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
		_actions.Add(Jump);
		_lateActions.Add(MoveCharacter);
	}

	public override float GetSpeedMultiplier(PlayerInput moveInfo, CharacterConfig CharConfig)
	{
		return CharConfig.ClimbSpeedMultiplier;
	}

	//TODO: fix climbing movement
	public override void Move()
	{
		float hInput = _moveInfo.HorizontalInput;
		float vInput = _moveInfo.VerticalInput;

		Transform playerTrans = _playerController.transform;
		Vector3 inputDir = playerTrans.right * hInput;

				if (_moveInfo.IsGrounded)
			inputDir += playerTrans.forward * vInput;

		_playerController.velocity = Vector3.zero;

		var climbVelocity = (_playerController.transform.forward.y + _cameraController.transform.forward.y + 0.2f) * _characterConfig.ClimbSpeedMultiplier;

		if (vInput < 0)
			_playerController.velocity.y += climbVelocity * vInput;
		else
			_playerController.velocity.y += climbVelocity * vInput;

		var xSpeed = inputDir.x * _characterConfig.ClimbSpeedMultiplier;
		var zSpeed = inputDir.z * _characterConfig.ClimbSpeedMultiplier;

		_playerController.velocity.x += xSpeed;
		_playerController.velocity.z += zSpeed;
	}

	public override void Jump()
	{
		if (_moveInfo.JumpInput == 0)
			return;

		var playerForward = _playerController.transform.forward;
		var climbablePosition = _playerController.CurrentClimbable.transform.position;
		var climbableDir = _playerController.transform.position - climbablePosition;
		var dot = Vector3.Dot(playerForward, climbableDir);
		var isFacingClimbable = dot < -0.3f;

		if (isFacingClimbable)
		{
			_playerController.velocity += -_playerController.transform.forward * _characterConfig.JumpImpulse;
		}
		else
		{
			_playerController.velocity += _playerController.transform.forward * _characterConfig.JumpImpulse;
		}
	}
}
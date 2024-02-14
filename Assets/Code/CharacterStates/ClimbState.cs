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
	}

	public override float GetSpeedMultiplier(PlayerInput moveInfo, CharacterConfig CharConfig)
	{
		return CharConfig.ClimbSpeedMultiplier;
	}

	public override void Move()
	{
		float verticalInput = _moveInfo.VerticalInput;

		_playerController.velocity = Vector3.zero;

		var climbVelocity = (_playerController.transform.forward.y + _cameraController.transform.forward.y + 0.2f) * _characterConfig.ClimbSpeed;

		if (verticalInput < 0)
			_playerController.velocity.y += Mathf.Abs(climbVelocity) * verticalInput;
		else
			_playerController.velocity.y += climbVelocity * verticalInput;
		
	}

	public override void Jump()
	{
		var playerForward = _playerController.transform.forward;
		var climbablePosition = _playerController.CurrentClimbable.transform.position;
		var climbableDir = _playerController.transform.position - climbablePosition;
		var dot = Vector3.Dot(playerForward, climbableDir);
		var isFacingClimbable = dot < -0.3f;

		if (isFacingClimbable)
		{
			_playerController.velocity += -_playerController.transform.forward * _characterConfig.JumpImpulse * 0.15f;
		}
		else
		{
			_playerController.velocity += _playerController.transform.forward * _characterConfig.JumpImpulse * 0.15f;
		}
		return;
	}
}
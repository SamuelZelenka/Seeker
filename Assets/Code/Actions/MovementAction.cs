using UnityEngine;

public class MovementAction : PlayerAction
{
	private CharacterMovementInfo _moveInfo;
	private PlayerController _playerController;
	private CameraController _cameraController;
	private CharacterConfig _characterConfig;

	public MovementAction(
		CharacterMovementInfo moveInfo,
		PlayerController playerController,
		CameraController cameraController,
		CharacterConfig config)
	{
		_moveInfo = moveInfo;
		_playerController = playerController;
		_cameraController = cameraController;
		_characterConfig = config;
	}

	public override void Invoke()
	{
		float horizontalInput = _moveInfo.HorizontalInput;
		float verticalInput = _moveInfo.VerticalInput;

		Vector3 inputDirection = _playerController.transform.right * horizontalInput;

		if (!_moveInfo.IsClimbing || _moveInfo.IsGrounded)
			inputDirection += _playerController.transform.forward * verticalInput;

		var moveSpeedMultiplier = _playerController.controllerState.GetSpeedMultiplier(_moveInfo, _characterConfig);

		//Move Bobbing Speed to own action
		float bobbingSpeed = inputDirection.magnitude > 0.01f ? 1 : 0;
		bobbingSpeed = _moveInfo.IsRunning ? 1.4f : bobbingSpeed;

		_cameraController.SetBobbingSpeed(bobbingSpeed);

		inputDirection.Normalize();

		HandleClimbing(verticalInput);

		_playerController.velocity.x += inputDirection.x * moveSpeedMultiplier * _characterConfig.GroundAcceleration * Time.deltaTime;
		_playerController.velocity.z += inputDirection.z * moveSpeedMultiplier * _characterConfig.GroundAcceleration * Time.deltaTime;
	}



	private void HandleClimbing(float verticalInput)
	{
		if (_moveInfo.IsClimbing)
		{
			_playerController.velocity.y = 0;

			var climbVelocity = (_playerController.transform.forward.y + _cameraController.transform.forward.y + 0.2f) * _characterConfig.ClimbSpeed;

			if (verticalInput < 0)
				_playerController.velocity.y += Mathf.Abs(climbVelocity) * verticalInput;
			else
				_playerController.velocity.y += climbVelocity * verticalInput;
		}
	}
}

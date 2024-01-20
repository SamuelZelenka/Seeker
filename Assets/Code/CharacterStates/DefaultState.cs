using UnityEngine;

public class DefaultState : ControllerState
{
	public DefaultState(
		CharacterMovementInfo moveInfo,
		PlayerController playerController,
		CameraController cameraController,
		CharacterConfig characterConfig,
		CharacterController characterController)
	{
		_earlyActions.Add(new MovementAction(
			moveInfo,
			playerController,
			cameraController,
			characterConfig));

		_actions.Add(new JumpAction(
			moveInfo,
			playerController,
			characterConfig));

		_actions.Add(new CrouchAction(
			characterController,
			characterConfig,
			moveInfo));
	}
	public override float GetSpeedMultiplier(CharacterMovementInfo moveInfo, CharacterConfig CharConfig)
	{
		float moveSpeedMultiplier = moveInfo.IsRunning ? CharConfig.RunSpeedMultiplier : CharConfig.WalkSpeedMultiplier;
		moveSpeedMultiplier = moveInfo.Crouching > 0 ? CharConfig.CrouchSpeedMultiplier : moveSpeedMultiplier;
		return moveSpeedMultiplier;
	}
}

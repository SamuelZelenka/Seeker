using UnityEngine;

public class ClimbState : ControllerState
{
	public ClimbState(
		PlayerInput moveInfo,
		PlayerData playerData,
		SkillController skillController,
		PlayerController playerController,
		CameraController cameraController,
		CharacterConfig characterConfig,
		CharacterController characterController) : base(
		 moveInfo,
		 playerData,
		 skillController,
		 characterConfig,
		 playerController,
		 characterController,
		 cameraController)
	{
		Setup();
	}

	private void Setup()
	{
		earlyActions.Add(Move);
		actions.Add(Jump);
		lateActions.Add(MoveCharacter);
	}

	public override float GetSpeedMultiplier(PlayerInput moveInfo, CharacterConfig CharConfig)
	{
		return CharConfig.ClimbSpeedMultiplier;
	}

	//TODO: fix climbing movement
	public override void Move()
	{
		float hInput = moveInfo.HorizontalInput;
		float vInput = moveInfo.VerticalInput;

		Transform playerTrans = playerController.transform;
		Vector3 inputDir = playerTrans.right * hInput;

				if (moveInfo.IsGrounded)
			inputDir += playerTrans.forward * vInput;

		playerController.velocity = Vector3.zero;

		var climbVelocity = (playerController.transform.forward.y + cameraController.transform.forward.y + 0.2f) * characterConfig.ClimbSpeedMultiplier;

		if (vInput < 0)
			playerController.velocity.y += climbVelocity * vInput;
		else
			playerController.velocity.y += climbVelocity * vInput;

		var xSpeed = inputDir.x * characterConfig.ClimbSpeedMultiplier;
		var zSpeed = inputDir.z * characterConfig.ClimbSpeedMultiplier;

		playerController.velocity.x += xSpeed;
		playerController.velocity.z += zSpeed;
	}

	public override void Jump()
	{
		if (moveInfo.JumpInput == 0)
			return;

		var playerForward = playerController.transform.forward;
		var climbablePosition = playerController.CurrentClimbable.transform.position;
		var climbableDir = playerController.transform.position - climbablePosition;
		var dot = Vector3.Dot(playerForward, climbableDir);
		var isFacingClimbable = dot < -0.3f;

		if (isFacingClimbable)
		{
			playerController.velocity += -playerController.transform.forward * characterConfig.JumpImpulse;
		}
		else
		{
			playerController.velocity += playerController.transform.forward * characterConfig.JumpImpulse;
		}
	}
}
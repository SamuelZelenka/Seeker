﻿using UnityEngine;

public class ClimbState : ControllerState
{
	public ClimbState(ControllerStateArgs args) : base(args)
	{
		Setup();
	}

	private void Setup()
	{
		earlyActions.Add(Move);
		actions.Add(Jump);
		lateActions.Add(MoveCharacter);
	}

	public override float GetSpeedMultiplier(MoveInfo moveInfo, CharacterConfig CharConfig)
	{
		return CharConfig.ClimbSpeedMultiplier;
	}

	public override void Move()
	{
		float hInput = moveInfo.HorizontalInput;
		float vInput = moveInfo.VerticalInput;

		Transform playerTrans = playerController.transform;
		Vector3 inputDir = playerTrans.right * hInput;

		RaycastHit hit;
		bool isGrounded = Physics.Raycast(playerTrans.position, -playerTrans.up, out hit, characterConfig.StandHeight * 0.55f);

		if (isGrounded)
			inputDir += playerTrans.forward * vInput;

		playerController.velocity = Vector3.zero;

		var climbVelocity = (playerController.transform.forward.y + cameraController.transform.forward.y + 0.2f) * characterConfig.ClimbSpeedMultiplier;

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
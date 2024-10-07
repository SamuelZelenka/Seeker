using UnityEngine;


public class CrouchState : DefaultState
{
	public CrouchState(
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
			playerController,
			cameraController,
			characterConfig,
			characterController)
	{
		base.moveInfo = moveInfo;
		Setup();
	}
}

public class DefaultState : ControllerState
{
	private float _lastGroundTime = 0;
	public DefaultState(
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
		base.moveInfo = moveInfo;
		Setup();
	}

	protected virtual void Setup()
	{
		earlyActions.Add(Move);
		earlyActions.Add(UpdateGravity);
		actions.Add(Jump);
		actions.Add(Crouch);
		actions.Add(NextSkill);
		actions.Add(PreviousSkill);
		lateActions.Add(Friction);
		lateActions.Add(MoveCharacter);
	}

	public override void Move()
	{
		float hInput = moveInfo.HorizontalInput;
		float vInput = moveInfo.VerticalInput;

		Transform playerTrans = playerController.transform;
		Vector3 inputDir = playerTrans.right * hInput * characterConfig.SidewaysMultiplier;

		var verticalMultiplier = vInput >= 0 ? characterConfig.ForwardMultiplier : characterConfig.BackwardMultiplier;
		inputDir += playerTrans.forward * vInput * verticalMultiplier;

		//TODO: Move Bobbing Speed somewhere else
		CameraBobbing(inputDir);
		//TODO: Move Bobbing Speed somewhere else


		var controllerState = playerController.currentControllerState;
		var moveSpeedMultiplier = controllerState.GetSpeedMultiplier(moveInfo, characterConfig);

		var xSpeed = inputDir.x * moveSpeedMultiplier;
		var zSpeed = inputDir.z * moveSpeedMultiplier;

		if (moveInfo.IsGrounded)
		{
			xSpeed *= characterConfig.GroundAcceleration;
			zSpeed *= characterConfig.GroundAcceleration;
		}
        else
        {
			xSpeed *= characterConfig.AirAcceleration;
			zSpeed *= characterConfig.AirAcceleration;
		}

		var normalizedHorizontalSpeed = new Vector2(xSpeed, zSpeed);

		playerController.velocity.x += normalizedHorizontalSpeed.x * Time.deltaTime;
		playerController.velocity.z += normalizedHorizontalSpeed.y * Time.deltaTime;
	}

	//TODO: Move Bobbing Speed somewhere else
	private void CameraBobbing(Vector3 inputDir)
	{
		float bobSpeed = inputDir.magnitude > 0.01f ? 1 : 0;
		bobSpeed = moveInfo.IsRunning ? 1.4f : bobSpeed;
		cameraController.SetBobbingSpeed(bobSpeed);
	}

	public override void Jump()
	{
		var allowJump = _lastGroundTime < Time.time + characterConfig.JumpGracePeriod;

		if (moveInfo.JumpInput > 0)
		{
			if (playerController.IsFacingLedge)
			{
				var vaultState = playerController.SetControllerState<VaultState>();
				vaultState.Start(playerController.Ledge + Vector3.up * characterController.height * 0.5f);
				return;
			}
			if (allowJump)
			{
				_lastGroundTime = Mathf.Infinity;
				playerController.velocity.y = characterConfig.JumpImpulse;
			}
		}
	}

	private float _crouchT;

	public override void Crouch()
	{
		var standHeight = characterConfig.StandHeight;
		var crouchHeight = characterConfig.CrouchHeight;

		if (moveInfo.CrouchingInput > 0)
		{
			_crouchT += 2 * Time.deltaTime;
		}
		else
		{
			_crouchT -= 2 * Time.deltaTime;
		}

		_crouchT = Mathf.Clamp01(_crouchT);
		characterController.height = Mathf.Lerp(standHeight, crouchHeight, _crouchT);
	}

	public override float GetSpeedMultiplier(PlayerInput moveInfo, CharacterConfig CharConfig)
	{
		float moveSpeedMultiplier = moveInfo.IsRunning ? CharConfig.RunSpeedMultiplier : CharConfig.WalkSpeedMultiplier;
		moveSpeedMultiplier = moveInfo.CrouchingInput > 0 ? CharConfig.CrouchSpeedMultiplier : moveSpeedMultiplier;
		return moveSpeedMultiplier;
	}

	private void UpdateGravity()
	{
		var enableGravity = !characterController.isGrounded && !moveInfo.IsClimbing;
		if (enableGravity)
		{
			if (characterController.velocity.y > 0)
			{
				if (characterController.collisionFlags == CollisionFlags.Above)
				{
					playerController.velocity.y = -1f;
				}
			}
			playerController.velocity += Physics.gravity * Time.deltaTime;
		}
		moveInfo.IsGrounded = characterController.isGrounded;
		if (moveInfo.IsGrounded)
		{
			_lastGroundTime = Time.time;
		}
	}

	private void Friction()
	{
		if (moveInfo.IsGrounded)
		{
			playerController.velocity.x = Mathf.Lerp(playerController.velocity.x, 0, characterConfig.GroundFriction * Time.deltaTime);
			playerController.velocity.z = Mathf.Lerp(playerController.velocity.z, 0, characterConfig.GroundFriction * Time.deltaTime);
		}
		else
		{
			playerController.velocity.x = Mathf.Lerp(playerController.velocity.x, 0, characterConfig.AirResistance * Time.deltaTime);
			playerController.velocity.z = Mathf.Lerp(playerController.velocity.z, 0, characterConfig.AirResistance * Time.deltaTime);
		}
	}

	protected override void MoveCharacter()
	{
		base.MoveCharacter();
		if (characterController.isGrounded)
		{
			playerController.velocity.y = 0;
		}
	}

	private void NextSkill()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			skillController.NextSkill();
		}
	}

	private void PreviousSkill()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			skillController.PreviousSkill();
		}
	}
}
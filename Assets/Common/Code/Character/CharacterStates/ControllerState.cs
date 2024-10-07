using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerState
{
	protected PlayerInput moveInfo;
	protected PlayerData playerData;
	protected SkillController skillController;
	protected CharacterConfig characterConfig;
	protected PlayerController playerController;
	protected CharacterController characterController;
	protected CameraController cameraController;

	protected List<Action> startActions = new();

	protected List<Action> earlyActions = new();
	protected List<Action> actions = new();
	protected List<Action> lateActions = new();

	public ControllerState(ControllerStateArgs args)
	{
		moveInfo = args.moveInfo;
		playerData = args.playerData;
		skillController = args.skillController;
		characterConfig = args.characterConfig;
		playerController = args.playerController;
		characterController = args.characterController;
		cameraController = args.cameraController;
	}
	public virtual void Start() => startActions.ForEach((a) => a.Invoke());
	public virtual void EarlyUpdate() => earlyActions.ForEach((a) => a.Invoke());
	public virtual void Update() => actions.ForEach((a) => a.Invoke());
	public virtual void LateUpdate() => lateActions.ForEach((a) => a.Invoke());

	public abstract float GetSpeedMultiplier(PlayerInput moveInfo, CharacterConfig CharConfig);
	public virtual void Move() { }
	public virtual void Jump() { }

	protected virtual void MoveCharacter()
	{
		characterController.Move(playerController.velocity * Time.deltaTime);
	}
}

public struct ControllerStateArgs
{
    public PlayerInput moveInfo;
    public PlayerData playerData;
    public SkillController skillController;
    public CharacterConfig characterConfig;
    public PlayerController playerController;
    public CharacterController characterController;
    public CameraController cameraController;

    public ControllerStateArgs(
    PlayerInput playerInput,
    PlayerData playerData,
    SkillController skillController,
    CharacterConfig characterConfig,
    PlayerController playerController,
    CharacterController characterController,
    CameraController cameraController)
    {
        this.moveInfo = playerInput;
        this.playerData = playerData;
        this.skillController = skillController;
        this.characterConfig = characterConfig;
        this.playerController = playerController;
        this.characterController = characterController;
        this.cameraController = cameraController;
    }
}

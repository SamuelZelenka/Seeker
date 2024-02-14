using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerState
{
	protected PlayerInput _moveInfo;
	protected CharacterConfig _characterConfig;
	protected PlayerController _playerController;
	protected CharacterController _characterController;
	protected CameraController _cameraController;

	protected List<Action> _earlyActions = new();
	protected List<Action> _actions = new();
	protected List<Action> _lateActions = new();

	public ControllerState(ControllerState controllerState)
	{
		_moveInfo = controllerState._moveInfo;
		_characterConfig = controllerState._characterConfig;
		_playerController = controllerState._playerController;
		_characterController = controllerState._characterController;
		_cameraController = controllerState._cameraController;
	}

	public ControllerState(
		PlayerInput moveInfo,
		CharacterConfig characterConfig,
		PlayerController playerController,
		CharacterController characterController,
		CameraController cameraController)
	{
		_moveInfo = moveInfo;
		_characterConfig = characterConfig;
		_playerController = playerController;
		_characterController = characterController;
		_cameraController = cameraController;
	}

	public virtual void EarlyUpdate() => _earlyActions.ForEach((a) => a.Invoke());
	public virtual void Update() => _actions.ForEach((a) => a.Invoke());
	public virtual void LateUpdate() => _lateActions.ForEach((a) => a.Invoke());

	public abstract float GetSpeedMultiplier(PlayerInput moveInfo, CharacterConfig CharConfig);
	public virtual void Move() {}
	public virtual void Jump() { }
	public virtual void Crouch() { }
}

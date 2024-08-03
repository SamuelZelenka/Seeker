using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
	public delegate void TriggerCollisionHandler(Collider collider);

	public GameObject CurrentClimbable => _currentClimbable;

	public TriggerCollisionHandler TriggerEnter;
	public TriggerCollisionHandler TriggerExit;

	public Dictionary<Type, ControllerState> _controllerStates = new();

	public ControllerState currentControllerState;

	public Vector3 velocity;

	public SkillEvent OnNextSkill;
	public SkillEvent OnPreviousSkill;
	public SkillEvent OnSkillChanged;
	public SkillEvent OnSkillUsed;

	public SkillController SkillController;

	[Header("Camera")]
	[SerializeField]
	private CameraController _cameraController;

	[SerializeField]
	private PlayerInput _inputData;

	[SerializeField]
	private PlayerData _playerData;

	[SerializeField]
	private CharacterConfig _characterConfig;

	private GameObject _currentClimbable;
	private CharacterController _characterController;

	private void Start()
	{
		SkillController = new SkillController(_playerData, OnNextSkill, OnPreviousSkill, OnSkillUsed, OnSkillChanged);
		_characterController = GetComponent<CharacterController>();
		_characterController.radius = _characterConfig.Radius;

		_controllerStates.Add(typeof(DefaultState), new DefaultState(_inputData, _playerData, SkillController, this, _cameraController, _characterConfig, _characterController));
		_controllerStates.Add(typeof(ClimbState), new ClimbState(_inputData, _playerData, SkillController, this, _cameraController, _characterConfig, _characterController));
		_controllerStates.Add(typeof(VaultState), new VaultState(_inputData, _playerData, SkillController, this, _cameraController, _characterConfig, _characterController));
		currentControllerState = _controllerStates[typeof(DefaultState)];

		//OnSkillChanged.Subscribe(SetActiveSkill);

		SetupTriggerActions();
	}

	private void Update()
	{
		UpdateInputs();
		currentControllerState.EarlyUpdate();
		currentControllerState.Update();
		UpdateLedgeDetection();
		currentControllerState.LateUpdate();
	}
	public T SetControllerState<T>() where T : ControllerState
	{
		currentControllerState = _controllerStates[typeof(T)];
		return currentControllerState as T;
	}

	private void SetupTriggerActions()
	{
		SetupClimbable();
	}

	private void UpdateInputs()
	{
		_inputData.HorizontalInput = Input.GetAxis("Horizontal");
		_inputData.VerticalInput = Input.GetAxis("Vertical");
		_inputData.JumpInput = Input.GetAxis("Jump");
		_inputData.CrouchingInput = Input.GetAxis("Crouch");
		_inputData.IsRunning = Input.GetKey(KeyCode.LeftShift);
	}

	private void OnTriggerEnter(Collider other)
	{
		TriggerEnter?.Invoke(other);
	}

	private void OnTriggerExit(Collider other)
	{
		TriggerExit?.Invoke(other);
	}
}
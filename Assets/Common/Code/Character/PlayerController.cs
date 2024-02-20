using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
	private const string DEFAULT_CONTROLLER_STATE = "DEFAULT";
	private const string CLIMB_CONTROLLER_STATE = "CLIMB";

	public delegate void TriggerCollisionHandler(Collider collider);


	public GameObject CurrentClimbable => _currentClimbable;

	public TriggerCollisionHandler TriggerEnter;
	public TriggerCollisionHandler TriggerExit;

	public Dictionary<string, ControllerState> _controllerStates = new();

	public ControllerState currentControllerState;

	public Vector3 velocity;

	public IntegerEvent OnSkillChanged;
	public IntegerEvent OnSkillUsed;

	[Header("Camera")]
	[SerializeField]
	private CameraController _cameraController;

	[SerializeField]
	private PlayerInput _moveInfo;

	[SerializeField]
	private PlayerData _playerData;

	[SerializeField]
	private CharacterConfig _characterConfig;

	private Skill _activeSkill;

	private GameObject _currentClimbable;
	private CharacterController _characterController;

	private void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_characterController.radius = _characterConfig.Radius;
		_moveInfo = new PlayerInput();

		_controllerStates.Add(DEFAULT_CONTROLLER_STATE, new DefaultState(_moveInfo, this, _cameraController, _characterConfig, _characterController));
		_controllerStates.Add(CLIMB_CONTROLLER_STATE, new ClimbState(_moveInfo, this, _cameraController, _characterConfig, _characterController));
		currentControllerState = _controllerStates[DEFAULT_CONTROLLER_STATE];

		//OnSkillChanged.Subscribe(SetActiveSkill);

		SetupTriggerActions();
	}

	private void Update()
	{
		UpdateInputs();
		currentControllerState.EarlyUpdate();
		currentControllerState.Update();
		currentControllerState.LateUpdate();
	}

	private void SetActiveSkill(int id)
	{
		_activeSkill = _playerData.GetSkill(id);
	}

	public void PerformSkill()
	{
		OnSkillUsed.Invoke(_activeSkill.GetID());
	}

	private void SetupTriggerActions()
	{
		SetupClimbable();
	}

	private void SetupClimbable()
	{
		TriggerEnter += (c) =>
		{
			if (c.CompareTag("Climbable"))
			{
				SetClimbing(c.gameObject);
			}
		};

		TriggerExit += (c) =>
		{
			if (c.CompareTag("Climbable"))
			{
				SetClimbing(null);
			}
		};
	}

	private void UpdateInputs()
	{
		_moveInfo.HorizontalInput = Input.GetAxis("Horizontal");
		_moveInfo.VerticalInput = Input.GetAxis("Vertical");
		_moveInfo.JumpInput = Input.GetAxis("Jump");
		_moveInfo.CrouchingInput = Input.GetAxis("Crouch");
		_moveInfo.IsRunning = Input.GetKey(KeyCode.LeftShift);
	}

	private void OnTriggerEnter(Collider other)
	{
		TriggerEnter?.Invoke(other);
	}

	private void OnTriggerExit(Collider other) => TriggerExit?.Invoke(other);

	private void SetClimbing(GameObject climbable)
	{
		_currentClimbable = climbable;
		if (climbable != null)
		{
			velocity = Vector3.zero;
			currentControllerState = _controllerStates[CLIMB_CONTROLLER_STATE];
		}
		else
		{
			currentControllerState = _controllerStates[DEFAULT_CONTROLLER_STATE];
		}
	}
}
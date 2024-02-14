using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
	public delegate void TriggerCollisionHandler(Collider collider);

	public TriggerCollisionHandler TriggerEnter;
	public TriggerCollisionHandler TriggerExit;

	public Vector3 velocity;

	private GameObject _currentClimbable;
	private CharacterController _characterController;

	public GameObject CurrentClimbable => _currentClimbable;

	[SerializeField]
	private PlayerInput _moveInfo;

	[SerializeField]
	private CharacterConfig _characterConfig;

	[Header("Camera")]
	[SerializeField]
	private CameraController _cameraController;

	public ControllerState controllerState;

	private void Start()
	{
		_characterController = GetComponent<CharacterController>();
		_characterController.radius = _characterConfig.Radius;
		_moveInfo = new PlayerInput();

		controllerState = new DefaultState(_moveInfo, this, _cameraController, _characterConfig, _characterController);

		SetupTriggerActions();
	}

	private void Update()
	{
		UpdateInputs();
		controllerState.EarlyUpdate();
		UpdateGravity();
		controllerState.Update();
		Friction();
		MoveCharacter();
		controllerState.LateUpdate();
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

	private void UpdateGravity()
	{
		var enableGravity = !_characterController.isGrounded && !_moveInfo.IsClimbing;
		if (enableGravity)
		{
			if (_characterController.velocity.y > 0)
			{
				if (_characterController.collisionFlags == CollisionFlags.Above)
				{
					velocity.y = -1f;
				}
			}
			velocity += Physics.gravity * Time.deltaTime;
		}
		_moveInfo.IsGrounded = _characterController.isGrounded;
	}

	private void Friction()
	{
		velocity.x = Mathf.Lerp(velocity.x, 0, _characterConfig.GroundFriction * Time.deltaTime);
		velocity.z = Mathf.Lerp(velocity.z, 0, _characterConfig.GroundFriction * Time.deltaTime);
	}

	private void MoveCharacter()
	{
		_characterController.Move(velocity * Time.deltaTime);

		if (_characterController.isGrounded)
		{
			velocity.y = 0;
		}
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
			controllerState = new ClimbState(controllerState);
		}
		else
		{
			controllerState = new DefaultState(controllerState);
		}
		
	}
}
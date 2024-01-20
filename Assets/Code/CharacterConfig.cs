using UnityEngine;

[CreateAssetMenu(menuName = "CharacterConfig/Character Config", fileName = "New Character Config")]
public class CharacterConfig : ScriptableObject
{
	[Header("External Forces")]
	[SerializeField]
	private float _airResistance;

	public float AirResistance => _airResistance;

	[SerializeField]
	[Range(0f, 10f)]
	private float _groundFriction;

	public float GroundFriction => _groundFriction;

	[SerializeField]
	private float _airAcceleration;

	public float AirAcceleration => _airAcceleration;

	[SerializeField]
	private float _groundAcceleration;

	public float GroundAcceleration => _groundAcceleration;

	[Header("Input Forces")]
	[SerializeField]
	private float _jumpImpulse;

	public float JumpImpulse => _jumpImpulse;

	[SerializeField]
	private float _maxSpeed;

	public float MaxSpeed => _maxSpeed;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float _forwardSpeed;

	public float ForwardSpeed => _forwardSpeed;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float _backwardSpeed;

	public float BackwardSpeed => _backwardSpeed;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float _sideSpeed;

	public float SideSpeed => _sideSpeed;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float _climbSpeed;

	public float ClimbSpeed => _climbSpeed;

	[SerializeField]
	[Range(0.1f, 2f)]
	private float _standHeight;

	public float StandHeight => _standHeight;

	[SerializeField]
	[Range(0.1f, 2f)]
	private float _crouchHeight;

	public float CrouchHeight => _crouchHeight;

	[SerializeField]
	private float _runSpeedMultiplier = 2f;

	public float RunSpeedMultiplier => _runSpeedMultiplier;

	[SerializeField]
	private float _crouchSpeedMultiplier = 0.8f;

	public float CrouchSpeedMultiplier => _crouchSpeedMultiplier;

	[SerializeField]
	private float _walkSpeedMultiplier = 1f;

	public float WalkSpeedMultiplier => _walkSpeedMultiplier;
}
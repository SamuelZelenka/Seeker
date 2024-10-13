using UnityEngine;

[CreateAssetMenu(menuName = "CharacterConfig/Character Config", fileName = "New Character Config")]
public class CharacterConfig : ScriptableObject
{
	[Header("External Forces")]
	[SerializeField]
	private float _airResistance;

	public float AirResistance => _airResistance;

	[SerializeField]
	private float _groundFriction;

	public float GroundFriction => _groundFriction;

	[Header("Acceleration")]
	[SerializeField]
	private float _airAcceleration;

	public float AirAcceleration => _airAcceleration;

	[SerializeField]
	private float _groundAcceleration;

	public float GroundAcceleration => _groundAcceleration;

	[Header("Jump")]
	[SerializeField]
	private float _jumpImpulse;

	public float JumpImpulse => _jumpImpulse;

	[SerializeField]
	private float _jumpGracePeriod;

	public float JumpGracePeriod => _jumpGracePeriod;

	[Header("Directional Multiplier")]
	[SerializeField]
	[Range(0.1f, 1f)]
	private float _forwardMultiplier;

	public float ForwardMultiplier => _forwardMultiplier;

	[SerializeField]
	[Range(0.1f, 1f)]
	private float _backwardMultiplier;

	public float BackwardMultiplier => _backwardMultiplier;

	[SerializeField]
	[Range(0.1f, 1f)]
	private float _sidewaysMultiplier;

	public float SidewaysMultiplier => _sidewaysMultiplier;

	[Header("Scaling")]
	[SerializeField]
	[Range(0.1f, 2)]
	private float radius;
	public float Radius => radius;

	[SerializeField]
	[Range(0.1f, 2f)]
	private float _standHeight;

	public float StandHeight => _standHeight;

	[SerializeField]
	[Range(0.1f, 2f)]
	private float _crouchHeight;

	public float CrouchHeight => _crouchHeight;

	[Header("Speed")]
	[SerializeField]
	private float _runSpeedMultiplier = 2f;

	public float RunSpeedMultiplier => _runSpeedMultiplier;

	[SerializeField]
	private float _crouchSpeedMultiplier = 0.8f;

	public float CrouchSpeedMultiplier => _crouchSpeedMultiplier;

	[SerializeField]
	private float _crouchTime;

	public float CrouchTime => _crouchTime;


	[SerializeField]
	private float _walkSpeedMultiplier = 1f;

	public float WalkSpeedMultiplier => _walkSpeedMultiplier;

	[Header("Climbing")]

	[SerializeField]
	private float _climbSpeedMultiplier = 1f;

	public float ClimbSpeedMultiplier => _climbSpeedMultiplier;

	[SerializeField]
	private AnimationCurve _vaultSpeed;

	[Range(0,180)]
	[SerializeField]
	private float _LedgeDetectionAngle;
	public float LedgeDetectionAngle => _LedgeDetectionAngle;

	[SerializeField]
	private float _ledgeDetectionRange;
	public float LedgeDetectionRange => _ledgeDetectionRange;

	public float VaultSpeed(float t) => _vaultSpeed.Evaluate(t);
}
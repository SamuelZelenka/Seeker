using UnityEngine;

[CreateAssetMenu(menuName = "CharacterMoveInfo/MoveInfo")]
public class MoveInfo : ScriptableObject
{
	// Inputs
	public bool IsClimbing { get; set; }
	public float HorizontalInput;
	public float VerticalInput;
	public float JumpInput;
	public float CrouchingInput;

	// States
	public bool IsGrounded;
	public bool IsCrouching;
    public bool IsRunning;

	// Values
	public float CrouchT;
}

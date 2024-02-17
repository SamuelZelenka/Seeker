using UnityEngine;

[CreateAssetMenu(menuName = "CharacterMoveInfo/MoveInfo")]
public class PlayerInput : ScriptableObject
{
	public bool IsClimbing { get; set; }
	public float HorizontalInput;
	public float VerticalInput;
	public float JumpInput;
	public float CrouchingInput;
	public bool IsGrounded;
	public bool IsRunning;
}

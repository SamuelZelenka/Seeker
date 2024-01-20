using UnityEngine;

public class CharacterMovementInfo
{
	public GameObject currentClimbable { get; set; }

	private CharacterController _controller;
	public CharacterMovementInfo(CharacterController controller)
	{
		_controller = controller;
	}

	public bool IsClimbing { get; set; }
	public float HorizontalInput => Input.GetAxis("Horizontal");
	public float VerticalInput => Input.GetAxis("Vertical");
	public float JumpInput => Input.GetAxis("Jump");
	public float Crouching => Input.GetAxis("Crouch");
	public bool IsGrounded => _controller.isGrounded;
	public bool IsRunning => Input.GetKey(KeyCode.LeftShift);
}

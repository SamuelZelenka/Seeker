using UnityEngine;

public partial class PlayerMovement
{
	private class InputData
	{
		public GameObject currentClimbable { get; set; }

		private CharacterController _controller;
		public InputData(CharacterController controller)
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
}
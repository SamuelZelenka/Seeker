using UnityEngine;

public class JumpAction : PlayerAction
{
	private CharacterMovementInfo _moveInfo;
	private PlayerController _playerController;
	private CharacterConfig _characterConfig;

	public JumpAction(
		CharacterMovementInfo moveInfo,
		PlayerController controller,
		CharacterConfig config)
	{
		_moveInfo = moveInfo;
		_playerController = controller;
		_characterConfig = config;
	}

	public override void Invoke()
	{
		var allowJump = _moveInfo.IsGrounded || _moveInfo.IsClimbing;

		if (_moveInfo.JumpInput > 0 && allowJump)
		{
			if (_moveInfo.IsClimbing)
			{
				var playerForward = _playerController.transform.forward;
				var climbablePosition = _playerController.CurrentClimbable.transform.position;
				var climbableDir = _playerController.transform.position - climbablePosition;
				var dot = Vector3.Dot(playerForward, climbableDir);
				var isFacingClimbable = dot < -0.3f;

				if (isFacingClimbable)
				{
					_playerController.velocity += -_playerController.transform.forward * _characterConfig.JumpImpulse * 0.15f;
				}
				else
				{
					_playerController.velocity += _playerController.transform.forward * _characterConfig.JumpImpulse * 0.15f;
				}
				return;
			}
			_playerController.velocity.y = _characterConfig.JumpImpulse;
		}
	}
}

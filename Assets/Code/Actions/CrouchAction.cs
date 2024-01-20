using UnityEngine;

public class CrouchAction : PlayerAction
{
	private CharacterController _characterController;
	private CharacterConfig _characterConfig;
	private CharacterMovementInfo _moveInfo;

	public CrouchAction(CharacterController controller, CharacterConfig config, CharacterMovementInfo moveInfo)
	{
		_characterController = controller;
		_characterConfig = config;
		_moveInfo = moveInfo;
	}

	public override void Invoke()
	{
		_characterController.height = Mathf.Lerp(_characterConfig.StandHeight, _characterConfig.CrouchHeight, _moveInfo.Crouching * Time.time);
	}
}
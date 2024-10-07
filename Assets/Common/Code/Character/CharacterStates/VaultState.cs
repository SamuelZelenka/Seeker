using UnityEngine;

public class VaultState : ControllerState
{
	private Vector3 _startPos;
	private Vector3 _targetPos;

	private float _currentTrajectory;

	public VaultState(ControllerStateArgs args) : base(args)
	{
		Setup();
	}

	private void Setup()
	{
		earlyActions.Add(Move);
		actions.Add(Jump);
		lateActions.Add(MoveCharacter);
	}

	public void Start(Vector3 targetPos)
	{
		_startPos = playerController.transform.position;
		_targetPos = targetPos;
		_currentTrajectory = 0;
	}

	public override float GetSpeedMultiplier(PlayerInput moveInfo, CharacterConfig CharConfig)
	{
		return CharConfig.ClimbSpeedMultiplier;
	}

	public override void Move()
	{
		if (_currentTrajectory < 1) // TODO MAKE THE TIMING MAKE SENSE FOR INSPECTOR
		{
			characterController.enabled = false;
			var nextPos = Vector3.Lerp(_startPos, _targetPos, _currentTrajectory);
			playerController.transform.position = nextPos;
			_currentTrajectory += Time.deltaTime * characterConfig.VaultSpeed(_currentTrajectory);    // TODO MAKE THE TIMING MAKE SENSE FOR INSPECTOR
		}
		else
		{
			characterController.enabled = true;
			playerController.SetControllerState<DefaultState>();
		}
	}

	public override void Jump()
	{
	}
}
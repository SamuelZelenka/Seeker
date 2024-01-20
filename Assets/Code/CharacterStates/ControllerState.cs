using System.Collections.Generic;

public abstract class ControllerState
{
	protected List<PlayerAction> _earlyActions = new();
	protected List<PlayerAction> _actions = new();
	protected List<PlayerAction> _lateActions = new();

	public virtual void EarlyUpdate() => _earlyActions.ForEach((a) => a.Invoke());
	public virtual void Update() => _actions.ForEach((a) => a.Invoke());
	public virtual void LateUpdate() => _lateActions.ForEach((a) => a.Invoke());

	private void UpdateCollection(PlayerAction action)
	{
		action.Invoke();
	}
	public abstract float GetSpeedMultiplier(CharacterMovementInfo moveInfo, CharacterConfig CharConfig);
}

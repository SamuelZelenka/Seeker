using UnityEngine;


public abstract class Skill : ScriptableObject
{
	[SerializeField]
	protected int id;
	[SerializeField]
	protected Sprite icon;
	[SerializeField]
	protected string displayName;
	public virtual int GetID() => id;
	public virtual string GetDisplayName() => displayName;
	public virtual Sprite GetIcon() => icon;
	public abstract void Perform();
}

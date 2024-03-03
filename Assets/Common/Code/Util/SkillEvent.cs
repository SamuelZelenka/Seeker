using System;
using UnityEngine;
[CreateAssetMenu(menuName = "Events/Skill")]
public class SkillEvent : ScriptableObject
{
	private Action<Skill> _event;

	public void Subscribe(Action<Skill> func)
	{
		_event += func;
	}

	public void Unsubscribe(Action<Skill> func)
	{
		_event -= func;
	}

	public void Invoke(Skill skill)
	{
		_event?.Invoke(skill);
	}
}

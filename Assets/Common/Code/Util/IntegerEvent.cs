using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Integer")]
public class IntegerEvent : ScriptableObject
{
	private Action<int> _event;

	public void Subscribe(Action<int> func)
	{
		_event += func;
	}

	public void Unsubscribe(Action<int> func)
	{
		_event -= func;
	}

	public void Invoke(int id)
	{
		_event?.Invoke(id);
	}
}

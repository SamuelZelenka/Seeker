using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositoryManager : MonoBehaviour
{
	private static RepositoryManager _instance;

	public static RepositoryManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<RepositoryManager>();

				if (_instance == null)
				{
					GameObject singletonObject = new GameObject(typeof(RepositoryManager).Name);
					_instance = singletonObject.AddComponent<RepositoryManager>();
				}
			}
			return _instance;
		}
	}

	[SerializeField]
	private SkillRepository _skillRepository;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
}
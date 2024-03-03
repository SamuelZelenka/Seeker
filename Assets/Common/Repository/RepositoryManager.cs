using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositoryManager : MonoBehaviour
{
	[SerializeField]
	private SkillRepository _skillRepository;
	public static SkillRepository SkillRepository => _instance._skillRepository;



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
					GameObject singletonObject = new GameObject("RepositoryManager");
					_instance = singletonObject.AddComponent<RepositoryManager>();
				}
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
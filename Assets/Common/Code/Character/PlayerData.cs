using Sirenix.OdinInspector;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : SerializedScriptableObject
{
	public const string ENCRYPT_KEY = "SuperGoodEncryptionKey";

	[SerializeField]
	private PlayerDataContainer data;

	public string[] availableSkills => data.availableSkills;
	public string[] activeSkills => data.activeSkills;

	public void Save()
	{
		SaveSystem.SaveData(ENCRYPT_KEY, "PlayerDataTest", data);
	}

	public void Load()
	{
		data = SaveSystem.LoadData<PlayerDataContainer>(ENCRYPT_KEY, "PlayerDataTest");
	}
}

[Serializable]
public class PlayerDataContainer
{
	[SerializeField]
	public string[] availableSkills;
	[SerializeField]
	public string[] activeSkills = new string[3];
}

using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
	[SerializeField]
	public string[] availableSkills;
	[SerializeField]
	public string[] activeSkills = new string[3];
}

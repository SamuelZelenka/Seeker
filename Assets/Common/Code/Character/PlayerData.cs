using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
	[SerializeField]
	private Skillset skillset;

	public Skill GetSkill(int id)
	{
		return skillset.GetSkill(id);
	}
}

using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Repository/SkillRepository")]
public class SkillRepository : SerializedScriptableObject
{
	[OdinSerialize]
	[ShowInInspector]
	private Dictionary<string, Skill> _skills;

	public Skill GetSkill(string id) => _skills[id];
}

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/SkillRepository")]
public class SkillRepository : ScriptableObject
{
	[ShowInInspector]
	public Dictionary<int,Skill> skills = new Dictionary<int, Skill>();
}
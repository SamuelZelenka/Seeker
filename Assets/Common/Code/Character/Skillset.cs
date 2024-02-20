using System.Collections.Generic;

public class Skillset
{
	public Skillset(SkillRepository repo, params int[] skillIds)
	{
		foreach (var id in skillIds)
		{
			var skill = repo.skills[id];
			skills.Add(id, skill);
		}
	}

	private Dictionary<int, Skill> skills = new Dictionary<int, Skill>();

	public Skill GetSkill(int id) => skills[0];
}

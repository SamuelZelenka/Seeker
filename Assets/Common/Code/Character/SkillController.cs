public class SkillController
{
	private SkillEvent _onNextSkill;
	private SkillEvent _onPreviousSkill;
	private SkillEvent _onSkillUsed;
	private SkillEvent _onSkillChanged;

	private int _activeSkillIndex;
	private PlayerData _playerData;

	public SkillController(PlayerData playerData, SkillEvent onNextSkill, SkillEvent onPreviousSkill, SkillEvent onSkillUsed, SkillEvent onSkillChanged)
	{
		_playerData = playerData;
		_onNextSkill = onNextSkill;
		_onPreviousSkill = onPreviousSkill;
		_onSkillUsed = onSkillUsed;
		_onSkillChanged = onSkillChanged;
	}

	public int GetActiveSkillsCount()
	{
		return _playerData.activeSkills.Length;
	}

	public int GetActiveSkillIndex()
	{
		return _activeSkillIndex;
	}

	public Skill GetSkillByIndex(int index)
	{
		return RepositoryManager.SkillRepository.GetSkill(_playerData.activeSkills[index]);
	}

	public Skill GetActiveSkill()
	{
		return GetSkillByIndex(_activeSkillIndex);
	}

	public Skill GetNextSkill()
	{
		var nextSkillIndex = MathExtra.WrapModulo(_activeSkillIndex + 1, _playerData.activeSkills.Length);

		return RepositoryManager.SkillRepository.GetSkill(_playerData.activeSkills[nextSkillIndex]);
	}

	public Skill GetPreviousSkill()
	{
		var previousSkillIndex = MathExtra.WrapModulo(_activeSkillIndex - 1, _playerData.activeSkills.Length);

		return RepositoryManager.SkillRepository.GetSkill(_playerData.activeSkills[previousSkillIndex]);
	}

	public void NextSkill()
	{
		_activeSkillIndex = MathExtra.WrapModulo(_activeSkillIndex + 1, _playerData.activeSkills.Length);
		_onNextSkill?.Invoke(GetActiveSkill());
		_onSkillChanged?.Invoke(GetActiveSkill());
	}

	public void PreviousSkill()
	{
		_activeSkillIndex = MathExtra.WrapModulo(_activeSkillIndex - 1, _playerData.activeSkills.Length);
		_onPreviousSkill?.Invoke(GetActiveSkill());
		_onSkillChanged?.Invoke(GetActiveSkill());
	}

	public void PerformActiveSkill()
	{
		GetActiveSkill().Perform();
		_onSkillUsed.Invoke(GetActiveSkill());
	}
}
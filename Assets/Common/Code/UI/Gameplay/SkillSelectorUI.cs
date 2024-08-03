using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectorUI : MonoBehaviour
{
	[SerializeField]
	private float animationDuration = 1f;

	[SerializeField]

	private Image _previousImage;

	[SerializeField]
	private Image _nextImage;

	[SerializeField]
	private Image[] _icons = new Image[3];

	[SerializeField]
	private AnimationCurve _animationCurve;

	[SerializeField]
	private SkillEvent _onNextSkill;
	[SerializeField]
	private SkillEvent _onPreviousSkill;
	[SerializeField]
	private SkillEvent _onSkillChanged;

	[SerializeField]
	private PlayerController _playerController;

	private IconTransform _selectedIconTransform;
	private IconTransform _nextIconTransform;
	private IconTransform _previousTransform;


	private Vector2 SMALL_SIZE = new Vector2(32, 32);
	private Vector2 BIG_SIZE = new Vector2(96, 96);

	private void Start()
	{

		var skillController = _playerController.SkillController;
		_icons[2].sprite = skillController.GetSkillByIndex(skillController.GetActiveSkillIndex()).GetIcon();

		var nextSkillIndex = MathExtra.WrapModulo(skillController.GetActiveSkillIndex() + 1, skillController.GetActiveSkillsCount());
		var nextSkillSprite = skillController.GetSkillByIndex(nextSkillIndex).GetIcon();

		var previousSkillIndex = MathExtra.WrapModulo(skillController.GetActiveSkillIndex() - 1, skillController.GetActiveSkillsCount());
		var previousSkillSprite = skillController.GetSkillByIndex(previousSkillIndex).GetIcon();

		_icons[0].sprite = nextSkillSprite;
		_icons[1].sprite = previousSkillSprite;

		_selectedIconTransform = new IconTransform(_icons[0].transform.position, 96);
		_nextIconTransform = new IconTransform(_icons[1].transform.position, 32);
		_previousTransform = new IconTransform(_icons[2].transform.position, 32);

		_onNextSkill.Subscribe(OnNextSkill);
		_onPreviousSkill.Subscribe(OnPreviousSkill);
		//_onSkillChanged.Subscribe(OnSkillChanged);

		StartCoroutine("AnimateIcons");
	}

	private void OnNextSkill(Skill skill)
	{
		(_icons[0], _icons[1], _icons[2]) = (_icons[1], _icons[2], _icons[0]);

		StartCoroutine("AnimateColor", _nextImage);
		StartCoroutine("AnimateIcons");
	}

	private void OnPreviousSkill(Skill skill)
	{
		(_icons[0], _icons[1], _icons[2]) = (_icons[2], _icons[0], _icons[1]);

		StartCoroutine("AnimateColor", _previousImage);
		StartCoroutine("AnimateIcons");
	}

	private IEnumerator AnimateColor(Image image)
	{
		image.color = new Color(0.2f, 0, 0);

		while (image.color.g < 1)
		{
			float delta = Time.deltaTime;
			image.color = new Color(image.color.r + delta, image.color.g + delta, image.color.b + delta);

			yield return null; 
		}

		image.color = Color.white;
	}

	IEnumerator AnimateIcons()
	{
		Vector2 icon1StartPos = _icons[0].transform.position;
		Vector2 icon2StartPos = _icons[1].transform.position;
		Vector2 icon3StartPos = _icons[2].transform.position;


		float animationTime = 0f;
		while (animationTime < animationDuration)
		{
			var t = animationTime / animationDuration;
			var tCurve = _animationCurve.Evaluate(t);

			_icons[2].rectTransform.sizeDelta = Vector2.Lerp(SMALL_SIZE, BIG_SIZE, tCurve);
			_icons[1].rectTransform.sizeDelta = Vector2.Lerp(SMALL_SIZE, SMALL_SIZE, tCurve);
			_icons[0].rectTransform.sizeDelta = Vector2.Lerp(SMALL_SIZE, SMALL_SIZE, tCurve);

			_icons[0].transform.position = Vector2.LerpUnclamped(icon1StartPos, _nextIconTransform.pos, tCurve);
			_icons[1].transform.position = Vector2.LerpUnclamped(icon2StartPos, _previousTransform.pos, tCurve);
			_icons[2].transform.position = Vector2.LerpUnclamped(icon3StartPos, _selectedIconTransform.pos, tCurve);

			animationTime += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}

		_icons[0].transform.position = _nextIconTransform.pos;
		_icons[1].transform.position = _previousTransform.pos;
		_icons[2].transform.position = _selectedIconTransform.pos;

		_icons[2].rectTransform.sizeDelta = BIG_SIZE;
		_icons[1].rectTransform.sizeDelta = SMALL_SIZE;
		_icons[0].rectTransform.sizeDelta = SMALL_SIZE;
	}

	private struct IconTransform
	{
		public Vector2 pos;
		public Vector2 size;

		public IconTransform(Vector2 pos, float size)
		{
			this.pos = pos;
			this.size = new Vector2(size, size);
		}
	}
}

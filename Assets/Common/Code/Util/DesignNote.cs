#if UNITY_EDITOR
using UnityEngine;

/// <summary>
/// DesignNotes
/// </summary>
public class DesignNote : MonoBehaviour
{
	public Texture2D icon;

	public string description;
	[TextArea]
	[SerializeField]
	private string _note = "Add Design Notes here.";
}
#endif

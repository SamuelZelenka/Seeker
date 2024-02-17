using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// DesignNotes
/// </summary>
public class DesignNote : MonoBehaviour
{
	[TextArea]
	[SerializeField]
	private string note = "Add Design Notes here.";
}
#endif
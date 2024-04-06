#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DesignNoteEditor : EditorWindow
{
	private const float NOTE_SIZE = 100f;
	private const float DESCRIPTION_OFFSET = NOTE_SIZE/2 + 5;
	private const float DESCRIPTION_SIZE = NOTE_SIZE * 4;
	private static bool showIcons = false;

	static DesignNoteEditor()
	{
		SceneView.duringSceneGui += OnSceneGUI;
	}

	private static void OnSceneGUI(SceneView sceneView)
	{
		if (showIcons)
		{
			foreach (var note in FindObjectsOfType<DesignNote>())
			{
				Vector3 screenPoint = Camera.current.WorldToScreenPoint(note.transform.position);

				Handles.BeginGUI();

				Vector2 descriptionPosition = new Vector2(
					screenPoint.x - NOTE_SIZE * 0.5f,
					Screen.height - screenPoint.y - NOTE_SIZE * 0.5f);

				GUI.Label(new Rect(descriptionPosition.x, descriptionPosition.y, NOTE_SIZE, NOTE_SIZE), note.icon);

				GUI.Label(new Rect(descriptionPosition.x, descriptionPosition.y + DESCRIPTION_OFFSET, DESCRIPTION_SIZE, NOTE_SIZE), note.description);

				Handles.EndGUI();
			}
		}
	}

	[MenuItem("Seeker/Show Design Notes")]
	private static void ToggleMenuItem()
	{
		showIcons = !showIcons;
	}

	[MenuItem("Seeker/Show Design Notes", true)]
	private static bool ToggleMenuItemValidate()
	{
		Menu.SetChecked("Seeker/Show Design Notes", showIcons);
		return true;
	}
}
#endif

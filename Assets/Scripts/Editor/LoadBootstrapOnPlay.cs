using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
	[InitializeOnLoad]
	public static class LoadBootstrapOnPlay
	{
		private const string EditorPrefsKey = "PreviousScenePath";
		private const string BootstrapScenePath = "Assets/Scenes/Bootstrap.unity";

		static LoadBootstrapOnPlay()
		{
			EditorApplication.playModeStateChanged += OnPlayModeChanged;
		}

		private static void OnPlayModeChanged(PlayModeStateChange playModeStateChange)
		{
			switch (playModeStateChange)
			{
				case PlayModeStateChange.ExitingEditMode when EditorPrefs.GetBool("StartFromBootstrapScene"):
					
					string currentScene = EditorSceneManager.GetActiveScene().path;
					EditorPrefs.SetString(EditorPrefsKey, currentScene);
					
					if (!string.IsNullOrEmpty(BootstrapScenePath))
					{
						EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
						EditorSceneManager.OpenScene(BootstrapScenePath);
					}
					
					break;
				
				case PlayModeStateChange.EnteredEditMode when EditorPrefs.GetBool("StartFromBootstrapScene"):
					
					if (EditorPrefs.HasKey(EditorPrefsKey))
					{
						string previousScene = EditorPrefs.GetString(EditorPrefsKey);
						if (!string.IsNullOrEmpty(previousScene))
						{
							EditorSceneManager.OpenScene(previousScene);
							EditorPrefs.DeleteKey(EditorPrefsKey);
						}
					}
					
					break;
			}
		}
	}
}

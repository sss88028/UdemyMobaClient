using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartHelper
{
	#region public-method
	[MenuItem("Tools/GameTool %`")]
	public static void Start() 
	{
		if (EditorBuildSettings.scenes.Length > 0) 
		{
			EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
			EditorApplication.isPlaying = true;
		}
	}
	#endregion public-method
}

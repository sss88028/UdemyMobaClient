using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestLoginUI))]
public class TestLoginUIEditor : Editor
{
	#region private-field
	private TestLoginUI _target;
	#endregion private-field

	#region public-method
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (Application.isPlaying)
		{
			if (GUILayout.Button("Open"))
			{
				_target.Open();
			}
			else if (GUILayout.Button("Close"))
			{
				_target.Close();
			}
		}
	}
	#endregion public-method

	#region MonoBehaviour-method
	private void OnEnable()
	{
		_target = (TestLoginUI)target;
	}
	#endregion MonoBehaviour-method
}

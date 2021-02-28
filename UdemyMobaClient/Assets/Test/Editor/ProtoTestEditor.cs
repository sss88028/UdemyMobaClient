using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProtoTest))]
public class ProtoTestEditor : Editor
{
	#region private-field
	private ProtoTest _target;
	#endregion private-field

	#region Editor-method
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Send Login"))
		{
			_target.SendLogin();
		}
	}

	private void OnEnable()
	{
		_target = (ProtoTest)target;
	}
	#endregion Editor-method
}

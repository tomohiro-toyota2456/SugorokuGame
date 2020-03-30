using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ToolDatabase))]
public class ToolDatabaseInspector : Editor 
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("データセット"))
		{
			DatabaseInspector.Set<ToolData>("Assets/SceneData/Game/ToolData", target);
		}

		if(GUILayout.Button(("TEST")))
		{
			var t = (ToolDatabase)target;
			Debug.Log(t.SearchFromId(0).Name);
		}
	}
}

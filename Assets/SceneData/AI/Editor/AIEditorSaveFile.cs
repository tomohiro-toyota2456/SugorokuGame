using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブデータ用ファイル
/// </summary>
public class AIEditorSaveFile : ScriptableObject
{
	[SerializeReference]
	public List<IAIAction> actions;

	public List<Rect> rects;

	public Rect SearchRect(IAIAction action)
	{
		for (int i = 0; i < actions.Count; i++)
		{
			if (action == actions[i])
			{
				return rects[i];
			}
		}
		Debug.LogError("Missing Rect");
		return new Rect(0, 0, 100, 100);
	}

}

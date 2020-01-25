using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviourのシングルトンクラス
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour 
{
	static T instance;

	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<T>();
			}

			return instance;
		}
	}

	public void Awake()
	{
		var t = this.GetComponent<T>();
		if(t != null)
		{
			instance = t;
		}
	}
}

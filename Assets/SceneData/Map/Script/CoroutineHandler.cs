using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MonoBehaviorを持たないクラスでコルーチンを呼び出す
public class CoroutineHandler : MonoBehaviour
{
	static CoroutineHandler instance;

	public static CoroutineHandler Instance
	{
		get
		{
			if(instance == null)
			{
				instance = new GameObject("CoroutineHandler").AddComponent<CoroutineHandler>();
			}

			return instance;
		}
	}

	public Coroutine StartCoroutineFromHandler(IEnumerator enumerator)
	{
		return StartCoroutine(enumerator);
	}
}

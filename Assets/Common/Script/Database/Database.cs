using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tはデータの型 Vは返すときに変換したい型 Jは自分のクラスを入れる
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="V"></typeparam>
/// <typeparam name="J"></typeparam>
public class Database<T,V,J> : MonoBehaviourSingleton<J> where  T : ScriptableObject,IDatabaseReader where V : class where J : MonoBehaviour
{
	[SerializeField]
	protected T[] data;


	public  V SearchFromId(int id)
	{
		foreach(var d in data)
		{
			if(id == d.Id)
			{
				return d as V;
			}
		}

		return default;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour,ISquareAccess
{
	[SerializeField]
	int id;
	[SerializeField]
	int[] accessIds;
	[SerializeField]
	SquareData squareData;

	public int Id { get { return id; } }
	public int[] AccessIds { get { return accessIds; } }//接続情報
	public int AccessIdSum { get { return accessIds.Length; } }
	public SquareData SquareData { get { return squareData; } }

	public int GetAccessId(int idx)
	{
		if(idx < 0 || idx >= accessIds.Length)
		{
			return -1;
		}

		return accessIds[idx];
	}
}

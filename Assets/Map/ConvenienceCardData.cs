using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvenienceCardData : ScriptableObject,IConvenienceCardDataReader
{
	[SerializeField]
	int id;
	[SerializeField]
	EffectType type;
	[SerializeField]
	int param;

	public enum EffectType
	{
		AdditionDices,//ダイス増加
		FixedMoving,//固定進行形
	}

	public int Id { get { return id; } set { id = value; } }
	public EffectType Type { get { return type; } set { type = value; } }
	public int Param { get { return param; } set { param = value; } }
}

public interface IConvenienceCardDataReader
{
	int Id { get; }
	ConvenienceCardData.EffectType Type { get; }
	int Param { get; }
}

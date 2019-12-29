using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvenienceCardData : ScriptableObject,IConvenienceCardDataReader
{
	[SerializeField]
	int id;
	[SerializeField]
	EffectKind kind;
	[SerializeField]
	EffectType type;
	[SerializeField]
	int param;//効果値
	[SerializeField]
	int priority;//効果優先度

	/// <summary>
	/// 効果の大項目　ダイスに関係とかどこに影響がでるのか
	/// </summary>
	public enum EffectKind
	{
		Dice,//ダイスに関係するもの
	}

	/// <summary>
	/// 効果の詳細
	/// </summary>
	public enum EffectType
	{
		AdditionDices,//ダイス増加
		FixedMoving,//固定進行形
	}

	public int Id { get { return id; } set { id = value; } }
	public EffectType Type { get { return type; } set { type = value; } }
	public EffectKind Kind { get { return kind; } set { kind = value; } }
	public int Param { get { return param; } set { param = value; } }
	public int Priority { get { return priority; } set { priority = value; } }
}

public interface IConvenienceCardDataReader
{
	int Id { get; }
	ConvenienceCardData.EffectType Type { get; }
	ConvenienceCardData.EffectKind Kind { get; }
	int Param { get; }
	int Priority { get; }
}

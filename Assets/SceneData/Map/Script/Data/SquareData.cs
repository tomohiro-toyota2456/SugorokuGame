using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SquareData", menuName = "CreateSquareData", order = 100)]
public class SquareData : ScriptableObject
{
	[SerializeField]
	EffectType type;
	[SerializeField]
	int level;//効果値を直接決定すると用意するデータ数が増えるので基準値からレベルで決めることでどの進行度でもデータを使える
	[SerializeField]
	int eventId;//Eventの場合のみ使います Shop等もイベント扱いにしたい

	public enum EffectType
	{
		NoEffect,//効果なし
		PlusMoney,//
		MinusMoney,//
		Event,//イベント
	}

	public int Level { get { return level; } }
	public int EventId { get { return eventId; } }
	public EffectType Type { get { return type; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BattleCardData",menuName ="CreateBattleCard",order = 100)]
public class BattleCardData : ScriptableObject,IBattleCardDataReader
{
	[SerializeField]
	int id;
	[SerializeField]
	int param;
	[SerializeField]
	bool isAtk;

	public int Id { get { return id; } set { id = value; } }
	public int Param { get { return param; } set { param = value; } }
	public bool IsAtk { get { return isAtk; } set { isAtk = value; } }
}

public interface IBattleCardDataReader
{
	int Id { get; }
	int Param { get; }
	bool IsAtk { get; }
}

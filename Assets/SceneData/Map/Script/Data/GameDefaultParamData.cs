using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDefaultParamData",menuName ="CreateDefaultParamData",order = 100)]
public class GameDefaultParamData : ScriptableObject
{
	[SerializeField]
	int maxHp;
	[SerializeField]
	int money;
	[SerializeField]
	int initPosId;

	public int MaxHp { get { return maxHp; } }
	public int Money { get { return money; } }
	public int InitPosId { get { return initPosId; } }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCalc
{
	public void Calc(CommandData commandData,GameDataManager.GamePlayerData[] players)
	{

	}

	public static int Dice(int sumDices)
	{
		return Random.Range(sumDices, 6 * sumDices + 1);
	}

}

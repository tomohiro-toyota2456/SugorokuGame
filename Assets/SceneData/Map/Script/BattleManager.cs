using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	public IEnumerator WaitBattle(int atkPlayerId, int defPlayerId)
	{
		Coroutine c = null;
		//yield return StartCoroutine(c);
		yield return null;
		//c.Current;
	}
}

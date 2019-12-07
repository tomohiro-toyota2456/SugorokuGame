using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	int curIdx = 0;

	List<ICommandController> commandControllerList = new List<ICommandController>();

	public void AddController(ICommandController controller)
	{
		commandControllerList.Add(controller);
	}

	public IEnumerator WaitCommand(IGameDataReader gameDataReader,GameUIController gameUIController,Map map)
	{
		IEnumerator c = commandControllerList[curIdx].WaitCommand(gameDataReader,gameUIController,map);
		yield return StartCoroutine(c);
		yield return c.Current;
	}

}

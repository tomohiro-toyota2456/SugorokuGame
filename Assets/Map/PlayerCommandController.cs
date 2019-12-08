using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommandController : ICommandController
{
	int actorId;
	public void Init(int actorId)
	{
		this.actorId = actorId;
	}

	public IEnumerator WaitCommand(IGameDataReader gameDataReader, GameUIController gameUIController,Map map)
	{
		int sumDices = 1;//gameDataGetterからしゅとく
		int numDice = GameCalc.Dice(sumDices);
		int curIdx = gameDataReader.ReadPlayerPositionId(actorId);
		gameUIController.Init(sumDices, numDice);

		Debug.Log("Dice:" + numDice);

		IEnumerator enumerator;

		yield return gameUIController.WaitCommandCompleteCoroutine(out enumerator);
		int usingCardId = (int)enumerator.Current;

		Debug.Log("usingCardId:" + usingCardId);

		if(usingCardId == -1)
		{
			var root = map.CalcEnableRoot(numDice, curIdx);

			bool isLoop = true;
			int areaId = 0;

			yield return new WaitForSeconds(0.5f);

			gameUIController.ChangeCameraMode(false);
			while (isLoop)
			{
				//ダイス
				yield return map.SelectSquareCoroutine(out enumerator, root);
				areaId = (int)enumerator.Current;
				isLoop = false;
			}
			gameUIController.ChangeCameraFixedMode(false);

			CommandData commandData = new CommandData();
			commandData.actorId = actorId;
			commandData.commandType = CommandType.Move;
			commandData.id = areaId;
			commandData.num = CalcRoot(root[areaId]);
			yield return commandData;
		}
		else
		{
			//カード

		}

	}

	public int[] CalcRoot(Tree<int> root)
	{
		List<int> ans = new List<int>();
		var parent = root.Parent;
		ans.Add(root.Data);
		while(parent != null)
		{
			ans.Insert(0, parent.Data);
			parent = parent.Parent;
		}

		return ans.ToArray();
	}
}

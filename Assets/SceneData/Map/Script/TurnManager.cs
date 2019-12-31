using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

	public int CurIdx { get; private set; } = 0;
	public bool IsIdxMoved { get; private set; } = true;//プレイヤーの操作が移動するような行動を行っていたか

	List<ICommandController> commandControllerList = new List<ICommandController>();
	/// <summary>
	/// コントローラーを追加
	/// </summary>
	/// <param name="controller"></param>
	public void AddController(ICommandController controller)
	{
		commandControllerList.Add(controller);
	}

	/// <summary>
	/// コントローラーを削除
	/// </summary>
	/// <param name="id"></param>
	public void RemoveController(int id)
	{
		foreach(var controller in commandControllerList)
		{
			if(controller.Id == id)
			{
				commandControllerList.Remove(controller);
				break;
			}
		}

		//エネミーが死亡するなどして上限を超えた場合は０に戻す
		if(CurIdx >= commandControllerList.Count)
		{
			CurIdx = 0;
		}
	}

	public IEnumerator WaitCommand(IGameDataReader gameDataReader,GameUIController gameUIController,Map map)
	{
		IEnumerator c = commandControllerList[CurIdx].WaitCommand(gameDataReader,gameUIController,map);
		yield return StartCoroutine(c);

		var command = (CommandData)c.Current;
		//ターンが動くのはダイスを振って移動している場合、もしくは死亡ぺナ等で動けない場合
		if(command.commandType == CommandType.Move || command.commandType == CommandType.None)
		{
			CurIdx++;
			if(CurIdx >= commandControllerList.Count)
			{
				CurIdx = 0;
			}
			IsIdxMoved = true;
		}
		else
		{
			IsIdxMoved = false;
		}

		yield return c.Current;
	}

	/// <summary>
	/// 現在のIdを取得
	/// </summary>
	/// <returns></returns>
	public int GetCurrentTurnId()
	{
		return commandControllerList[CurIdx].Id;
	}
}

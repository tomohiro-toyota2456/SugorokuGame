using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	GameDataManager gameDataManager;
	[SerializeField]
	TurnManager turnManager;
	[SerializeField]
	GraphicManager graphicManager;
	[SerializeField]
	GameUIController gameUIController;
	[SerializeField]
	EventManager eventManager;
	[SerializeField]
	BattleManager battleManager;
	[SerializeField]
	Map test;

	public static int StageId { private get; set; }

	private void Start()
	{
		int playerSum = 1;
		gameDataManager.Init(playerSum, 0);

		PlayerCommandController playerController = new PlayerCommandController();
		playerController.Init(0);
		turnManager.AddController(playerController);
		graphicManager.AddFigure(null, test.GetSquare(0).transform.position);

		for(int i = 0; i < playerSum-1;i++)
		{ 

		}

		StartCoroutine(UpdateMain());
	}

	IEnumerator UpdateMain()
	{
		while(true)
		{
			Debug.Log("CurPos" + gameDataManager.ReadPlayerPositionId(0));
			//コマンド待ち
			IEnumerator c = turnManager.WaitCommand(gameDataManager,gameUIController,test);
			yield return StartCoroutine(c);
			CommandData commandData = (CommandData)c.Current;

			//パラメータ更新
			int eventId = -1;
			GameCalc.CalcPlayerCommand(commandData,gameDataManager.GamePlayers, test, out eventId);

			//グラフィック更新
			yield return StartCoroutine(graphicManager.UpdateGraphic(gameDataManager,test,commandData));

			//イベント更新
			if (eventId != -1)
			{
				c = eventManager.WaitEvent(eventId, commandData.actorId);
				yield return StartCoroutine(c);
				var eventResultData = (EventResultData)c.Current;

				//パラメータ更新
				GameCalc.CalcEventEffect(eventResultData, gameDataManager.GamePlayers);

				//グラフィック更新
			}

			//バトルチェック
			if (commandData.commandType == CommandType.Move)
			{
				int[] targets = null;
				if (gameDataManager.CheckSamePositionPlayer(commandData.id, commandData.actorId, out targets))
				{
					for(int i = 0; i < targets.Length; i++)
					{
						c = battleManager.WaitBattle(commandData.actorId, targets[i]);
						yield return StartCoroutine(c);
						//パラメータ更新


						//グラフィック更新

					}
				}
			}


			//グラフィック更新
		}
	}

}

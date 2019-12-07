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
			//仮
			gameDataManager.SetPlayerPosition(commandData.actorId, commandData.id);

			//グラフィック更新
			yield return StartCoroutine(graphicManager.UpdateGraphic(gameDataManager,test,commandData));
			//イベント・バトル更新

			//パラメータ更新

			//グラフィック更新
		}
	}

}

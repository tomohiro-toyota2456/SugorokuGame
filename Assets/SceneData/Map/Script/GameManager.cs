using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ゲーム本編のメインループ等管理
/// </summary>
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
	Camera mapCamera;
	[SerializeField]
	Map test;//本来はなんかしらのルールで読み込む

	public static int StageId { private get; set; }
	int turnNum = 0;

	Map usingMap;

	private void Start()
	{
		CreateMap();

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
			//ターン表示
			if(CheckTurnChanged())
			{
				turnNum++;
				yield return StartCoroutine(graphicManager.PlayTurnAnimation(turnNum));
			}

			//誰のターンかの表示
			if(CheckChangedPlayer())
			{
				yield return StartCoroutine(graphicManager.PlayTurnAnimation("player:" + turnManager.CurIdx));
			}

			//コマンド待ち
			IEnumerator c = turnManager.WaitCommand(gameDataManager,gameUIController,usingMap);
			yield return StartCoroutine(c);
			CommandData commandData = (CommandData)c.Current;

			//パラメータ更新
			int eventId = -1;
			GameCalc.CalcPlayerCommand(commandData,gameDataManager.GamePlayers, usingMap, out eventId);

			//グラフィック更新
			yield return StartCoroutine(graphicManager.UpdateGraphic(gameDataManager,usingMap,commandData));

			//イベント更新
			if (eventId != -1)
			{
				c = eventManager.WaitEvent(eventId, commandData.actorId);
				yield return StartCoroutine(c);
				var eventResultData = (EventResultData)c.Current;

				//パラメータ更新
				GameCalc.CalcEventEffect(eventResultData, gameDataManager.GamePlayers);

				//グラフィック更新
				StartCoroutine(graphicManager.UpdateUI(gameDataManager));
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
						StartCoroutine(graphicManager.UpdateUI(gameDataManager));
					}
				}
			}


			//グラフィック更新

			
		}
	}

	bool CheckTurnChanged()
	{
		return turnManager.IsIdxMoved && turnManager.CurIdx == 0;
	}

	bool CheckChangedPlayer()
	{
		return turnManager.IsIdxMoved;
	}

	void CreateMap()
	{
		var ins = Instantiate(test);
		ins.transform.localPosition = Vector3.zero;
		ins.transform.localScale = Vector3.one;
		ins.MapCamera = mapCamera;
		usingMap = ins;
		ins.Setup();
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ゲームコア部分で使う計算処理
/// </summary>
public static class GameCalc
{
	/// <summary>
	/// プレイヤーの行動の計算処理を行う
	/// マスでイベントがある場合のみeventIdに値が入る
	/// </summary>
	/// <param name="commandData"></param>
	/// <param name="players"></param>
	/// <param name="map"></param>
	/// <param name="eventId"></param>
	public static void CalcPlayerCommand(CommandData commandData,List<GameDataManager.GamePlayerData> players,Map map,out int eventId)
	{
		eventId = -1;
		switch(commandData.commandType)
		{
			case CommandType.Move:
				//プレイヤー位置更新
				players[commandData.actorId].SetPosition(commandData.id);
				//マスのイベント処理
				eventId = CalcSquareEffect(map.GetSquare(commandData.id).SquareData, players[commandData.actorId]);
				break;
			case CommandType.UsingCard:

				break;
		}
	}

	public static void CalcEventEffect(EventResultData eventResultData,List<GameDataManager.GamePlayerData> players)
	{

	}

	static int CalcSquareEffect(SquareData squareData,GameDataManager.GamePlayerData player)
	{
		//Player以外は無駄なのでスルー
		if(squareData == null || player.type > GameDataManager.GamePlayerData.PlayerType.EnemyPlayer)
		{
			return -1;
		}

		switch(squareData.Type)
		{
			case SquareData.EffectType.PlusMoney:
				player.AddMoney(100);//とりあえず固定
				break;

			case SquareData.EffectType.MinusMoney:
				player.SubtractMoney(100);//
				break;

			case SquareData.EffectType.Event:
				return squareData.EventId;
				break;
		}
		return -1;
	}

	public static int RollDice(int sumDices)
	{
		return Random.Range(sumDices, 6 * sumDices + 1);
	}

	/// <summary>
	/// カード効果を加味してダイス判定を行う
	/// プレイヤーにかかっているカード効果群から優先度の高い"ダイスに関係する"効果を使用する
	/// </summary>
	/// <param name="cardReader"></param>
	/// <returns></returns>
	public static int RollDiceWithCardEffect(IConvenienceCardDataReader[] effects )
	{
		IConvenienceCardDataReader usingEffect = null;

		//ダイス系効果であることと優先度が高いカード効果を１つ採用する
		int curPriority = -1;
		foreach(var ef in effects)
		{
			if (ef.Kind != ConvenienceCardData.EffectKind.Dice)
				continue;

			if(ef.Priority > curPriority)
			{
				usingEffect = ef;
			}
		}

		return RollDiceWithCardEffect(usingEffect);

	}
	/// <summary>
	/// カード効果からダイス判定を行う
	/// </summary>
	/// <param name="cardReader"></param>
	/// <returns></returns>
	public static int RollDiceWithCardEffect(IConvenienceCardDataReader cardReader)
	{
		if(cardReader == null || cardReader.Kind != ConvenienceCardData.EffectKind.Dice)
		{
			return RollDice(1);
		}

		switch(cardReader.Type)
		{
			case ConvenienceCardData.EffectType.AdditionDices:
				return RollDice(1 + cardReader.Param);
			case ConvenienceCardData.EffectType.FixedMoving:
				return cardReader.Param;
		}

		return RollDice(1);
	}

}

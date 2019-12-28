using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour, IGameDataReader
{
	[SerializeField]
	GameDefaultParamData defaultData;

	List<GamePlayerData> gamePlayerData = new List<GamePlayerData>();

	public List<GamePlayerData> GamePlayers { get { return gamePlayerData; } }
	/// <summary>
	/// 動作するキャラクターはすべてPlayerという扱いにする
	/// </summary>
	public class GamePlayerData
	{
		public GamePlayerData(int maxHp, int money, int curPosId,PlayerType type)
		{
			this.type = type;
			this.maxHp = maxHp;
			this.hp = maxHp;
			this.money = money;
			this.prevMoney = money;
			this.curPosId = curPosId;
			this.prevPosId = curPosId;
			this.battleCardList = new List<IBattleCardDataReader>();
			this.convenienceCardList = new List<IConvenienceCardDataReader>();
		}

		public enum PlayerType
		{
			Player,
			EnemyPlayer,
			NpcEnemy,
			Boss,
		}

		public PlayerType type;
		public int exId = -1;//type: Npc Bossのみ使う保存用
		public int hp;
		public int maxHp;
		public int curPosId;
		public int prevPosId;
		public int money;
		public int prevMoney;
		public List<IBattleCardDataReader> battleCardList;
		public List<IConvenienceCardDataReader> convenienceCardList;

		public void SetPosition(int posId)
		{
			prevPosId = curPosId;
			curPosId = posId;
		}

		public void AddMoney(int value)
		{
			prevMoney = money;
			money += value;
		}

		public void SubtractMoney(int value)
		{
			prevMoney = money;
			money -= value;
		}
	}

	public class GameBossData
	{
		public int hp;
		public int maxHp;
		public int curPosId;
		public int prevPosId;
	}

	public void Save()
	{

	}

	public void Load()
	{

	}

	/// <summary>
	/// 同じ位置にいるプレイヤーを探す
	/// </summary>
	/// <param name="squareId"></param>
	/// <param name="actorId"></param>
	/// <param name="targetPlayerId"></param>
	/// <returns></returns>
	public bool CheckSamePositionPlayer(int squareId,int actorId,out int[] targetPlayerIds)
	{
		targetPlayerIds = null;
		List<int> targetList = new List<int>();

		for(int i = 0; i < gamePlayerData.Count; i++)
		{
			if (i == actorId)
				continue;

			if(gamePlayerData[i].curPosId == squareId)
			{
				targetList.Add(i);
			}
		}

		if(targetList.Count != 0)
		{
			targetPlayerIds = targetList.ToArray();
			return true;
		}

		return false;
	}

	public void Init(int playerSum, int bossId)
	{
		//プレイヤーデータの作成
		for (int i = 0; i < playerSum; i++)
		{
			gamePlayerData.Add(new GamePlayerData(defaultData.MaxHp, defaultData.Money, defaultData.InitPosId, GamePlayerData.PlayerType.Player));
		}

		//ボスデータの作成
	}
	public int ReadPlayerPositionId(int actorId)
	{
		return gamePlayerData[actorId].curPosId;
	}

	public bool ReadPlayerMoney(int actorId, out int curMoney,out int prevMoney)
	{
		if(actorId < 0 || actorId >= gamePlayerData.Count)
		{
			curMoney = -1;
			prevMoney = -1;
			return false;
		}

		var player = gamePlayerData[actorId];
		curMoney = player.money;
		prevMoney = player.prevMoney;
		return true;
	}
}

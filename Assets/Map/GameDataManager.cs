using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour,IGameDataReader
{
	[SerializeField]
	GameDefaultParamData defaultData;

	GamePlayerData[] gamePlayerData;
	GameBossData gameBossData;

	public class GamePlayerData
	{
		public GamePlayerData(int maxHp,int money,int curPosId)
		{
			this.maxHp = maxHp;
			this.hp = maxHp;
			this.money = money;
			this.curPosId = curPosId;
			this.prevPosId = curPosId;
			this.battleCardList = new List<IBattleCardDataReader>();
			this.convenienceCardList = new List<IConvenienceCardDataReader>();
		}

		public int hp;
		public int maxHp;
		public int curPosId;
		public int prevPosId;
		public int money;
		public List<IBattleCardDataReader> battleCardList;
		public List<IConvenienceCardDataReader> convenienceCardList;
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

	public void Init(int playerSum,int bossId)
	{
		//プレイヤーデータの作成
		gamePlayerData = new GamePlayerData[playerSum];
		for(int i = 0; i < gamePlayerData.Length;i++)
		{
			gamePlayerData[i] = new GamePlayerData(defaultData.MaxHp, defaultData.Money, defaultData.InitPosId);
		}

		//ボスデータの作成

	}

	public void SetPlayerPosition(int id,int posId)
	{
		gamePlayerData[id].prevPosId = gamePlayerData[id].curPosId;
		gamePlayerData[id].curPosId = posId;
	}

	public int ReadPlayerPositionId(int id)
	{
		return gamePlayerData[id].curPosId;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStatus
{
	Normal,
	Stop,//休み状態
}

public class PlayerData
{
	public int id;
	public int hp;
	public int money;
	public int maxHp;
	public int[] deckIds;
	public PlayerStatus status;
}

public class EnemyData
{
	int hp;
	int maxHp;
	int id;
	int enemyId;
}

public class BossData
{
	int hp;
	int maxHp;
	int id;
	int bossId;
}

public class GameData
{
	int currentTurn;
}

public enum CommandType
{
	None,//なんらかの理由で行動不能
	Move,
	UsingCard,
}

public class CommandData
{
	public int actorId;
	public CommandType commandType;
	public int id;
	public int[] num;
}

public class EventResultData
{
	public int[] targetActorIds;
}

public class BattlePlayerData
{
	public string userName;
	public int maxHp;
	public int curHp;
	public IConvenienceCardDataReader[] cardData;
	public IBattleCardDataReader[] battleCardData;
}

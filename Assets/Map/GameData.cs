﻿using System.Collections;
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
}

public class GameData
{
	int currentTurn;
}

public enum CommandType
{
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

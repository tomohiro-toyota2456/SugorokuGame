using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameDataReader
{
	/// <summary>
	/// プレイヤーのIdから現在の位置IDを取得する
	/// </summary>
	/// <param name="actorId"></param>
	/// <returns></returns>
	int ReadPlayerPositionId(int actorId);

	bool ReadPlayerMoney(int actorId, out int curMoney, out int prevMoney);

	int ReadPlayerHp(int actorId);
	IToolDataReader ReadPlayerTool(int actorId);
	IArmorDataReader ReadPlayerArmor(int actorId);
	int ReadImageId(int actorId);
	int ReadExId(int actorId);
}

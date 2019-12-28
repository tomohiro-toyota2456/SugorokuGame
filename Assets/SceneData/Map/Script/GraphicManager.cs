using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicManager : MonoBehaviour
{
	[SerializeField]
	UIGraphicManager uiGraphicManager;
	[SerializeField]
	FieldGraphicManager fieldGraphicManager;
	
	public void AddFigure(Sprite sprite,Vector3 pos)
	{
		fieldGraphicManager.CreateAndAddPrefab(sprite, pos);
	}

	public IEnumerator UpdateGraphic(IGameDataReader dataReader,Map map,CommandData commandData)
	{
		switch(commandData.commandType)
		{
			case CommandType.Move:
				yield return StartCoroutine(UpdateMoving(dataReader,map,commandData));
				break;

			case CommandType.UsingCard:
				yield return StartCoroutine(UpdateUsingCard(dataReader, map, commandData));
				break;
		}
	}

	IEnumerator UpdateMoving(IGameDataReader dataReader,Map map,CommandData commandData)
	{
		yield return fieldGraphicManager.StartMoveFromNowPosition(commandData.actorId,CalcTargetPos(map, commandData.num),2.0f);
		StartCoroutine(UpdateUI(dataReader));
	}

	IEnumerator UpdateUI(IGameDataReader dataReader)
	{
		for(int i = 0; i < 4; i++)
		{
			int curMoney = 0;
			int prevMoney = 0;
			if (dataReader.ReadPlayerMoney(i,out curMoney,out prevMoney))
			{
				uiGraphicManager.SetMoney(i, curMoney);
			}
		}

		yield return null;
	}

	IEnumerator UpdateUsingCard(IGameDataReader dataReader, Map map, CommandData commandData)
	{
		yield return null;
	}

	Vector3[] CalcTargetPos(Map map,int[] ids)
	{
		Vector3[] targets = new Vector3[ids.Length];
		for(int i = 0; i < targets.Length; i++)
		{
			targets[i] = map.GetSquare(ids[i]).transform.position;
		}

		return targets;
	}

}

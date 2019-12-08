using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGraphicManager : MonoBehaviour
{
	[SerializeField]
	CharactorFigure charaPrefab;

	List<CharactorFigure> figureList = new List<CharactorFigure>();

	public void CreateAndAddPrefab(Sprite sprite,Vector3 pos)
	{
		var ins = Instantiate(charaPrefab);
		ins.transform.position = pos;
		//ins.SetSprite(sprite);
		figureList.Add(ins);
	}

	public void SetPosition(int idx,Vector3 pos)
	{
		if(idx < 0 || idx >= figureList.Count)
		{
			return;
		}

		figureList[idx].transform.position = pos;
	}

	public Coroutine StartMoveFromNowPosition(int idx,Vector3[] targets,float time)
	{
		return StartCoroutine(MoveFromNowPosition(idx, targets, time));
	}

	IEnumerator MoveFromNowPosition(int idx,Vector3[] targets,float time)
	{
		float oneTime = time / targets.Length;
		for(int i = 0; i < targets.Length;i++)
		{
			yield return StartMoveFromNowPosition(idx, targets[i], oneTime);
		}
	}

	public Coroutine StartMoveFromNowPosition(int idx,Vector3 target,float time)
	{
		return StartCoroutine(MoveFromNowPosition(idx,target,time));
	}

	IEnumerator MoveFromNowPosition(int idx,Vector3 target,float time)
	{
		Vector3 pos = figureList[idx].transform.position;

		float timer = 0;
		while (timer <= 1)
		{
			float t = timer / time;
			figureList[idx].transform.position = Vector3.Lerp(pos,target, t);
			timer += Time.deltaTime;
			yield return null;
		}

		figureList[idx].transform.position = Vector3.Lerp(pos, target, 1);

	}


}

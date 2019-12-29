using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	//後々会話イベ等

	public IEnumerator WaitEvent(int eventId,int actorId)
	{
		var eventResultData = new EventResultData();

		//発生するイベントを待つ
		yield return null;

		yield return eventResultData;
	}
}

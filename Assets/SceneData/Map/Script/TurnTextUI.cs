using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTextUI : MonoBehaviour
{
	[SerializeField]
	Text turnTextUI;

	public void SetPlayerName(string playerName)
	{
		turnTextUI.text = string.Format("{0}\nStart!", playerName);
	}

	public void SetTurn(int turnNum)
	{
		turnTextUI.text = string.Format("Turn {0}\nStart!",turnNum);
	}

	public IEnumerator PlayTurnAnimation(float expantionTime,float reductionTime,float viewTime)
	{
		Vector3 scl = Vector3.zero;
		turnTextUI.transform.localScale = scl;

		float timer = 0;
		Vector3 targetScl = new Vector3(1.5f, 1.5f, 1.0f);
		while(timer < expantionTime)
		{
			timer += Time.deltaTime;
			float t = timer / expantionTime;
			if (t > 1)
			{
				t = 1;
			}

			turnTextUI.transform.localScale = scl * (1 - t) + targetScl * t;
			yield return null;
		}

		timer = 0;
		targetScl = new Vector3(1.0f, 1.0f, 1.0f);
		scl = turnTextUI.transform.localScale;
		while (timer < expantionTime)
		{
			timer += Time.deltaTime;
			float t = timer / 0.3f;

			if(t > 1)
			{
				t = 1;
			}

			turnTextUI.transform.localScale = scl * (1 - t) + targetScl * t;
			yield return null;
		}

		yield return new WaitForSeconds(viewTime);

		timer = 0;
		targetScl = new Vector3(0f, 0f, 0f);
		scl = turnTextUI.transform.localScale;
		while (timer < reductionTime)
		{
			timer += Time.deltaTime;
			float t = timer / 0.3f;
			if (t > 1)
			{
				t = 1;
			}

			turnTextUI.transform.localScale = scl * (1 - t) + targetScl * t;
			yield return null;
		}

	}
}

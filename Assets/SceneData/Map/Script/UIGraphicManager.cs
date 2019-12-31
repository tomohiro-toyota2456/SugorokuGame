using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGraphicManager : MonoBehaviour
{
	[SerializeField]
	PlayerStatusUIPanel[] uiPanels;
	[SerializeField]
	TurnTextUI turnTextUI;

	public void SetMoney(int uiIdx,int money)
	{
		uiPanels[uiIdx].SetMoneyText(money);
	}

	public Coroutine StartTurnTextAnimation(string playerName)
	{
		turnTextUI.SetPlayerName(playerName);
		return StartCoroutine(turnTextUI.PlayTurnAnimation(0.5f, 0.5f, 1.5f));
	}

	public Coroutine StartTurnTextAnimation(int turnNum)
	{
		turnTextUI.SetTurn(turnNum);
		return StartCoroutine(turnTextUI.PlayTurnAnimation(0.5f, 0.5f, 1.5f));
	}

	public void SetActiveTurnTextUI(bool isActive)
	{
		turnTextUI.gameObject.SetActive(isActive);
	}
}

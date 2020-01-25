using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
	[SerializeField]
	MapCameraMoveButtonManager mapCameraMoveButtonManager;
	[SerializeField]
	GameObject diceButton;//ダイス投げモード
	[SerializeField]
	GameObject diceDecisionButton;//ダイス投げけってい
	[SerializeField]
	GameObject cameraModeButton;//カメラモード切替ボタン
	[SerializeField]
	GameObject backButton;//戻るボタン
	[SerializeField]
	GameObject[] menuModeViewButtons;

	bool isWaitCommand = true;
	int usingCardId = -1;

	int sumDices;//いくつのダイス
	int numDice;//出目
	public void Init(int sumDices,int numDice)
	{
		isWaitCommand = true;
		this.sumDices = sumDices;
		this.numDice = numDice;
		usingCardId = -1;
		ChangeMenuMode();
		mapCameraMoveButtonManager.SetState(false);
	}

	public void ChangeMenuMode()
	{
		for(int i = 0; i < menuModeViewButtons.Length;i++)
		{
			menuModeViewButtons[i].SetActive(true);
		}

		backButton.SetActive(false);
		diceDecisionButton.SetActive(false);
	}

	public void ChangeCommandMode()
	{
		for (int i = 0; i < menuModeViewButtons.Length; i++)
		{
			menuModeViewButtons[i].SetActive(false);
		}

		backButton.SetActive(true);
	}

	public void UseDicePre()
	{
		//カメラモードを戻す
		mapCameraMoveButtonManager.SetState(false);
		//なんらかのアニメーションを開始

		//ダイス投下決定ボタンを表示
		diceDecisionButton.SetActive(true);

		//行動モードへ変更
		ChangeCommandMode();
	}

	public void ChangeCameraMode(bool isModeButtonShow)
	{
		mapCameraMoveButtonManager.SetState(true);
		cameraModeButton.SetActive(isModeButtonShow);
	}

	public void ChangeCameraFixedMode(bool isModeButtonShow)
	{
		mapCameraMoveButtonManager.SetState(false);
		cameraModeButton.SetActive(isModeButtonShow);
	}

	public void OpenCardList()
	{

	}

	public void SelectCard()
	{

	}

	public void UseCard()
	{

	}

	public void UseDice()
	{
		diceDecisionButton.SetActive(false);
		backButton.SetActive(false);

		//アニメーション
		StartCoroutine(WaitDiceAnimation());
	}

	IEnumerator WaitDiceAnimation()
	{ 
		//転がりアニメーションあるなら待ち
		while(false)
		{
			yield return null;
		}

		isWaitCommand = false;

	}

	public void CancelDice()
	{
		diceButton.SetActive(false);
		//アニメーション停止

		ChangeMenuMode();
	}


	public Coroutine WaitCommandCompleteCoroutine(out IEnumerator enumerator)
	{
		enumerator = WaitCommandComplete();
		return StartCoroutine(enumerator);
	}


	IEnumerator WaitCommandComplete()
	{
		while(isWaitCommand)
		{
			yield return null;
		}

		yield return usingCardId;
	}



}

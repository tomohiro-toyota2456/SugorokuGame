using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// タイトル　本文　閉じるのシンプルポップアップ
/// </summary>
public class PopupSimple : MonoBehaviour,IPopupBase
{
	[SerializeField]
	PopupAnimation popupAnimation;
	[SerializeField]
	Text titleTextUI;
	[SerializeField]
	Text descTextUI;

	bool isEnding = false;

	public void SetText(string titleText,string descText)
	{
		titleTextUI.text = titleText;
		descTextUI.text = descText;
	}

	virtual public void Open()
	{
		popupAnimation.PlayOpeningAnimation();
	}

	virtual public void Close()
	{
		popupAnimation.PlayClosingAnimation();
		isEnding = true;
	}

	public Coroutine Wait()
	{
		return StartCoroutine(WaitCoroutine());
	}

	virtual protected IEnumerator WaitCoroutine()
	{
		while (!isEnding || !popupAnimation.IsPlayingAnimation)
			yield return null;
	}

	public void OnClickClose()
	{
		Close();
	}
}

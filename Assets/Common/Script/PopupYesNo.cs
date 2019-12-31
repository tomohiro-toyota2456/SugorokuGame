using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ２択ポップアップ
/// </summary>
public class PopupYesNo : PopupSimple,IPopupYesNo
{	
	bool isYes = false;

	public void SelectYes()
	{
		isYes = true;
		Close();
	}

	public void SelectNo()
	{
		isYes = false;
		Close();
	}

	public void OnClickYes()
	{
		SelectYes();
	}

	public void OnClickNo()
	{
		SelectNo();
	}

	public bool GetResult()
	{
		return isYes;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopupBase
{
	void SetText(string title, string desc);
	void Open();
	void Close();
	/// <summary>
	/// 終了をこれで待つ
	/// </summary>
	/// <returns></returns>
	Coroutine Wait();
}

public interface IPopupYesNo : IPopupBase
{
	void SelectYes();
	void SelectNo();
	/// <summary>
	/// YesNoをWait後に取得できる関数
	/// </summary>
	/// <returns></returns>
	bool GetResult();
}

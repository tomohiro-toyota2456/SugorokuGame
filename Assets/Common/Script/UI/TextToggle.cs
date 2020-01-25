using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextToggle : MonoBehaviour,IToggleController
{
	[SerializeField]
	Text text;
	[SerializeField]
	string[] onoffStr;


	bool isOn = false;
	System.Action onAction;
	System.Action offAction;

	public void ClickToggle()
	{
		isOn = !isOn;
		SetToggleState(isOn);
	}

	public void OnClickToggle()
	{
		ClickToggle();
	}

	public void SetOnOffActions(Action onAction, Action offAction)
	{
		this.onAction = onAction;
		this.offAction = offAction;
	}

	public void SetToggleState(bool isOn)
	{
		System.Action action = isOn ? onAction : offAction;
		text.text = isOn ? onoffStr[0] : onoffStr[1];
		action();
	}
}

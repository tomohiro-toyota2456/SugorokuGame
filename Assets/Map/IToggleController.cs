using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IToggleController
{
	void SetOnOffActions(System.Action onAction, System.Action offAction);
	void SetToggleState(bool isOn);
	void ClickToggle();
}

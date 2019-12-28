using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGraphicManager : MonoBehaviour
{
	[SerializeField]
	PlayerStatusUIPanel[] uiPanels;

	public void SetMoney(int uiIdx,int money)
	{
		uiPanels[uiIdx].SetMoneyText(money);
	}
}

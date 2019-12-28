using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUIPanel : MonoBehaviour
{
	[SerializeField]
	Text moneyTextUI;

	public void SetMoneyText(int money)
	{
		moneyTextUI.text = money.ToString();
	}
}

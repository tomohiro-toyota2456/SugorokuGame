using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviourSingleton<PopupManager>
{
	[SerializeField,Tooltip("配列順はType順でいれること")]
	GameObject[] popupObjs;
	[SerializeField]
	Transform root;//生成先

	List<GameObject> popupList = new List<GameObject>();

	public enum PopupType
	{
		Simple = 0,
		YesNo,
	}

	public GameObject CreatePopup(PopupType type)
	{
		var ins = Instantiate(popupObjs[(int)type]);
		ins.transform.SetParent(root);
		ins.transform.localPosition = Vector3.zero;
		ins.transform.localScale = Vector3.one;
		popupList.Add(ins);
		return ins;
	}
	/// <summary>
	/// 移動先の確認ポップアップ
	/// </summary>
	/// <returns></returns>
	public IPopupYesNo CreateMovingVerificationPopup()
	{
		var ins = CreatePopup(PopupType.YesNo);
		var yesNo = ins.GetComponent<IPopupYesNo>();
		yesNo.SetText("移動しますか？", "決定すると前には戻れませんがよろしいですか？");
		return yesNo;
	}

	/// <summary>
	/// 破棄命令
	/// </summary>
	/// <param name="obj"></param>
	public void DestroyPopup(GameObject obj)
	{
		for(int i = 0; i < popupList.Count;i++)
		{
			if(popupList[i] == obj)
			{
				popupList.RemoveAt(i);
				DestroyImmediate(obj);
			}
		}
	}

}

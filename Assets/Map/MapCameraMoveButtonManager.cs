using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// カメラ移動ボタンUI管理
/// </summary>
public class MapCameraMoveButtonManager : MonoBehaviour
{
	[SerializeField]
	GameObject toggleUI;//インターフェースで受け取りたいけどできないので・・・
	[SerializeField]
	CameraMoveButton[] buttons;
	[SerializeField]
	Camera camera;

	IToggleController toggleController;

	public void Awake()
	{
		toggleController = toggleUI.GetComponent<IToggleController>();
		toggleController.SetOnOffActions(() => { SetStateUI(true); }, () => { SetStateUI(false); });
		toggleController.SetToggleState(false);
		Init(camera, new SimpleMapCameraMover());
	}

	public void Init(Camera cam,IMapCameraMover mover)
	{
		foreach(var b in buttons)
		{
			b.Target = cam.transform;
			b.Mover = mover;
		}
	}

	void SetStateUI(bool active)
	{
		foreach (var b in buttons)
		{
			b.gameObject.SetActive(active);
		}
	}

	public void SetState(bool active)
	{
		toggleController.SetToggleState(active);
	}
}

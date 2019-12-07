using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// カメラをボタン操作で移動させるUI制御
/// </summary>
public class CameraMoveButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
	[SerializeField]
	Vector3 vec;//進行方向

	public Transform Target { get; set; }
	public IMapCameraMover Mover { private get; set; }

	bool isDown = false;
	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		if(isDown)
		{
			return;
		}
		isDown = true;
		StartCoroutine(Wait());

	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		isDown = false;
	}

	IEnumerator Wait()
	{
		if (Mover == null)
			yield break;

		while (isDown)
		{
			Mover.Move(Target, vec);
			yield return null;
		}

	}

}

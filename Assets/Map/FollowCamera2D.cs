using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//2Dの箱カメラ向け
public class FollowCamera2D : MonoBehaviour
{
	[SerializeField]
	float leftLimit;
	[SerializeField]
	float rightLimit;
	[SerializeField]
	float topLimit;
	[SerializeField]
	float bottomLimit;
	[SerializeField]
	Transform targetTrans;
	[SerializeField]
	Camera targetCamera;

	Vector3 zero = new Vector3(0, 0, 0);
	Vector3 one = new Vector3(1, 1, 0);
	// Use this for initialization
	void Start()
	{
		if (targetCamera == null)
		{
			targetCamera = GetComponent<Camera>();
		}
	}

	public void SetLimit(float left, float right, float top, float bottom)
	{
		leftLimit = left;
		rightLimit = right;
		topLimit = top;
		bottomLimit = bottom;
	}

	public void SetTargetTrans(Transform trans)
	{
		targetTrans = trans;
	}

	public void SetTargetCamera(Camera cam)
	{
		targetCamera = cam;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 camPos = targetCamera.transform.position;
		camPos.x = targetTrans.position.x;
		camPos.y = targetTrans.position.y;

		targetCamera.transform.position = camPos;

		zero.z = targetCamera.nearClipPlane;
		one.z = targetCamera.nearClipPlane;
		Vector3 bottomLeft = targetCamera.ViewportToWorldPoint(zero);
		Vector3 topRight = targetCamera.ViewportToWorldPoint(one);

		float xFix = 0;
		if (topRight.x > rightLimit)
		{
			xFix = rightLimit - topRight.x;
		}
		else if (bottomLeft.x < leftLimit)
		{
			xFix = leftLimit - bottomLeft.x;
		}

		float yFix = 0;

		if (bottomLeft.y < bottomLimit)
		{
			yFix = bottomLimit - bottomLeft.y;
		}
		else if (topRight.y > topLimit)
		{
			yFix = topLimit - topRight.y;
		}

		camPos.x += xFix;
		camPos.y += yFix;

		targetCamera.transform.position = camPos;

	}
}


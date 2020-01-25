using UnityEngine;

public class SimpleMapCameraMover : IMapCameraMover
{
	void IMapCameraMover.Move(Transform trans,Vector3 vec)
	{
		trans.position += vec;
	}

}

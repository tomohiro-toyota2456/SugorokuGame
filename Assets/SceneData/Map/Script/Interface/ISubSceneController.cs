using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubSceneController<T,K>
{
	void Init(IGameDataReader dataReader,int[] targets,K param);
	IEnumerator WaitSubScene();
	T result { get; }
}

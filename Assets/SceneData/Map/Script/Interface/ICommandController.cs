using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandController
{
	IEnumerator WaitCommand(IGameDataReader gameDataReader, GameUIController gameUIController,Map map);
	void Init(int id);
	int Id { get; }
}

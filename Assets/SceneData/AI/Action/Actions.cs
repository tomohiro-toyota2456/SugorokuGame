using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIAction
{
	bool Act(IGameDataReader dataReader,int actorIdx,Map map,AICommand command);
}

public class AICommand
{
	public enum CommandType
	{
		UseDice,
		UseCard,
		Buy,
		CreateWepon,
		CreateArmor,
	}

	public CommandType commandType;
	public int[] userData;
}

[System.Serializable]
public class AIActionBase
{
	[SerializeReference]
	public IAIAction parent;
	[SerializeReference]
	public List<IAIAction>children;
}

[System.Serializable]
public class AIDiceAction : AIActionBase,IAIAction
{
	bool IAIAction.Act(IGameDataReader dataReader,int actorIdx,Map map,AICommand command)
	{
		command.commandType = AICommand.CommandType.UseDice;
		return true;
	}
}

[System.Serializable]
public class AISequence : AIActionBase,IAIAction
{
	bool IAIAction.Act(IGameDataReader dataReader, int actorIdx, Map map, AICommand command)
	{
		foreach(var child in children)
		{
			if(!child.Act(dataReader,actorIdx,map,command))
			{
				return false;
			}
		}

		return true;
	}
}

[System.Serializable]
public class AISelector : AIActionBase,IAIAction
{
	bool IAIAction.Act(IGameDataReader dataReader, int actorIdx, Map map, AICommand command)
	{
		foreach (var child in children)
		{
			if (child.Act(dataReader, actorIdx, map, command))
			{
				return true;
			}
		}

		return false;
	}
}



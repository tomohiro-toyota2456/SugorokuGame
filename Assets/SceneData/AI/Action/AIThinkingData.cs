using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIThinkingData : ScriptableObject
{
	[SerializeReference]
	private List<IAIAction> actions;

	public IAIAction Root { get { return actions[0]; } }
}

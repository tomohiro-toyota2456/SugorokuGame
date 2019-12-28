using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData : ScriptableObject,IEventDataReader
{
	[SerializeField]
	int id;
	[SerializeField]
	int eventDetailId;
	[SerializeField]
	EventType type;

	public enum EventType
	{
		Talk,//2択や会話イベ
		Shop,//ショップ　イベント扱い
	}

	public int Id { get { return id; } set { id = value; } }
	public int EventDetailId { get { return eventDetailId; } set { eventDetailId = value; } }
	public EventType Type { get { return type; } set { type = value; } }
}

public interface IEventDataReader
{
	int Id { get; }
	int EventDetailId { get; }
	EventData.EventType Type { get; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが装備するツールデータ（武器）
/// </summary>
public class ToolData : ScriptableObject,IToolDataReader
{
	[SerializeField]
	int id;
	[SerializeField]
	new string name;
	[SerializeField]
	int atk;
	[SerializeField]
	MaterialType materialType;
	[SerializeField]
	int bonusParam;

	public string Name { get { return name; } set { name = value; } }
	public int Id { get { return id; } set { id = value; } }
	public int Atk { get { return atk; } set { atk = value; } }
	public MaterialType MaterialType { get { return materialType; } set { materialType = value; } }
	public int BonusParam { get { return bonusParam; } set { bonusParam = value; } }
}

public interface IToolDataReader
{
	int Id { get; }
	string Name { get; }
	int Atk { get; }
	MaterialType MaterialType { get; }
	int BonusParam { get; }
}

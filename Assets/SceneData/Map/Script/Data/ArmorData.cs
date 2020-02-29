using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorData : ScriptableObject,IArmorDataReader
{
	[SerializeField]
	int id;
	[SerializeField]
	new string name;
	[SerializeField]
	int def;

	public int Id { get { return id; } set { id = value; } }
	public string Name { get { return name; } set { name = value; } }
	public int Def { get { return def; } set { def = value; } }
}

public interface IArmorDataReader
{
	int Id { get; }
	string Name { get; }
	int Def { get; }
}

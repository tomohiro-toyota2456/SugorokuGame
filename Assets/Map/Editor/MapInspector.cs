using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Map))]
public class MapInspector : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("CreateMapData"))
		{
			AddSquares();
			CalcAccess();
			CalcMapSize();
		}
	}

	void AddSquares()
	{
		Map map = target as Map;

		Transform root = map.transform.root;

		Square[] children = root.GetComponentsInChildren<Square>();

		var type = children[0].GetType();
		var idField = type.GetField("id", System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		for (int i = 0; i < children.Length; i++)
		{
			children[i].name = "Square_id:" + i.ToString();
			idField.SetValue(children[i], i);
		}

		var mapType = map.GetType();
		var squaresField = mapType.GetField("squares", System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		squaresField.SetValue(map, children);

	}

	void CalcAccess()
	{
		Map map = target as Map;

		Transform root = map.transform.root;
		Transform access = null;
		foreach(var child in  root)
		{
			var c = child as Transform;
			if(c.name == "Access")
			{
				access = c;
				break;
			}
		}

		if(access == null)
		{
			return;
		}

		List<int>[] localAccessData = new List<int>[map.SquareSum];

		foreach(var child in access)
		{
			var node = child as Transform;
			var renderer = node.GetComponent<SpriteRenderer>();

			//いったん pibotはcenter
			//伸ばす方向はx軸のみ 残りは回転で合わせる前提とする
			float size = renderer.sprite.bounds.size.x;
			float half = size * node.transform.localScale.x * 0.5f;

			Debug.Log(node.name + half);

			float angle = node.localEulerAngles.z * Mathf.PI / 180;

			Vector3 lpos = new Vector3(Mathf.Cos(angle) * -half + node.position.x, Mathf.Sin(angle) * -half + node.position.y, node.position.z);
			Vector3 rpos = new Vector3(Mathf.Cos(angle) * half + node.position.x, Mathf.Sin(angle) * half + node.position.y, node.position.z);

			int lId = GetNearSquareId(lpos);
			int rId = GetNearSquareId(rpos);

			if(lId == rId)
			{
				Debug.Assert(false, "接続IDが同一を検出 id:" + rId.ToString());

				continue;
			}

			if(localAccessData[lId] == null)
			{
				localAccessData[lId] = new List<int>();
			}

			if (localAccessData[rId] == null)
			{
				localAccessData[rId] = new List<int>();
			}

			if(localAccessData[lId].Contains(rId))
			{
				Debug.Assert(false, "接続先がすでに登録されています" +lId +"->"+rId);
			}
			else
			{
				localAccessData[lId].Add(rId);
			}

			if(localAccessData[rId].Contains(lId))
			{
				Debug.Assert(false, "接続先がすでに登録されています"+rId + "->" + lId);
			}
			else
			{
				localAccessData[rId].Add(lId);
			}
		}

		var type = map.GetSquare(0).GetType();
		var accessIdsField = type.GetField("accessIds", System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		for(int i = 0; i < map.SquareSum; i++)
		{
			var square = map.GetSquare(i);
			if (square == null)
			{
				break;
			}

			accessIdsField.SetValue(square, localAccessData[i].ToArray());
		}

	}

	int GetNearSquareId(Vector3 position)
	{
		var map = target as Map;
		int max = map.SquareSum;
		int retId = -1;
		float minDist = 9999999;//限りなく大きい数値　これが最も近い場合は設計がおかしい
		for(int i = 0; i < max; i++)
		{
			var square = map.GetSquare(i);
			if(square == null)
			{
				break;
			}

			var dist = Vector3.Distance(square.transform.position, position);
			if (dist < minDist)
			{
				retId = square.Id;//iを代入でも変わらん（id = idxにする設計上)
				minDist = dist;
			}
			else if(dist == minDist)
			{
				Debug.Assert(false,"同一数値はおかしい可能性があります。マップの設計を見直してください id:"+square.Id +"id:"+retId);
			}
		}
		return retId;
	}

	void CalcMapSize()
	{
		//直下にBGある前提
		Map map = target as Map;

		Transform root = map.transform.root;
		Transform bg = null;
		foreach (var child in root)
		{
			var c = child as Transform;
			if (c.name == "BG")
			{
				bg = c;
				break;
			}
		}

		if (bg == null)
			return;

		var renderer = bg.gameObject.GetComponent<SpriteRenderer>();

		if (renderer == null)
			return;

		float width = renderer.bounds.size.x;
		float height = renderer.bounds.size.y;

		var mapType = map.GetType();
		var widthField = mapType.GetField("width", System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
		var heightField = mapType.GetField("height", System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		widthField.SetValue(map, width);
		heightField.SetValue(map, height);
	}

}

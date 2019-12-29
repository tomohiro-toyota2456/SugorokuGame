using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	[SerializeField]
	Square[] squares;//Id = 配列番号になるように格納（検索のしやすさを優先)
	[SerializeField]
	float width;//マップサイズ 0,0位置前提
	[SerializeField]
	float height;
	[SerializeField]
	Camera mapCamera;//マップを見るようのカメラ レイを飛ばすのに使う

	public int SquareSum { get { return squares.Length; } }
	public float Width { get { return width; } }
	public float Height { get { return height; } }

	Dictionary<int, List<int>> rootDict;

	public Square GetSquare(int idx)
	{
		if(idx < 0 || idx >= squares.Length)
		{
			return null;
		}

		return squares[idx];
	}

	public Dictionary<int, Tree<int>> CalcEnableRoot(int num,int curIdx)
	{

		Dictionary<int, Tree<int>> deepDict = new Dictionary<int, Tree<int>>();
		bool isLoop = true;

		var root = GetSquare(curIdx);

		Tree<int> tree = new Tree<int>();
		tree.Data = curIdx;
		Tree<int> curTree = tree;

		int deep = 0;
		while(isLoop)
		{
			var sq = GetSquare(curTree.Data);
			int prevDeep = deep;
			for (int i = 0; i < sq.AccessIdSum; i++)
			{
				if(curTree.GetChild(i) != null)
				{
					continue;
				}

				int parentIdx = curTree.Parent != null ? curTree.Parent.Data : -1;

				if(parentIdx == sq.AccessIds[i])
				{
					continue;
				}


				bool isPass = false;
				for(int te = 0; ;te++)
				{
					var tee = curTree.GetChild(te);

					if(tee == null)
					{
						break;
					}

					if(tee.Data == sq.AccessIds[i])
					{
						isPass = true;
					}

				}

				if(isPass)
				{
					continue;
				}


				Tree<int> t = new Tree<int>();
				t.Data = sq.AccessIds[i];

			//	Debug.Log("データ:" + curTree.Data + "に子要素"+t.Data+"追加");
				curTree.AddChildren(t);
				t.Parent = curTree;
				curTree = t;

				deep++;
				break;
			}

			if(prevDeep == deep)
			{
				curTree = curTree.Parent;
				deep--;
			}

			if(deep < 0)
			{
				break;
			}

			if(deep == num)
			{
				
				if(!deepDict.ContainsKey(curTree.Data))
				{
					deepDict.Add(curTree.Data, curTree);
				}

				deep--;
				curTree = curTree.Parent;
			}
		}

	//	Debug.Log("いけるのは");
		foreach(var data in  deepDict)
		{
			Debug.Log(data.Key);
		}

		return deepDict;
	}

	public Coroutine SelectSquareCoroutine(out IEnumerator enumerator,Dictionary<int,Tree<int>> root)
	{
		enumerator = SelectSquare(root);
		return StartCoroutine(enumerator);
	}

	public IEnumerator SelectSquare(Dictionary<int, Tree<int>> root)
	{
		int selectedId = -1;
		while (true)
		{
			if (Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Ended)
				{
					Debug.Log("Tap");
					Vector2 screenTouchPos = touch.position;

					Ray ray = mapCamera.ScreenPointToRay(screenTouchPos);
					RaycastHit hit;
					int mask = 1 << 9;
					if (Physics.Raycast(ray, out hit, 1000, mask))
					{
						int id = hit.collider.GetComponent<Square>().Id;

						if (root != null && root.ContainsKey(id))
						{
							selectedId = id;
							break;
						}
					}
				}
			}

#if UNITY_EDITOR

			if(Input.GetMouseButtonUp(0))
			{
				Vector2 screenTouchPos = Input.mousePosition;

				Ray ray = mapCamera.ScreenPointToRay(screenTouchPos);
				Debug.Log("Ray:"+ray.direction);
				RaycastHit hit;
				int mask = 1 << 9;
				if (Physics.Raycast(ray, out hit, 1000,mask))
				{
					int id = hit.collider.GetComponent<Square>().Id;

					if (root != null && root.ContainsKey(id))
					{
						selectedId = id;
						break;
					}
				}
			}

#endif

			yield return null;
		}

		yield return selectedId;
	}

	public void Test(int i)
	{
		CalcEnableRoot(i, 0);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree<T>
{
	List<Tree<T>> children;

	public T Data { get; set; }
	public Tree<T> Parent { get; set; } = null;

	public void AddChildren(Tree<T> child)
	{
		if(children == null)
		{
			children = new List<Tree<T>>();
		}

		children.Add(child);
	}

	public Tree<T> GetChild(int idx)
	{
		if (children == null)
			return null;

		if(idx < 0 || idx >= children.Count)
			return null;

		return children[idx];
	}

	public Tree<T>[] GetOrderDeepTrees(int deep)
	{
		Tree<T> curTree = this;

		List<Tree<T>> ret = new List<Tree<T>>();

		int[] deepChecker = new int[deep];

		bool isLoop = true;

		int curDeep = 0;
		while(isLoop)
		{
			Tree<T> child = curTree.GetChild(deepChecker[curDeep]);
			if(child == null)
			{
				if(curDeep == deep)
				{
					ret.Add(curTree);
				}

				curDeep--;

				if(curDeep <= 0)
				{
					isLoop = false;
				}
				else
				{
					deepChecker[curDeep]++;
				}
				continue;
			}

			curDeep++;
			curTree = child;

			if(curDeep == deep)
			{
				ret.Add(curTree);
				curTree = child.Parent;
				curDeep--;
				deepChecker[curDeep]++;
			}
		}

		return ret.ToArray();
	}

	public Tree<T>[] GetRoot()
	{
		List<Tree<T>> ret = new List<Tree<T>>();
		bool isLoop = true;

		Tree<T> curTree = this;

		ret.Insert(0, curTree);

		while(isLoop)
		{
			curTree = curTree.Parent;

			if(curTree == null)
			{
				isLoop = false;
				continue;
			}

			ret.Insert(0, curTree);
		}

		return ret.ToArray();
	}
}

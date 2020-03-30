using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public static class  AssetDatabaseEx 
{
	public static T[] GetAssetsFromFolder<T>(string folderPath) where T : class
	{
		string[] paths = Directory.GetFiles(folderPath);

		List<T> list = new List<T>();

		foreach(var path in paths)
		{
			var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

			if(asset is T)
			{
				list.Add(asset as T);
			}

		}

		return list.ToArray();
	}
}

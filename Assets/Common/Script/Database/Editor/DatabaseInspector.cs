using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
public static class DatabaseInspector
{
	static public void Set<T>(string searchPath, Object obj) where T : class
	{
		var objs = AssetDatabaseEx.GetAssetsFromFolder<T>(searchPath); 

		var type = obj.GetType();
		var field = type.GetField("data", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

		
		field.SetValue(obj, objs);
	}
}

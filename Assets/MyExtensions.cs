using System;
using System.Reflection;
using UnityEngine;

namespace ExtensionMethods
{
	public static class MyExtensions
	{
		public static int getCoverValue<T>(this T coverType)
		{
			string name = Enum.GetName(typeof(T), coverType);
			FieldInfo field = typeof(T).GetField(name);
			CostAttribute attr = Attribute.GetCustomAttribute(field, typeof(CostAttribute)) as CostAttribute;
			if (attr == null)
			{
				Debug.Log($"{name} of type {typeof(T)} does not have a decoration [cost] DEFAULT is 0");
				return 0;
			}
			return attr.cost;
		}
	}
}
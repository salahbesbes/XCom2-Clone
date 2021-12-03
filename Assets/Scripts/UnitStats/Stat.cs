using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
	[SerializeField]
	private int baseValue = 5;
	public List<int> modifiers = new List<int>();

	public int Value
	{
		get
		{
			int finalValue = baseValue;
			modifiers.ForEach(el => finalValue += el);
			return finalValue;
		}
		set
		{
			baseValue = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public void AddModifier(int modifier)
	{
		if (modifier != 0)
		{
			modifiers.Add(modifier);
		}
	}

	public void RemoveModifier(int modifier)
	{
		if (modifier != 0)
		{
			modifiers.Remove(modifier);
		}
	}
}
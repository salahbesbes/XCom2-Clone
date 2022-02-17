using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "new iventory", menuName = "Inventory ")]

public class Inventory : ScriptableObject
{
	public List<Item> items = new List<Item>();
	public int space = 20;
	[HideInInspector] public AnyClass unit;



	public bool Add(Item item)
	{
		// Check if out of space
		if (items.Count >= space)
		{
			Debug.Log("Not enough room.");
			return false;
		}

		items.Add(item);
		return true;
	}

	public bool remove(Item item)
	{
		if (items.Count <= 0)
			return false;
		items.Remove(item);

		return true;


	}

	private void OnEnable()
	{
		items.Clear();
	}
}




using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	#region Singleton

	public static Inventory instance;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("More than one instance of Inventory found!");
			return;
		}

		instance = this;
	}

	#endregion Singleton

	public delegate void OnItemChanged();

	public OnItemChanged onItemChangedCallback;

	public List<Item> items = new List<Item>();

	public bool Add(Item item)
	{
		items.Add(item);
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();

		return true;
	}

	public void Remove(Item item)
	{
		items.Remove(item);

		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
}
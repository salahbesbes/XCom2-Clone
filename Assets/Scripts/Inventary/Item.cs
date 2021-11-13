using System.Collections;
using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
	public new string name = "New Item";

	public Sprite icon = null;

	public virtual void Use()
	{
		Debug.Log("Using " + name);
	}

	public void RemoveFromInventory()
	{
		Inventory.instance.Remove(this);
	}
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Action : Item
{
	public override void Use()
	{
		base.Use();
	}

	public IEnumerator move(UnitAction unit, MoveAction moveInstance, Vector3[] turnPoints)
	{
		for (int i = 0; i < 4; i++)
		{
			yield return null;
		}
	}

	public void CreateNewShootAction()
	{
	}
}
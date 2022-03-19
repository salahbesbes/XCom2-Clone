using UnityEngine;

[CreateAssetMenu(fileName = "new Item ", menuName = "Inventory / Item")]
public class Item : ScriptableObject
{
	public Sprite icon;
	public string nameItem;
	public SelectableItem prefab;

	public virtual void Use()
	{
		Debug.Log($"virtual use");
	}
}
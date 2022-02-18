using UnityEngine;

public class InventoryUI : MonoBehaviour
{

	Transform itemHolder;
	SlotUi[] slots;



	private void Update()
	{

	}


	private void Start()
	{
		itemHolder = transform.GetChild(0);
		slots = itemHolder.GetComponentsInChildren<SlotUi>();

	}



	public void updateUIInventory(Inventory inventory)
	{
		itemHolder = transform.GetChild(0);

		slots = itemHolder.GetComponentsInChildren<SlotUi>();

		for (int i = 0; i < itemHolder.childCount; i++)
		{

			if (i < inventory.items.Count)
			{
				slots[i].addItem(inventory.items[i]);
			}
			else
			{
				slots[i].clearSlot();
			}
		}
	}

	public void close()
	{
		itemHolder = transform.GetChild(0);
		itemHolder.gameObject.SetActive(false);
	}
	public void open()
	{
		itemHolder = transform.GetChild(0);
		itemHolder.gameObject.SetActive(true);
	}
	private void OnEnable()
	{
		if (GameStateManager.Instance?.SelectedUnit?.inventory)
			updateUIInventory(GameStateManager.Instance.SelectedUnit.inventory);
	}

}
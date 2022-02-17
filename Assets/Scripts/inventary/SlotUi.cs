using UnityEngine;
using UnityEngine.UI;

public class SlotUi : MonoBehaviour
{

	Image icon;
	Transform iconTransfort;
	Transform closeButton;
	public Item item = null;

	private void Awake()
	{
		iconTransfort = transform.GetChild(0).GetChild(0);
		icon = iconTransfort.GetComponent<Image>();
		closeButton = transform.GetChild(1);
	}



	public void addItem(Item itemParam)
	{
		item = itemParam;
		icon.sprite = itemParam.icon;
		transform.name = $"{itemParam.nameItem}";
		closeButton.gameObject.SetActive(true);
		iconTransfort.gameObject.SetActive(true);

	}


	public void clearSlot()
	{
		item = null;
		icon.sprite = null;
		transform.name = "cleared";
		closeButton.gameObject.SetActive(false);
		iconTransfort.gameObject.SetActive(false);


	}

	public void useItem()
	{
		item.Use();
	}


	public void removeThisItem(Inventory inventory)
	{
		inventory.remove(item);
	}


}






using UnityEngine;

public class Equipement : MonoBehaviour
{
	public EquipementData equipement;

	private void OnTriggerEnter(Collider other)
	{
		if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
		{

			Stats pickerStats = other.transform.parent.parent.GetComponent<Stats>();
			pickerStats.addArmorModifier(equipement);
		}
	}
}
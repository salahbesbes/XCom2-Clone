using TMPro;
using UnityEngine;

public class uiTest : MonoBehaviour
{
	public UnitStats unit;

	public void updateHealth()
	{
		GetComponent<TextMeshProUGUI>().text = $"health: {unit.Health}";
	}

	public void updateArmor()
	{
		GetComponent<TextMeshProUGUI>().text = $"armor: {unit.armor.Value}";
	}

	public void updateDamage()
	{
		GetComponent<TextMeshProUGUI>().text = $"Damage: {unit.damage.Value}";
	}
}
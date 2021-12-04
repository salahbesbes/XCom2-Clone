using TMPro;
using UnityEngine;

public class uiTest : MonoBehaviour
{
	public UnitStats unit;

	public void updateUI()
	{
		GetComponent<TextMeshProUGUI>().text = $"health: {unit.Health}";
	}
}
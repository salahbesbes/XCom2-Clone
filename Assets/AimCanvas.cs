using TMPro;
using UnityEngine;

public class AimCanvas : MonoBehaviour
{
	private TextMeshProUGUI aimText;
	private TextMeshProUGUI dmgText;


	private void OnEnable()
	{
		aimText = transform.Find("pannel").Find("Aim").GetComponent<TextMeshProUGUI>();
		dmgText = transform.Find("pannel").Find("Damage").GetComponent<TextMeshProUGUI>();
		updatePannel();
	}
	private void Start()
	{
		aimText = transform.Find("pannel").Find("Aim").GetComponent<TextMeshProUGUI>();
		dmgText = transform.Find("pannel").Find("Damage").GetComponent<TextMeshProUGUI>();
	}

	public void updatePannel()
	{
		AnyClass thisUnit = GameStateManager.Instance.SelectedUnit;
		AnyClass target = GameStateManager.Instance.SelectedUnit.CurrentTarget;

		int dmg = thisUnit.stats.unit.damage.Value - target.stats.unit.armor.Value;

		dmg = Mathf.Clamp(dmg, 0, int.MaxValue);

		aimText.text = $"Aim: {thisUnit.TargetAimPercent}";
		dmgText.text = $"Dmg: {dmg}";
	}
}
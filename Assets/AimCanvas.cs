using TMPro;
using UnityEngine;

public class AimCanvas : MonoBehaviour
{
	private TextMeshProUGUI aimText;
	private TextMeshProUGUI dmgText;
	private TextMeshProUGUI criticalText;

	private void OnEnable()
	{
		aimText = transform.Find("pannel").Find("Aim").GetComponent<TextMeshProUGUI>();
		dmgText = transform.Find("pannel").Find("Damage").GetComponent<TextMeshProUGUI>();
		criticalText = transform.Find("pannel").Find("Critical").GetComponent<TextMeshProUGUI>();
		updatePannel();
	}

	private void Start()
	{
		aimText = transform.Find("pannel").Find("Aim").GetComponent<TextMeshProUGUI>();
		dmgText = transform.Find("pannel").Find("Damage").GetComponent<TextMeshProUGUI>();
	}

	public async void updatePannel()
	{
		AnyClass thisUnit = GameStateManager.Instance.SelectedUnit;

		int damage = thisUnit.stats.unit.damage.Value;

		AnyClass target = await thisUnit.getTarget();
		int criticalDamage = target.IsFluncked ? damage / 4 : 0;
		damage -= thisUnit.CurrentTarget.stats.unit.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);

		damage = Mathf.Clamp(damage, 0, int.MaxValue);

		aimText.text = $"Aim: {thisUnit.TargetAimPercent}";
		dmgText.text = $"Dmg: {damage}";
		criticalText.text = $"crit +{criticalDamage}";
	}
}
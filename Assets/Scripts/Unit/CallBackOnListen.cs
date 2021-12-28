using TMPro;
using UnityEngine;

public partial class CallBackOnListen : MonoBehaviour
{
	private GameStateManager manager;

	[Header("--------------------")]
	public TextMeshProUGUI myHealth;

	public TextMeshProUGUI MyName;
	public TextMeshProUGUI myArmor;
	public TextMeshProUGUI myDamage;

	[Header("--------------------")]
	public TextMeshProUGUI TargetHealth;

	public TextMeshProUGUI TargetName;
	public TextMeshProUGUI TargetArmor;
	public TextMeshProUGUI TargetDamage;

	[Header("--------------------")]
	public TextMeshProUGUI AimChance;

	private void Start()
	{
		manager = GameStateManager.Instance;
	}

	public void updateMyUiStats()
	{
		UnitStats thisUnit = manager.SelectedUnit.stats.unit;

		myHealth.text = $"Health : {thisUnit.Health}";
		MyName.text = $"{  thisUnit.myName }:";
		myArmor.text = $"Armor: { thisUnit.armor.Value}";
		myDamage.text = $"damage: { thisUnit.damage.Value}";
	}

	public void updateTargetUiStats()
	{
		UnitStats currentTargetSelected = manager.SelectedUnit.CurrentTarget.stats.unit;

		//Debug.Log($" target { manager.SelectedUnit.stats.unit.name}");
		TargetHealth.text = $"Health : { currentTargetSelected.Health}";
		TargetName.text = $"{  currentTargetSelected.myName }:";
		TargetArmor.text = $"Armor: { currentTargetSelected.armor.Value}";
		TargetDamage.text = $"damage: { currentTargetSelected.damage.Value}";
	}

	public void onEquipeEventTrigger(EquipementData equipement)
	{
		Debug.Log($"{ manager.SelectedUnit.stats.unit.name} equiped {equipement.name} and update  UI");
		updateMyUiStats();
	}

	public void onTargetChangeEventTrigger()
	{
		updateTargetUiStats();
		updateAimUi();
	}

	private void updateAimUi()
	{
		//thisUnit = GetComponentInParent<AnyClass>();
		//targetUiStats = GetComponentInParent<Stats>();
		//targetStats = unit;
		//Debug.Log($"  oncallback  {thisUnit.transform.name}");

		AimChance.text = $"Aim : {manager.SelectedUnit.TargetAimPercent} %";
	}

	public void onPlayerChangeEventTrigger()
	{
		//Debug.Log($"update player stas");
		updateMyUiStats();
	}

	public void onStatsChange()
	{
		updateMyUiStats();
	}
}
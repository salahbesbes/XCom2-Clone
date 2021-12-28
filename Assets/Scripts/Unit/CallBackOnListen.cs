using TMPro;
using UnityEngine;

public class CallBackOnListen : MonoBehaviour
{
	public GameStateManager manager;

	//private UnitStats targetStats;
	//private Stats targetUiStats;

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
	}

	public void TakeDamage(UnitStats triggerStats)
	{
		//Debug.Log($" trigger of event is  {triggerStats.name} has health  {triggerStats.Health}");
		int damage = triggerStats.damage.Value;
		damage -= manager.SelectedUnit.CurrentTarget.stats.unit.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);
		manager.SelectedUnit.CurrentTarget.stats.unit.Health -= damage;
		if (manager.SelectedUnit.CurrentTarget.stats.unit.Health <= 0)
		{
			Debug.Log($"{ manager.SelectedUnit.CurrentTarget.stats.unit.name} killed by {triggerStats.name}");
		}
	}

	public void updateMyUiStats()
	{
		myHealth.text = $"Health : {manager.SelectedUnit.stats.unit.Health}";
		MyName.text = $"{  manager.SelectedUnit.stats.unit.myName }:";
		myArmor.text = $"Armor: { manager.SelectedUnit.stats.unit.armor.Value}";
		myDamage.text = $"damage: { manager.SelectedUnit.stats.unit.damage.Value}";
	}

	public void updateTargetUiStats()
	{
		//Debug.Log($" target { manager.SelectedUnit.stats.unit.name}");
		TargetHealth.text = $"Health : { manager.SelectedUnit.CurrentTarget.stats.unit.Health}";
		TargetName.text = $"{  manager.SelectedUnit.CurrentTarget.stats.unit.myName }:";
		TargetArmor.text = $"Armor: { manager.SelectedUnit.CurrentTarget.stats.unit.armor.Value}";
		TargetDamage.text = $"damage: { manager.SelectedUnit.CurrentTarget.stats.unit.damage.Value}";
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

	public void onWeaponShootEventTrigger(UnitStats triggerStats)
	{
		TakeDamage(triggerStats);
		updateTargetUiStats();
	}
}
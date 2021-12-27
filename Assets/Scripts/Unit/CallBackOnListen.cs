using TMPro;
using UnityEngine;

public class CallBackOnListen : MonoBehaviour
{
	private UnitStats targetStats;
	private Stats targetUiStats;
	public AnyClass thisUnit;
	public GameStateManager manager;

	private void Start()
	{
	}

	public void TakeDamage(UnitStats triggerStats)
	{
		//Debug.Log($" trigger of event is  {triggerStats.name} has health  {triggerStats.Health}");
		int damage = triggerStats.damage.Value;
		damage -= targetStats.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);
		targetStats.Health -= damage;
		if (targetStats.Health <= 0)
		{
			Debug.Log($"{targetStats.name} killed by {triggerStats.name}");
		}
	}

	public void updateMyUiStats()
	{
		targetUiStats = GetComponentInParent<Stats>();
		targetStats = targetUiStats.unit;
		targetUiStats.myHealth.text = $"Health : {targetStats.Health}";
		targetUiStats.MyName.text = $"{ targetStats.myName }:";
		targetUiStats.myArmor.text = $"Armor: {targetStats.armor.Value}";
		targetUiStats.myDamage.text = $"damage: {targetStats.damage.Value}";
	}

	public void updateTargetUiStats()
	{
		targetUiStats = GetComponentInParent<Stats>();
		targetStats = targetUiStats.unit;
		targetUiStats.TargetHealth.text = $"Health : {targetStats.Health}";
		targetUiStats.TargetName.text = $"{ targetStats.myName }:";
		targetUiStats.TargetArmor.text = $"Armor: {targetStats.armor.Value}";
		targetUiStats.TargetDamage.text = $"damage: {targetStats.damage.Value}";
	}

	public void onEquipeEventTrigger(EquipementData equipement)
	{
		thisUnit = GetComponentInParent<AnyClass>();

		targetUiStats = GetComponentInParent<Stats>();
		Debug.Log($"{targetStats.name} equiped {equipement.name} and update  UI");
		updateMyUiStats();
	}

	public void onTargetChangeEventTrigger()
	{
		updateTargetUiStats();
		//updateAimUi();
	}

	private void updateAimUi()
	{
		//thisUnit = GetComponentInParent<AnyClass>();
		//targetUiStats = GetComponentInParent<Stats>();
		//targetStats = targetUiStats.unit;
		//Debug.Log($"  oncallback  {thisUnit.transform.name}");
		TextMeshProUGUI thisTextUi = GetComponent<TextMeshProUGUI>();
		thisTextUi.text = $"Aim = {manager.SelectedPlayer.TargetAimValue} %";
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
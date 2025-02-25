﻿using UnityEngine;

public class UnitCallBack : MonoBehaviour
{
	private GameStateManager manager;
	private PlayerStateManager thisUnit;

	private void Start()
	{
		manager = GameStateManager.Instance;
		thisUnit = GetComponentInParent<PlayerStateManager>();
	}

	public void TakeDamage(UnitStats triggerStats)
	{
		int damage = triggerStats.damage.Value;
		int criticalDamage = thisUnit.IsFluncked ? damage / 4 : 0;
		damage += criticalDamage;
		damage -= thisUnit.stats.unit.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);

		thisUnit.stats.unit.Health.Value -= damage;

		//Debug.Log($"{thisUnit.name}  was hit by {triggerStats.name} remain health {thisUnit.stats.unit.Health.Value}");
		//Debug.Log($"health after hit {thisUnit.stats.unit.Health}");
		if (thisUnit.stats.unit.Health.Value <= 0)
		{
			//Debug.Log($"{thisUnit.name} killed by {triggerStats.name}");

			thisUnit.SwitchState(thisUnit.dead);
			//thisUnit.model.GetComponent<Animator>().SetBool("idel", false);
			//thisUnit.model.GetComponent<Animator>().SetBool("die", true);
		}
	}

	public void onWeaponShootEventTrigger(UnitStats triggerStats)
	{
		TakeDamage(triggerStats);
		//updateTopCanvas();
		updateTargetHealthBar();
	}

	public void onGrenadeExplodes(UnitStats triggerStats)
	{
		TakeDamage(triggerStats);
		//updateTopCanvas();
		updateTargetHealthBar();
	}

	private void updateTopCanvas()
	{
		manager.SelectedUnit.onChangeTarget.Raise();
	}

	private void updateTargetHealthBar()
	{
		PlayerStateManager thisUnit = GetComponentInParent<PlayerStateManager>();
		manager.MakeOnlySelectedUnitListingToEventArgument(thisUnit, manager.SelectedUnit.stats.onStatsChange);
		manager.SelectedUnit.stats.onStatsChange.Raise();
		manager.clearPreviousSelectedUnitFromAllVoidEvents(thisUnit);
	}

	private void updateMytHealthBar()
	{
		PlayerStateManager thisUnit = GetComponentInParent<PlayerStateManager>();
		manager.MakeOnlySelectedUnitListingToEventArgument(thisUnit, manager.SelectedUnit.stats.onStatsChange);
		manager.SelectedUnit.stats.onStatsChange.Raise();
		manager.clearPreviousSelectedUnitFromAllVoidEvents(thisUnit);
	}

	public void onUnitFlunked(bool parm)
	{
		PlayerStateManager thisUnit = GetComponentInParent<PlayerStateManager>();
		Debug.Log($" {thisUnit} is fluncked by {GameStateManager.Instance.SelectedUnit} {parm} ");
	}

	public void onEquipeEventTrigger(EquipementData equipement)
	{
		//PlayerStateManager thisUnit = GetComponentInParent<PlayerStateManager>();
		Debug.Log($"we modifie stats");
		thisUnit.stats.addArmorModifier(equipement);
		//updateMytHealthBar();
	}
}
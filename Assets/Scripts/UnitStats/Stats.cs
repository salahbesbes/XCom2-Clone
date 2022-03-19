using gameEventNameSpace;
using UnityEngine;

public class Stats : MonoBehaviour
{
	public UnitStats unit;

	public WeaponEvent onWeaponFinishShooting;
	public StatsChangeEvent onStatsChange;
	public BoolEvent FlunckingTarget;

	private void Start()
	{
	}

	public void addArmorModifier(EquipementData equiment)
	{
		unit.armor.AddModifier(equiment.Value);
	}

	public void addDamageModifier(EquipementData equiment)
	{
		unit.damage.AddModifier(equiment.Value);
	}

	public void addHealthModifier(EquipementData equiment)
	{
		unit.Health.AddModifier(equiment.Value);
	}
}
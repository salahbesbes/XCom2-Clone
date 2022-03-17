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
		unit.EquipeEvent.Raise(equiment);
	}

	public void addDamageModifier()
	{
		if (unit.damage.modifiers.Count != 0)

		{
			unit.damage.AddModifier(unit.damage.modifiers[unit.damage.modifiers.Count - 1] + 2);
		}
		else
		{
			unit.damage.AddModifier(10);
		}
	}

	public void heal(int healValue)
	{
		unit.Health = Mathf.Clamp(unit.Health += healValue, 0, int.MaxValue);
	}
}
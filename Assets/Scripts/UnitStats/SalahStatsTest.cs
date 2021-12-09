using UnityEngine;

public class SalahStatsTest : MonoBehaviour
{
	public UnitStats unit;

	private void Start()
	{
	}

	public void triggerEvent()
	{
		unit.eventToListnTo.Raise(unit);
	}

	public void Die()
	{
		Debug.Log($"player died");
	}

	public void addArmorModifier()
	{
		if (unit.damage.modifiers.Count != 0)

		{
			unit.armor.AddModifier(unit.damage.modifiers[unit.damage.modifiers.Count - 1] + 2);
		}
		else
		{
			unit.armor.AddModifier(7);
		}
		triggerEvent();
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
		triggerEvent();
	}

	public void heal(int healValue)
	{
		unit.Health = Mathf.Clamp(unit.Health += healValue, 0, int.MaxValue);
		triggerEvent();
	}
}
using UnityEngine;

public class SalahStatsTest : MonoBehaviour
{
	public UnitStats unit;

	public void triggerEvent()
	{
		unit.eventToListnTo.Raise();
	}

	public void TakeDamage(int damage)
	{
		damage -= unit.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);
		Debug.Log($"unit.armor is {unit.armor.Value}");
		Debug.Log($"damage is {damage}");
		Debug.Log($"{unit.Health}");
		unit.Health -= damage;
		Debug.Log($"{unit.Health}");
		if (unit.Health <= 0)
		{
			Die();
		}
		triggerEvent();
	}

	public void Die()
	{
		Debug.Log($"player died");
	}
}
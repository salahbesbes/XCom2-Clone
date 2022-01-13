using UnityEngine;

public class UnitCallBack : MonoBehaviour
{
	private GameStateManager manager;

	private void Start()
	{
		manager = GameStateManager.Instance;
	}

	public void TakeDamage(UnitStats triggerStats)
	{

		AnyClass thisUnit = GetComponentInParent<AnyClass>();
		Debug.Log($" trigger of event is  {triggerStats.name} target is   {thisUnit.stats.unit.name}");
		int damage = triggerStats.damage.Value;
		Debug.Log($"trigger damage {damage}");
		damage -= thisUnit.stats.unit.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);
		Debug.Log($"after hit {damage}");
		Debug.Log($"target health {thisUnit.stats.unit.Health}");

		thisUnit.stats.unit.Health -= damage;

		Debug.Log($"health after hit {thisUnit.stats.unit.Health}");

		if (manager.SelectedUnit.CurrentTarget.stats.unit.Health <= 0)
		{
			Debug.Log($"{ thisUnit.stats.unit.name} killed by {triggerStats.name}");
		}
	}

	public void onWeaponShootEventTrigger(UnitStats triggerStats)
	{
		TakeDamage(triggerStats);
		manager.SelectedUnit.onChangeTarget.Raise();
	}
}
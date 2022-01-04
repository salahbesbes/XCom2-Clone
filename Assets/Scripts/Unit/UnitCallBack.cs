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
		//Debug.Log($" trigger of event is  {triggerStats.name} has health  {triggerStats.Health}");
		int damage = triggerStats.damage.Value;
		Debug.Log($"trigger damage {damage}");
		damage -= manager.SelectedUnit.CurrentTarget.stats.unit.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);
		Debug.Log($"after hit {damage}");
		Debug.Log($"target health {manager.SelectedUnit.CurrentTarget.stats.unit.Health}");

		manager.SelectedUnit.CurrentTarget.stats.unit.Health -= damage;

		Debug.Log($"health after hit {manager.SelectedUnit.CurrentTarget.stats.unit.Health}");

		if (manager.SelectedUnit.CurrentTarget.stats.unit.Health <= 0)
		{
			Debug.Log($"{ manager.SelectedUnit.CurrentTarget.stats.unit.name} killed by {triggerStats.name}");
		}
	}

	public void onWeaponShootEventTrigger(UnitStats triggerStats)
	{
		TakeDamage(triggerStats);
		manager.SelectedUnit.onChangeTarget.Raise();
	}
}
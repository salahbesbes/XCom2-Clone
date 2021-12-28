using UnityEngine;

public class UnitCallBack : MonoBehaviour
{
	public GameStateManager manager;

	private void Start()
	{
		manager = GameStateManager.Instance;
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

	public void onWeaponShootEventTrigger(UnitStats triggerStats)
	{
		TakeDamage(triggerStats);
		updateTargetUiStats();
	}
}
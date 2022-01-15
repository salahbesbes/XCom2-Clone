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

		PlayerStateManager thisUnit = GetComponentInParent<PlayerStateManager>();
		int damage = triggerStats.damage.Value;
		damage -= thisUnit.stats.unit.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);

		thisUnit.stats.unit.Health -= damage;

		Debug.Log($"health after hit {thisUnit.stats.unit.Health}");
		if (thisUnit.stats.unit.Health <= 0)
		{
			Debug.Log($"{ thisUnit.stats.unit.name} killed by {triggerStats.name}");

			thisUnit.SwitchState(thisUnit.dead);
			//thisUnit.model.GetComponent<Animator>().SetBool("idel", false);
			//thisUnit.model.GetComponent<Animator>().SetBool("die", true);


		}
	}

	public void onWeaponShootEventTrigger(UnitStats triggerStats)
	{
		TakeDamage(triggerStats);
		manager.SelectedUnit.onChangeTarget.Raise();
	}
}
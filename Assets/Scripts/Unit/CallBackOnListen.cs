using UnityEngine;

public class CallBackOnListen : MonoBehaviour
{
	private UnitStats myStats;
	private Stats myUiStats;
	public AnyClass thisUnit;

	private void Start()
	{
		myUiStats = GetComponentInParent<Stats>();
		myStats = GetComponentInParent<Stats>().unit;
		//thisUnit = GetComponentInParent<AnyClass>();
	}

	public void TakeDamage(UnitStats triggerStats)
	{
		//Debug.Log($" trigger of event is  {triggerStats.name} has health  {triggerStats.Health}");
		int damage = triggerStats.damage.Value;
		damage -= myStats.armor.Value;
		damage = Mathf.Clamp(damage, 0, int.MaxValue);
		myStats.Health -= damage;
		if (myStats.Health <= 0)
		{
			Debug.Log($"{myStats.name} killed by {triggerStats.name}");
		}
	}

	public void updateMyUiStats()
	{
		myUiStats = GetComponentInParent<Stats>();
		myStats = myUiStats.unit;
		myUiStats.myHealth.text = $"Health : {myStats.Health}";
		myUiStats.MyName.text = $"{ myStats.myName }:";
		myUiStats.myArmor.text = $"Armor: {myStats.armor.Value}";
		myUiStats.myDamage.text = $"damage: {myStats.damage.Value}";
	}

	public void updateTargetUiStats()
	{

		myUiStats = GetComponentInParent<Stats>();
		myStats = myUiStats.unit;
		myUiStats.TargetHealth.text = $"Health : {myStats.Health}";
		myUiStats.TargetName.text = $"{ myStats.myName }:";
		myUiStats.TargetArmor.text = $"Armor: {myStats.armor.Value}";
		myUiStats.TargetDamage.text = $"damage: {myStats.damage.Value}";
	}

	public void onTargetChangeEventTrigger()
	{
		updateTargetUiStats();
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
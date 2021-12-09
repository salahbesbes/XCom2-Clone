using TMPro;
using UnityEngine;

namespace gameEventNameSpace

{
	public class WeaponListner : BaseGameEventListner<UnitStats, WeaponEvent, UnityWeaponEvent>
	{
		private UnitStats myStats;
		public TextMeshProUGUI UiText;
		public TextMeshProUGUI MyName;

		public void Start()
		{
			myStats = GetComponent<SalahStatsTest>().unit;
		}

		public void TakeDamage(UnitStats triggerStats)
		{
			Debug.Log($" trigger of event is  {triggerStats.name} has health  {triggerStats.Health}");
			int damage = triggerStats.damage.Value;
			damage -= myStats.armor.Value;
			damage = Mathf.Clamp(damage, 0, int.MaxValue);
			myStats.Health -= damage;
			if (myStats.Health <= 0)
			{
				Debug.Log($"{myStats.name} killed by {triggerStats.name}");
			}
		}

		public void updateUiText()
		{
			Debug.Log($" updateui called ");
			UiText.text = $"Health : {myStats.Health}";
			MyName.text = $"{ myStats.myName }:";
		}

		public void whenThisInstanceIsSelectedAsTarget()
		{
			updateUiText();
		}

		public void executeCallback(UnitStats triggerStats)
		{
			TakeDamage(triggerStats);
			updateUiText();
		}
	}
}
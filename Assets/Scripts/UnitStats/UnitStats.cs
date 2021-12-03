using UnityEngine;

public class UnitStats : MonoBehaviour
{
	private int maxHealth = 100;
	private int _health;

	public int Health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = Mathf.Clamp(value, 0, maxHealth);
		}
	}

	public Stat damage;
	public Stat armor;

	private void Start()
	{
		Health = maxHealth;
		armor.Value = 0;
	}

	private void TakeDamage(int damage)
	{
		damage -= armor.Value;
		Debug.Log($"armor is {armor.Value}");
		Debug.Log($"damage is {damage}");
		Health -= damage;
		Debug.Log($"{Health}");
		if (Health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		Debug.Log($"player died");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			TakeDamage(10);
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			armor.AddModifier(2);
		}
	}
}
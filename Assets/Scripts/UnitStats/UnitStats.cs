using gameEventNameSpace;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "unit Stats")]
public class UnitStats : ScriptableObject
{
	public string myName;
	private int maxHealth = 100;
	private int _health;
	public WeaponEvent ShootActionEvent;
	public EquipementEvent EquipeEvent;
	public Weapon weapon;

	private void Reset()
	{
		//Output the message to the Console
		//Debug.Log("Reset");
		Health = maxHealth;

		//eventToListnTo = FindObjectOfType<VoidEvent>();

		armor.modifiers.Clear();
		damage.modifiers.Clear();
	}

	private void Awake()
	{
		Health = maxHealth;
		//Debug.Log($"awake called");
	}

	private void OnEnable()
	{
		Health = maxHealth;
		armor.modifiers.Clear();
		damage.modifiers.Clear();
		//Debug.Log($"enabled");
	}

	private void OnDisable()
	{
		//Debug.Log($"disabled");
	}

	private void OnDestroy()
	{
		//Debug.Log($"destroyed");
	}

	private void OnValidate()
	{
		//armor.Value = 5;
		//Debug.Log($"validate");
	}

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
}
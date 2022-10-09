using gameEventNameSpace;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "unit Stats")]
public class UnitStats : ScriptableObject
{
	public string myName;

	[SerializeField]
	private Stat _health;
	public EquipementEvent EquipeEvent;
	public Weapon weapon;
	public Stat damage;
	public Stat armor;

	[SerializeField]
	private int _maxHealth = 100;

	public int maxHealth
	{ get => _maxHealth; private set { } }

	public Stat Health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = value;
			_health.Value = Mathf.Clamp(value.Value, 0, maxHealth);
		}
	}

	[SerializeField]
	public int _maxActionPoint = 2;
	private int _ActionPoint = 2;
	public int ActionPoint { get => _ActionPoint; set => _ActionPoint = Mathf.Clamp(value, 0, int.MaxValue); }



	public void Reset()
	{
		ActionPoint = _maxActionPoint;
		Health.Value = _maxHealth;
		armor.modifiers.Clear();
		damage.modifiers.Clear();
		Health.modifiers.Clear();
	}

	private void Awake()
	{
		Health.Value = _maxHealth;
		//Debug.Log($"awake called");
	}

	private void OnEnable()
	{
		Reset();
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


	public void resetActionPoints()
	{
		ActionPoint = _maxActionPoint;
	}



}
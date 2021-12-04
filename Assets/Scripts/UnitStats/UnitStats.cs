using gameEventNameSpace;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "unit Stats")]
public class UnitStats : ScriptableObject
{
	private int maxHealth = 100;
	private int _health;
	public VoidEvent eventToListnTo;

	private void Reset()
	{
		//Output the message to the Console
		Debug.Log("Reset");
		Health = maxHealth;

		eventToListnTo = FindObjectOfType<VoidEvent>();
		Debug.Log($"{eventToListnTo}");
	}

	private void Awake()
	{
		Health = maxHealth;
		armor.Value = 0;
		Debug.Log($"awake called");
	}

	private void OnEnable()
	{
		Debug.Log($"enabled");
	}

	private void OnDisable()
	{
		Debug.Log($"disabled");
	}

	private void OnDestroy()
	{
		Debug.Log($"destroyed");
	}

	private void OnValidate()
	{
		armor.Value = 5;
		Debug.Log($"validate");
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
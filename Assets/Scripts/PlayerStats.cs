using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	[SerializeField]
	private float _Health;

	public float Health
	{
		get { return _Health; }
		set
		{
			_Health = Mathf.Clamp(value, 0, MaxHealth);
			eventListner.Invoke(nameof(Health));
		}
	}

	[SerializeField]
	public float _MoveRange = 10;

	public float MoveRange
	{
		get { return _MoveRange; }
		set
		{
			_MoveRange = value;
			eventListner.Invoke(nameof(MoveRange));
		}
	}

	[SerializeField]
	private float _MoveVision = 20;

	public float MoveVision
	{
		get { return _MoveVision; }
		set
		{
			_MoveVision = value;
			eventListner.Invoke(nameof(MoveVision));
		}
	}

	[SerializeField]
	public int _ActionPoint = 2;

	public int ActionPoint
	{
		get { return _ActionPoint; }
		set
		{
			_ActionPoint = value;
			eventListner.Invoke(nameof(ActionPoint));
		}
	}

	public Transform healthBar;
	public GameObject Textprefab;
	public float MaxHealth = 20;

	//private Dictionary<string, dynamic> dictProperties = new Dictionary<string, dynamic>()

	public static event Action<string> eventListner = delegate { };

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			//List<object> localprops = new List<object>() { Health, MoveRange, MoveVision, ActionPoint };
			Health -= 2;
			MoveVision++;
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			//List<object> localprops = new List<object>() { Health, MoveRange, MoveVision, ActionPoint };
			Health += 2;
			MoveVision++;
		}
	}

	private void OnEnable()
	{
		// subscribe the observable to the this subscriaber
		healthBar.GetComponent<ObserverAbstraction>().Subsribe(this);
	}

	public void Start()
	{
		Health = MaxHealth;
		ActionPoint = ActionPoint;
	}

	public override string ToString()
	{
		return String.Join(", ", $"Health: {Health}", $"MoveRange: {MoveRange}", $"MoveVision: {MoveVision}", $"ActionPoint: {ActionPoint}");
	}
}
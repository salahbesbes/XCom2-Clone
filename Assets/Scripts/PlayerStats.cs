using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour, ISubject<PlayerStats, PlayerStats>
{
	[SerializeField]
	private float _Health;

	public float Health
	{
		get { return _Health; }
		set
		{
			_Health = Mathf.Clamp(value, 0, MaxHealth);
			EventListner.Invoke(this);
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
			EventListner.Invoke(this);
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
			EventListner.Invoke(this);
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
			EventListner.Invoke(this);
		}
	}

	public Action<PlayerStats> EventListner { get; set; }

	public Transform healthBar;
	public GameObject Textprefab;
	public float MaxHealth = 20;

	//private Dictionary<string, dynamic> dictProperties = new Dictionary<string, dynamic>()

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
		healthBar.GetComponent<ObserverAbstraction<PlayerStats, PlayerStats>>().Subsribe(this);
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
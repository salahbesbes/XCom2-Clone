using UnityEngine;
using UnityEngine.Events;

public class PlayerEventListener : MonoBehaviour
{

	[SerializeField]
	public PlayerAction GameEvent;


	[SerializeField]
	public UnityEvent response;

	private void OnEnable()
	{
		if (GameEvent == null) return;
		GameEvent.RegisterListener(this);
	}

	public void register()
	{
		if (GameEvent == null) return;
		GameEvent.RegisterListener(this);

	}

	private void OnDisable()
	{
		if (GameEvent == null) return;

		GameEvent.UnregisterListener(this);
	}
	private void OnDestroy()
	{
		if (GameEvent == null) return;
		GameEvent.UnregisterListener(this);

	}
	public void OnEventRaised()
	{
		if (response == null)
			Debug.Log($"response is null suppose to crash the game");
		response?.Invoke();
	}
}
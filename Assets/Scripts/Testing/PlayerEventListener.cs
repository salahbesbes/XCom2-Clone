using UnityEngine;
using UnityEngine.Events;

public class PlayerEventListener : MonoBehaviour
{

	[SerializeField]
	public PlayerEvent GameEvent;


	[SerializeField]
	public UnityEvent response;

	//private void OnEnable()
	//{
	//	GameEvent?.RegisterListener(this);
	//}

	public void register()
	{
		GameEvent.RegisterListener(this);

	}

	private void OnDisable()
	{
		GameEvent.UnregisterListener(this);
	}

	public void OnEventRaised()
	{
		if (response == null)
			Debug.Log($"response is null suppose to crash the game");
		response?.Invoke();
	}
}
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
	//	gameEvent.RegisterListener(this);
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
		response.Invoke();
	}
}
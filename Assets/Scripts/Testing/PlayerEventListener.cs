using UnityEngine;
using UnityEngine.Events;

public class PlayerEventListener : MonoBehaviour
{
	[SerializeField]
	private PlayerEvent gameEvent;

	[SerializeField]
	private UnityEvent response;

	private void OnEnable()
	{
		gameEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		gameEvent.UnregisterListener(this);
	}

	public void OnEventRaised()
	{
		response.Invoke();
	}
}
using UnityEngine;
using UnityEngine.Events;

public class GameManagerListner : MonoBehaviour
{
	private void clearPreviousPlayerListners()
	{
		PlayerEventListener[] listners = gameObject.GetComponents<PlayerEventListener>();
		foreach (PlayerEventListener listner in listners)
		{
			Destroy(listner);
		}
	}

	public void UpdateSelectedPlayerResponse(PlayerClass unit)
	{
		clearPreviousPlayerListners();
		foreach (ActionData action in unit.actions)
		{
			PlayerEvent playerEvent = action.Actionevent;
			PlayerEventListener e = gameObject.AddComponent<PlayerEventListener>();
			e.GameEvent = playerEvent;
			e.response = new UnityEvent();
			e.register();
			if (unit == null) return;
			if (playerEvent is MovementEvent)
			{
				e.response.AddListener(unit.CreateNewMoveAction);
			}
			if (playerEvent is ShootEvent)
			{
				e.response.AddListener(unit.CreateNewShootAction);
			}
		}
	}
}
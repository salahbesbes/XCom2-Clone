using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : ScriptableObject
{

	private List<PlayerEventListener> listeners = new List<PlayerEventListener>();

	public void Raise()
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventRaised();
		}
	}

	public void RegisterListener(PlayerEventListener listener)
	{
		listeners.Add(listener);
	}

	public void UnregisterListener(PlayerEventListener listener)
	{
		listeners.Remove(listener);
	}
}

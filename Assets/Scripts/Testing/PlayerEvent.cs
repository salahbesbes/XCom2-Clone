using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Action Event", menuName = "ActionEvent"), System.Serializable]
public class PlayerEvent : ScriptableObject
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
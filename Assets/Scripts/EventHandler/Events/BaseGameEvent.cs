using System.Collections.Generic;
using UnityEngine;

namespace gameEventNameSpace

{
	public abstract class BaseGameEvent<T> : ScriptableObject
	{
		private readonly List<IGameEventListner<T>> eventListner = new List<IGameEventListner<T>>();

		public void Raise(T item)
		{
			for (int i = eventListner.Count - 1; i >= 0; i--)
			{
				eventListner[i].OnEventRase(item);
			}
		}

		public void RegisterListner(IGameEventListner<T> listner)
		{
			if (!eventListner.Contains(listner))
			{
				eventListner.Add(listner);
			}
		}

		public void UnRegisterListner(IGameEventListner<T> listner)
		{
			if (eventListner.Contains(listner))
			{
				eventListner.Remove(listner);
			}
		}
	}
}
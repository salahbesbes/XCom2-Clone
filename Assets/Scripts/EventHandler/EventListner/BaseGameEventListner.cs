using UnityEngine;
using UnityEngine.Events;

namespace gameEventNameSpace

{
	public abstract class BaseGameEventListner<T, E, UER> : MonoBehaviour, IGameEventListner<T>
		where E : BaseGameEvent<T> where UER : UnityEvent<T>
	{
		[SerializeField] private E _gameEvent;
		public E GameEvent { get => _gameEvent; set => _gameEvent = value; }

		[SerializeField] private UER unityEventResponse;

		private void OnEnable()
		{
			if (_gameEvent == null) return;

			GameEvent.RegisterListner(this);
		}

		private void OnDisable()
		{
			if (_gameEvent == null) return;

			GameEvent.UnRegisterListner(this);
		}

		public void OnEventRase(T item)
		{
			if (unityEventResponse != null)
			{
				unityEventResponse.Invoke(item);
			}
		}
	}

	[System.Serializable] public class Void { }
}
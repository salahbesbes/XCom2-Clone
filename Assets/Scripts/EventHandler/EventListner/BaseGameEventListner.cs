using UnityEngine;
using UnityEngine.Events;

namespace gameEventNameSpace

{
	public abstract class BaseGameEventListner<T, E, UER> : MonoBehaviour, IGameEventListner<T>
		where E : BaseGameEvent<T> where UER : UnityEvent<T>
	{
		[SerializeField] private E _gameEvent;
		public E GameEvent { get => _gameEvent; set => _gameEvent = value; }
		[SerializeField]
		private UER _unityEventResponse;

		public UER UnityEventResponse { get => _unityEventResponse; set => _unityEventResponse = value; }


		public void OnEnable()
		{

			if (GameEvent == null) return;

			GameEvent.RegisterListner(this);
		}

		private void OnDisable()
		{
			if (GameEvent == null) return;

			GameEvent.UnRegisterListner(this);
		}
		private void OnDestroy()
		{
			if (GameEvent == null) return;

			GameEvent.UnRegisterListner(this);
		}



		public void Register()
		{

			if (GameEvent == null) return;

			GameEvent.RegisterListner(this);
		}

		public void OnEventRase(T item)
		{

			if (UnityEventResponse != null)
			{
				UnityEventResponse.Invoke(item);
			}
		}
	}

	[System.Serializable] public class Void { }
}
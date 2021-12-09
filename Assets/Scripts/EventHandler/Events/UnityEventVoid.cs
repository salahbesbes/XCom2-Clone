using UnityEngine.Events;

namespace gameEventNameSpace
{
	[System.Serializable]
	public class UnityVoidEvent : UnityEvent<Void>
	{
	}

	[System.Serializable]
	public class UnityIntEvent : UnityEvent<int>
	{
	}

	[System.Serializable]
	public class UnityStateEvent : UnityEvent<BaseState<GameStateManager>>
	{
	}

	[System.Serializable]
	public class UnityWeaponEvent : UnityEvent<UnitStats>
	{
	}
}
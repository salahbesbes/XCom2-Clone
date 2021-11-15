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
}
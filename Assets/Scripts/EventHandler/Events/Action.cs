using UnityEngine;
using UnityEngine.Events;

namespace gameEventNameSpace

{
	public class BaseAction
	{
	}

	public class Color : BaseAction
	{
	}

	[CreateAssetMenu(fileName = "new color Event ", menuName = "Game Event / color Event")]
	public class ColorEvent : BaseGameEvent<Color>
	{
		public void Raise() => Raise(new Color());
	}

	[System.Serializable]
	public class UnityColorEvent : UnityEvent<Color>
	{
	}

	public class ColorListner : BaseGameEventListner<Color, ColorEvent, UnityColorEvent>
	{
	}
}
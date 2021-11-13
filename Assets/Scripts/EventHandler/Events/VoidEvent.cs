using UnityEngine;

namespace gameEventNameSpace
{
	[CreateAssetMenu(fileName = "new Void Event ", menuName = "Game Event / Void Event")]
	public class VoidEvent : BaseGameEvent<Void>
	{
		public void Raise() => Raise(new Void());
	}
}
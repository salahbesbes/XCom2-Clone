using UnityEngine;

namespace gameEventNameSpace
{

	[CreateAssetMenu(fileName = "new void Event ", menuName = "Game Event / void Event ")]

	public class VoidEvent : BaseGameEvent<Void>
	{
		public void Raise() => Raise(new Void());
	}
}
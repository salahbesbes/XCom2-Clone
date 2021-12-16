namespace gameEventNameSpace
{
	public class VoidEvent : BaseGameEvent<Void>
	{
		public void Raise() => Raise(new Void());
	}
}
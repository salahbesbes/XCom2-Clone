namespace gameEventNameSpace

{
	public interface IGameEventListner<T>
	{
		public void OnEventRase(T item);
	}
}
public abstract class BaseState<T>
{
	public string name { get; set; }
	public abstract void EnterState(T Context);

	public abstract void Update(T Context);

	public abstract void ExitState(T Context);

	public override string ToString()
	{
		return $"{GetType().Name}";
	}
}

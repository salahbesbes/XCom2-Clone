public class PlayerStateManager : Unit
{
	public SelectingEnemy selectingEnemy = new SelectingEnemy();
	public Idel idelState = new Idel();
	public DoingAction doingAction = new DoingAction();
	public Dead dead = new Dead();

	public AnimationType currentActionAnimation = AnimationType.idel;

	//private void Awake()
	//{
	//	//unit = GetComponent<AnyClass>();
	//	SwitchState(idelState);

	// //Debug.Log($"start of player state manager ");

	//}
	public void OnEnable()
	{
	}
	public void IsDead()
	{
		gameStateManager.players.Remove(this);
		gameStateManager.enemies.Remove(this);
	}


	private BaseState<PlayerStateManager> _State;

	public BaseState<PlayerStateManager> State
	{
		get => _State;
		private set
		{
			_State = value;
		}
	}

	private void Awake()
	{
		SwitchState(idelState);
	}

	private void Update()
	{
		//currentPos = grid.getNodeFromTransformPosition(transform);
		State.Update(this);
		//customUpdate();
	}

	public virtual void customUpdate()
	{
	}

	public void SwitchState(BaseState<PlayerStateManager> newState)
	{
		State?.ExitState(this);
		State = newState;
		State.EnterState(this);
	}
}
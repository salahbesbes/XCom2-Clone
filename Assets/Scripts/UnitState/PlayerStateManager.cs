using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
	public SelectingEnemy selectingEnemy = new SelectingEnemy();
	public Idel idelState = new Idel();
	public DoingAction doingAction = new DoingAction();

	private BaseState<PlayerStateManager> _State;

	public BaseState<PlayerStateManager> State
	{
		get => _State;
		private set
		{
			_State = value;
		}
	}

	private void OnEnable()
	{
		//grid = FindObjectOfType<NodeGrid>();
		//currentPos = grid.getNodeFromTransformPosition(transform);
	}

	private void Awake()
	{
		State = idelState;
		State.EnterState(this);
	}

	private void Update()
	{
		//currentPos = grid.getNodeFromTransformPosition(transform);
		State.Update(this);
	}

	public void SwitchState(BaseState<PlayerStateManager> newState)
	{
		State?.ExitState(this);

		State = newState;
		State.EnterState(this);
	}
}
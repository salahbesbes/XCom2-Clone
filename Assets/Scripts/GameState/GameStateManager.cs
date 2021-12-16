using gameEventNameSpace;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : GameManagerListner
{
	// Current state of the player, this script is attached to the object of interest the player
	// object is accessible in this class and other children

	// this is the Subject and it have some Observers

	// initialise 4 stat of the Player 4 instatnce that lives in this class
	public PlayerTurn playerTurn = new PlayerTurn();

	public EnemyTurn enemyTurn = new EnemyTurn();
	private BaseState<GameStateManager> _State;

	public BaseState<GameStateManager> State
	{
		get => _State;
		private set
		{
			_State?.ExitState(this);
			_State = value;
			StateEventSubject.Raise(_State);
		}
	}

	[SerializeField]
	public List<Enemy> enemies;

	private Enemy _selectedEnemy;

	public Enemy SelectedEnemy
	{
		get => _selectedEnemy; set
		{
			_selectedEnemy?.SwitchState(_selectedEnemy?.idelState);
			clearPreviousSelectedUnitFromAllVoidEvents(_selectedEnemy);
			clearPreviousSelectedUnitFromAllWeaponEvent(_selectedEnemy);
			_selectedEnemy = value;
			if (State is PlayerTurn)
			{
				MakeOnlySelectedUnitListingToEventArgument(_selectedEnemy, SelectedPlayer?.onChangeTarget);
				MakeOnlySelectedUnitListingToWeaponEvent(_selectedEnemy, SelectedPlayer?.GetComponent<Stats>()?.unit?.eventToListnTo);
			}
			else if (State is EnemyTurn)
			{
				MakeGAmeMAnagerListingToNewSelectedUnit(_selectedEnemy);
				MakeOnlySelectedUnitListingToEventArgument(_selectedEnemy, PlayerChangeEvent);
			}
		}
	}

	[SerializeField]
	public List<Player> players;

	private Player _selectedPlayer;

	public BaseStateEvent StateEventSubject;
	public VoidEvent PlayerChangeEvent;

	public Player SelectedPlayer
	{
		get => _selectedPlayer; set
		{
			// every time game manager want to switch player update the old selected one
			// to idel state
			_selectedPlayer?.SwitchState(_selectedPlayer?.idelState);
			clearPreviousSelectedUnitFromAllVoidEvents(_selectedPlayer);
			clearPreviousSelectedUnitFromAllWeaponEvent(_selectedPlayer);
			_selectedPlayer = value;
			if (State is PlayerTurn)
			{
				MakeGAmeMAnagerListingToNewSelectedUnit(_selectedPlayer);
				MakeOnlySelectedUnitListingToEventArgument(_selectedPlayer, PlayerChangeEvent);
			}
			else if (State is EnemyTurn)
			{

				MakeOnlySelectedUnitListingToWeaponEvent(_selectedPlayer, SelectedEnemy?.GetComponent<Stats>()?.unit?.eventToListnTo);
				MakeOnlySelectedUnitListingToEventArgument(_selectedPlayer, SelectedEnemy?.onChangeTarget);
			}
		}
	}

	[HideInInspector]
	public NodeGrid grid;

	private void OnEnable()
	{
	}

	private void Awake()
	{
		SwitchState(enemyTurn);
		grid = FindObjectOfType<NodeGrid>();
	}

	private void Start()
	{
		if (State is PlayerTurn)
		{
			PlayerChangeEvent.Raise();
			SelectedPlayer.onChangeTarget.Raise();
		}
		else if (State is EnemyTurn)
		{
			PlayerChangeEvent.Raise();
			SelectedEnemy.onChangeTarget.Raise();
		}
	}

	private void Update()
	{
		// for any state the player is in, we execute the update methode of that State
		// change of the state is instant since this update executs every frame
		State.Update(this);
	}

	public void SwitchState(BaseState<GameStateManager> newState)
	{
		// change the current state and execute the start methode of that new State this is
		// the only way to change the state
		State = newState;
		AnyClass selectedUnit = State.EnterState(this);
	}

	public void ChangeState()
	{
		if (State == playerTurn) SwitchState(enemyTurn);
		else if (State == enemyTurn) SwitchState(playerTurn);
	}
}

public abstract class BaseState<T>
{
	public string name;

	public abstract AnyClass EnterState(T playerContext);

	public abstract void Update(T playerContext);

	public abstract void ExitState(T playerContext);

	public override string ToString()
	{
		return $"{GetType().Name}";
	}
}

// this class inhirit from PlayerBaseState Class the role of this class is to share (execute) code
// that all state execute it
public abstract class AnyState<T> : BaseState<T>
{
	// if we want to execute code in all states in the update methode

	public AnyState()
	{
		name = GetType().Name;
	}
}
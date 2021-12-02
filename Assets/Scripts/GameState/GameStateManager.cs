using gameEventNameSpace;
using System;
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

	public Action<BaseState<GameStateManager>> EventListner { get; set; }

	[SerializeField]
	public List<Enemy> enemies;

	private Enemy _selectedEnemy;

	public Enemy SelectedEnemy
	{
		get => _selectedEnemy; set
		{
			_selectedEnemy = value;

			if (State is EnemyTurn)
				_selectedEnemy.updatePlayerActionUi();
		}
	}

	[SerializeField]
	public List<Player> players;

	private Player _selectedPlayer;

	public BaseStateEvent StateEventSubject;

	public Player SelectedPlayer
	{
		get => _selectedPlayer; set
		{
			_selectedPlayer = value;
			if (State is PlayerTurn)
				_selectedPlayer.updatePlayerActionUi();
		}
	}

	[HideInInspector]
	public NodeGrid grid;

	private void OnEnable()
	{
	}

	private void Awake()
	{
		SwitchState(playerTurn);
		grid = FindObjectOfType<NodeGrid>();
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

		//EventListner.Invoke(State);
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
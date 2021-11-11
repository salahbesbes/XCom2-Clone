using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISubject<T, W>
{
	public Action<W> EventListner { get; set; }
}

public class GameStateManager : MonoBehaviour, ISubject<GameStateManager, GameBaseState>
{
	// Current state of the player, this script is attached to the object of interest the player
	// object is accessible in this class and other children

	// this is the Subject and it have some Observers

	// initialise 4 stat of the Player 4 instatnce that lives in this class
	public PlayerTurn playerTurn = new PlayerTurn();
	public EnemyTurn enemyTurn = new EnemyTurn();
	public ObserverAbstraction<GameStateManager, GameBaseState> stateObserverText;
	private GameBaseState _State;

	public GameBaseState State
	{
		get => _State;
		private set
		{
			_State?.ExitState(this);
			_State = value;
		}
	}

	public Action<GameBaseState> EventListner { get; set; }

	[SerializeField]
	public List<Enemy> enemies;
	public Enemy selectedEnemy;

	[SerializeField]
	public List<Player> players;
	public Player selectedPlayer;

	private void OnEnable()
	{
		// todo: since im OnEnable, this code executes before every thing, and when i
		// disable the Script Player i prevent the Player to execute Start or Awake Function
		// that's why if i want to diable the move of the player i need to separate in a
		// separate class, because other logic/classes need some properties which are
		// initialized in Start/Awake
		players = players.Select(p =>
		{
			p.enabled = false;
			return p;
		}).ToList();
		// subscribe to the observer onEnable

		stateObserverText.Subsribe(this);
		SwitchState(playerTurn);
	}

	private void Awake()
	{
	}

	private void Update()
	{
		// for any state the player is in, we execute the update methode of that State
		// change of the state is instant since this update executs every frame
		State.Update(this);
	}

	public void SwitchState(GameBaseState newState)
	{
		// change the current state and execute the start methode of that new State this is
		// the only way to change the state
		State = newState;
		State.EnterState(this);
		EventListner.Invoke(State);
	}

	public void ChangeState()
	{
		if (State == playerTurn) SwitchState(enemyTurn);
		else if (State == enemyTurn) SwitchState(playerTurn);
	}
}

public abstract class GameBaseState
{
	public abstract void EnterState(GameStateManager playerContext);

	public abstract void Update(GameStateManager playerContext);

	public abstract void ExitState(GameStateManager playerContext);

	public override string ToString()
	{
		return $"{GetType().Name}";
	}
}

// this class inhirit from PlayerBaseState Class the role of this class is to share (execute) code
// that all state execute it
public abstract class AnyState : GameBaseState
{
	// if we want to execute code in all states in the update methode
	public virtual void ExecuteInAnyState()
	{
	}

	public virtual void OnEnableSubscriber()
	{
	}
}
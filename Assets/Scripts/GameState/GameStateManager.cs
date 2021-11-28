using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ISubject<T, W>
{
	public Action<W> EventListner { get; set; }
}

public class GameStateManager : GameManagerListner, ISubject<GameStateManager, BaseState<GameStateManager>>
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
		}
	}

	[SerializeField]
	public List<Player> players;

	private Player _selectedPlayer;

	public Player SelectedPlayer
	{
		get => _selectedPlayer; set
		{
			_selectedPlayer = value;
		}
	}

	[HideInInspector]
	public NodeGrid grid;

	private void OnEnable()
	{
		// todo: since im OnEnable, this code executes before every thing, and when i
		// disable the Script Player i prevent the Player to execute Start or Awake Function
		// that's why if i want to diable the move of the player i need to separate in a
		// separate class, because other logic/classes need some properties which are
		// initialized in Start/Awake
		//players = players.Select(p =>
		//{
		//	p.enabled = false;
		//	return p;
		//}).ToList();
		// subscribe to the observer onEnable

		//stateObserverText.Subsribe(this);

		SwitchState(playerTurn);
	}

	private void Awake()
	{
		grid = FindObjectOfType<NodeGrid>();

	}

	private void Update()
	{
		grid.resetGrid();
		// for any state the player is in, we execute the update methode of that State
		// change of the state is instant since this update executs every frame
		State.Update(this);
	}

	public void SwitchState(BaseState<GameStateManager> newState)
	{
		// change the current state and execute the start methode of that new State this is
		// the only way to change the state
		State = newState;
		PlayerClass selectedUnit = State.EnterState(this);
		//EventListner.Invoke(State);
	}

	public List<Node> CheckMovementRange(PlayerClass unit)
	{
		// by default the first 4 neighbor are always in range
		List<Node> lastLayerOfInrangeNeighbor = new List<Node>(unit.currentPos.neighbours);
		List<Node> allAccceccibleNodes = new List<Node>();

		int firstRange = 8 / 2;
		bool depassMidDepth = true;
		int depth = 0;

		while (true)
		{
			allAccceccibleNodes.AddRange(lastLayerOfInrangeNeighbor);
			if (depth >= firstRange) depassMidDepth = false;

			lastLayerOfInrangeNeighbor = updateNeigbor(lastLayerOfInrangeNeighbor, unit.currentPos, depassMidDepth);
			depth++;
			if (depth == 8) break;
		}

		foreach (Node item in allAccceccibleNodes)
		{
			if (item.firstRange == true)
				item.tile.GetComponent<Renderer>().material.color = Color.black;
			else
				item.tile.GetComponent<Renderer>().material.color = Color.yellow;
		}

		return allAccceccibleNodes;
	}

	public List<Node> updateNeigbor(List<Node> neighbors, Node origin, bool depassMidDepth)
	{
		List<Node> newLastLayer = new List<Node>();
		// every neighbor is updated to inrage is true
		foreach (Node node in neighbors)
		{
			node.inRange = true;
			if (depassMidDepth == false) node.firstRange = true;
		}

		// loop again to create new list of neighbor which are adjacent to the old one
		foreach (Node node in neighbors)
		{
			// for each node loop throw the neighbor which are not inRange and are not
			// in the local newLastLayer list, if they are add them the newLastLayer
			foreach (Node n in node.neighbours.Where((nei) => nei.inRange == false && nei != origin).ToList())
			{
				if (newLastLayer.Contains(n) == false)
					newLastLayer.Add(n);
			}
		}
		return newLastLayer;
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

	public abstract PlayerClass EnterState(T playerContext);

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

	public virtual void ExecuteInAnyGameState(PlayerClass unit)
	{
	}

	public virtual void ExecuteInAnyPlayerState()
	{
	}

	public virtual void OnEnableSubscriber()
	{
	}
}
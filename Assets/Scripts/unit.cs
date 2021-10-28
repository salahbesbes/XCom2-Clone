using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public List<ActionType> actions = new List<ActionType>();
	private NodeGrid grid;
	public Queue<ActionType> actionsInQueue = new Queue<ActionType>();
	public bool processing = false;
	public ActionType currentActionInProcess;

	private void Update()
	{
	}

	private void Start()
	{
		grid = FindObjectOfType<NodeGrid>();
	}

	public void EnQueue(ActionType action)
	{
		actionsInQueue.Enqueue(action);

		tryExecuteNextAction();
	}

	public void tryExecuteNextAction()
	{
		if (processing == false && actionsInQueue.Count > 0)
		{
			processing = true;
			ActionType firstAction = actionsInQueue.Dequeue();
			firstAction.TryUseAction();
		}
	}

	public void finishProcessingAction()
	{
		processing = false;
		grid.path = new List<Node>();
		grid.turnPoints = new Vector3[0];
	}

	public ActionType GetActionNamed(string actionName)
	{
		return actions.Single(el => el.Name == actionName);
	}
}

public interface IStatus
{
}

[Serializable]
public abstract class ActionType
{
	[SerializeField]
	public string name = "Default Name";

	public string Name { get => name; set => name = value; }

	[SerializeField, Range(0, 2)]
	private int cost = 1;

	public int Cost { get => cost; set => cost = value; }

	public abstract bool TryUseAction();

	public abstract bool onFinishAction();

	public override string ToString()
	{
		return $"-- {Name} --- action with cose {Cost}";
	}
}

public interface IActionType
{
	string Name { get; set; }
	int Cost { get; set; }

	bool TryUseAction();

	bool onFinishAction();
}

public class MoveAction : ActionType
{
	public Node start;
	public Node destination;
	public Action<Node, Node> MoveMethod;
	private NodeGrid Grid { get; set; }

	public MoveAction(Node start, Node destination, int cost = 1)
	{
		Name = $"{GetType()}";
		Cost = cost;
		this.start = start;
		this.destination = destination;
	}

	public override bool TryUseAction()
	{
		MoveMethod(start, destination);
		return true;
	}

	public override bool onFinishAction()
	{
		throw new NotImplementedException();
	}

	public override string ToString()
	{
		return $"{base.ToString()} \n  start {start} destination {destination}";
	}
}
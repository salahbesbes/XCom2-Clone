using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IActionType
{
	int Cost { get; set; }
	string Name { get; set; }

	bool onFinishAction();

	bool TryUseAction();
}

public interface IStatus
{
}

[Serializable]
public abstract class ActionType
{
	[SerializeField]
	public string name = "Default Name";

	[SerializeField, Range(0, 2)]
	private int cost = 1;

	public int Cost { get => cost; set => cost = value; }
	public string Name { get => name; set => name = value; }

	public abstract bool onFinishAction();

	public override string ToString()
	{
		return $"-- {Name} --- action with cose {Cost}";
	}

	public abstract bool TryUseAction();
}

public class MoveAction : ActionType
{
	public Node destination;
	public Action<Node, Node> MoveMethod;
	public Node start;

	public MoveAction(Node start, Node destination, int cost = 1)
	{
		Name = $"{GetType()}";
		Cost = cost;
		this.start = start;
		this.destination = destination;
	}

	private NodeGrid Grid { get; set; }

	public override bool onFinishAction()
	{
		throw new NotImplementedException();
	}

	public override string ToString()
	{
		return $"{base.ToString()} \n  start {start} destination {destination}";
	}

	public override bool TryUseAction()
	{
		MoveMethod(start, destination);
		return true;
	}
}

public class Unit : MonoBehaviour
{
	public List<ActionType> actions = new List<ActionType>();
	public Queue<ActionType> actionsInQueue = new Queue<ActionType>();
	public ActionType currentActionInProcess;
	public bool processing = false;
	private NodeGrid grid;

	public void EnQueue(ActionType action)
	{
		actionsInQueue.Enqueue(action);

		tryExecuteNextAction();
	}

	public void finishProcessingAction()
	{
		processing = false;
		//grid.path = new List<Node>();
		//grid.turnPoints = new Vector3[0];
	}

	public ActionType GetActionNamed(string actionName)
	{
		return actions.Single(el => el.Name == actionName);
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

	private void Start()
	{
		grid = FindObjectOfType<NodeGrid>();
	}

	private void Update()
	{
	}
}
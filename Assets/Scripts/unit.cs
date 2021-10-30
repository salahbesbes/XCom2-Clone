using System.Collections.Generic;
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

public class Unit : MonoBehaviour
{
	public List<ActionType> actions = new List<ActionType>();
	public Queue<ActionType> actionsInQueue = new Queue<ActionType>();
	public ActionType currentActionInProcess;
	public bool processing = false;
	private NodeGrid grid;
}
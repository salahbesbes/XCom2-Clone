using System;
using UnityEngine;

public class MoveAction : ActionBase
{
	public new Action<MoveAction, Node, Node> executeAction;
	private Node start, end;

	public MoveAction(Action<MoveAction, Node, Node> callback, string name, Node start, Node end)
	{
		executeAction = callback;

		this.name = name;
		this.start = start;
		this.end = end;
	}

	public override void TryExecuteAction()
	{
		executeAction(this, start, end);
	}

	public override string ToString()
	{
		return $" moving from {start} to {end}";
	}
}

public class ShootAction : ActionBase
{
	public new Action<ShootAction> executeAction;

	public ShootAction(Action<ShootAction> callback, string name)
	{
		executeAction = callback;

		this.name = name;
	}

	public override void TryExecuteAction()
	{
		executeAction(this);
	}
}

public class ReloadAction : ActionBase
{
	public new Action<ReloadAction> executeAction;

	public ReloadAction(Action<ReloadAction> callback, string name)
	{
		executeAction = callback;
		this.name = name;
	}

	public override void TryExecuteAction()
	{
		executeAction(this);
	}
}

public class LunchGrenadeAction : ActionBase
{
	public new Action<LunchGrenadeAction, Node> executeAction;
	public Node destination;

	public LunchGrenadeAction(Action<LunchGrenadeAction, Node> callback, string name, Node dest)
	{
		executeAction = callback;
		this.name = name;
		destination = dest;
	}

	public override void TryExecuteAction()
	{
		executeAction(this, destination);
	}
}

[Serializable]
public class ActionBase
{
	public Action executeAction;
	public string name;
	public Sprite icon;
	public int cost = 1;

	public virtual void TryExecuteAction()
	{
	}

	public virtual void onActionFinish()
	{
	}
}
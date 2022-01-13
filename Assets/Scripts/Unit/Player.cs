using System;
using UnityEngine;

public class Player : PlayerStateManager
{



	//public void OnDrawGizmossss()
	//{
	//	if (grid != null && grid.graph != null)
	//	{
	//		Debug.Log($"{currentPos == null && !TurnOnGizmos}");
	//		if (currentPos == null && !TurnOnGizmos) return;

	//		foreach (Node node in grid?.graph)
	//		{
	//			//string[] collidableLayers = { "Player", "Unwalkable" };
	//			string[] collidableLayers = { "Unwalkable" };
	//			int layerToCheck = LayerMask.GetMask(collidableLayers);

	//			Collider[] hitColliders = Physics.OverlapSphere(node.coord, grid.nodeSize / 2, layerToCheck);
	//			node.isObstacle = hitColliders.Length > 0 ? true : false;
	//			node.color = node.isObstacle ? Color.red : node.inRange ? node.firstRange ? Color.yellow : Color.black : Color.cyan;
	//			//if (node.inRange && node.firstRange) node.color = Color.yellow;
	//			if (path.Contains(node)) node.color = Color.gray;
	//			if (turnPoints.Contains(node.coord)) node.color = Color.green;

	//			foreach (var n in turnPoints)
	//			{
	//				if (node.coord.x == n.x && node.coord.z == n.z)
	//				{
	//					node.color = Color.green;
	//					break;
	//				}
	//			}
	//			if (node == destination) { node.color = Color.black; }
	//			if (node == currentPos) { node.color = Color.blue; }
	//			if (node == CurrentTarget?.currentPos) { node.color = CurrentTarget?.isFlanked == false ? Color.magenta : Color.yellow; }
	//			Gizmos.color = node.color;

	//			Gizmos.DrawCube(node.coord, new Vector3(grid.nodeSize - 0.1f, 0.02f, grid.nodeSize - 0.1f));
	//		}
	//	}

	//	if (CurrentTarget != null)
	//	{
	//		Debug.DrawRay(new Vector3(CurrentTarget.partToRotate.transform.position.x, 0.5f, CurrentTarget.partToRotate.transform.position.z), CurrentTarget.partToRotate.forward * 2, Color.cyan);
	//	}
	//}
}

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
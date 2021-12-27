using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : PlayerStateManager
{
	public void Start()
	{
		grid = NodeGrid.Instance;
		//gameStateManager = FindObjectOfType<GameStateManager>();
		//currentTarget = gameStateManager.SelectedEnemy;

		currentPos = grid.getNodeFromTransformPosition(transform);
		queueOfActions = new Queue<ActionBase>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
		//actions = new ActionType[0];
		//playerHeight = transform.GetComponent<Renderer>().bounds.size.y;
		currentPos = grid.getNodeFromTransformPosition(transform);
		animator = model.GetComponent<Animator>();
		stats = GetComponent<Stats>();
		gameStateManager = GameStateManager.Instance;
		//sportPoints.AddRange(model.GetComponent<SpotPoints>().sportPoint);

		//listners = transform.Find("listners").gameObject;
		//lineConponent.SetUpLine(turnPoints);
	}

	private void getTheRightActionOnClick(string action)
	{
		switch (action)
		{
			case "Shoot":
				CreateNewShootAction();
				break;

			case "Reload":
				CreateNewReloadAction();
				break;

			default:
				Debug.Log($"{action} action does not exist, Check Spelling");
				break;
		}
	}

	private bool checkPointIfSameLineOrColumAsTarget(Vector3 target, Vector3 pointNode)
	{
		if (pointNode != null)
		{
			if (target.x == pointNode.x || target.z == pointNode.z)
			{
				return true;
			}
		}
		return false;
	}

	public void checkFlank(Node target)
	{
		if (currentPos == null || target == null) return;

		Transform points = transform.Find("Points");
		Vector3 selectedPointCood = Vector3.zero;
		Vector3 selectedPoint;
		if (target != null && points != null)
		{
			Dictionary<Vector3, float> ordredDictByMagnitude = new Dictionary<Vector3, float>();

			for (int i = 0; i < points.childCount; i++)
			{
				Transform point = points.GetChild(i);
				float mag = (target.coord - point.position).magnitude;
				ordredDictByMagnitude.Add(point.position, mag);
			}

			ordredDictByMagnitude = ordredDictByMagnitude.OrderBy((item) => item.Value)
									.ToDictionary(t => t.Key, t => t.Value);

			// default node is the nearest one to the target (first one in the dict)
			Vector3 defaultPoint = ordredDictByMagnitude.First().Key;
			selectedPoint = defaultPoint;

			bool foundPotentialPositionToFlank = false;
			foreach (var item in ordredDictByMagnitude)
			{
				Vector3 point = item.Key;
				if (checkPointIfSameLineOrColumAsTarget(target.coord, point))
				{
					foundPotentialPositionToFlank = true;
					// update selected Point Coord
					selectedPoint = point;
					break;
				}
			}
			if (foundPotentialPositionToFlank)
			{
				if (defaultPoint == selectedPoint)
				{
					selectedPoint = ordredDictByMagnitude.ElementAt(1).Key;
				}
				CheckForTargetWithRayCast(selectedPoint, target.coord);
			}
			else
			{
				CheckForTargetWithRayCast(defaultPoint, target.coord);
			}
		}
	}

	public void CheckForTargetWithRayCast(Vector3 pointPosition, Vector3 targetPosition)
	{
		RaycastHit hit;
		Vector3 dir = targetPosition - pointPosition;
		string[] collidableLayers = { "Unwalkable", "Enemy" };
		int layerToCheck = LayerMask.GetMask(collidableLayers);
		// if i can see the player directly without any Obstacle in between => im flanking
		// it else if i see some thing other the enemy in the way => im not
		/* note i dont understand why layer to check is not working properly, if the collidableLayers
		// contain 2 layer, the raycast should detect only object with those layer, and the
		// first object the ray hit it return that hit object, but it doesnot do i thought,
		// when it found an "unwalckable" object the ray conteniou and detect the "Enemy".
		// in my case since im always looking at the enemy the raycast always return True
		// even if the obsacle is in front of the enemy
		*/
		if (Physics.Raycast(pointPosition, dir, out hit, layerToCheck))
		{
			// to compaire the layer of the object we hit to the "Enemy" layer.
			// LayerMask return an bitMask int type different to the gameObject.layer
			// int type => (index) convert index of the layer Enemy to the BitMast type
			// to compair it
			if ((LayerMask.GetMask("Enemy") & 1 << hit.transform.gameObject.layer) != 0)
				CurrentTarget.isFlanked = true;
			else
				CurrentTarget.isFlanked = false;
		}
		else
		{
			CurrentTarget.isFlanked = false;
		}
		Debug.DrawRay(pointPosition, dir, Color.yellow);
	}

	private void rotateToWard(Vector3 dir)
	{
	}

	public void SelectNextEnemy()
	{
		List<Enemy> enemies = gameStateManager.enemies;
		int nbEnemies = enemies.Count;
		if (CurrentTarget != null)
		{
			int currentTargetIndex = enemies.FindIndex(instance => instance == CurrentTarget);
			CurrentTarget = enemies[(currentTargetIndex + 1) % nbEnemies];
		}
	}

	public void OnDrawGizmossss()
	{
		if (grid != null && grid.graph != null)
		{
			Debug.Log($"{currentPos == null && !TurnOnGizmos}");
			if (currentPos == null && !TurnOnGizmos) return;

			foreach (Node node in grid?.graph)
			{
				//string[] collidableLayers = { "Player", "Unwalkable" };
				string[] collidableLayers = { "Unwalkable" };
				int layerToCheck = LayerMask.GetMask(collidableLayers);

				Collider[] hitColliders = Physics.OverlapSphere(node.coord, grid.nodeSize / 2, layerToCheck);
				node.isObstacle = hitColliders.Length > 0 ? true : false;
				node.color = node.isObstacle ? Color.red : node.inRange ? node.firstRange ? Color.yellow : Color.black : Color.cyan;
				//if (node.inRange && node.firstRange) node.color = Color.yellow;
				if (path.Contains(node)) node.color = Color.gray;
				if (turnPoints.Contains(node.coord)) node.color = Color.green;

				foreach (var n in turnPoints)
				{
					if (node.coord.x == n.x && node.coord.z == n.z)
					{
						node.color = Color.green;
						break;
					}
				}
				if (node == destination) { node.color = Color.black; }
				if (node == currentPos) { node.color = Color.blue; }
				if (node == CurrentTarget?.currentPos) { node.color = CurrentTarget?.isFlanked == false ? Color.magenta : Color.yellow; }
				Gizmos.color = node.color;

				Gizmos.DrawCube(node.coord, new Vector3(grid.nodeSize - 0.1f, 0.02f, grid.nodeSize - 0.1f));
			}
		}

		if (CurrentTarget != null)
		{
			Debug.DrawRay(new Vector3(CurrentTarget.partToRotate.transform.position.x, 0.5f, CurrentTarget.partToRotate.transform.position.z), CurrentTarget.partToRotate.forward * 2, Color.cyan);
		}
	}
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
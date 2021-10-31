using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public ActionType[] actions;
	public List<Node> path;
	public Vector3[] turnPoints;
	private NodeGrid grid;
	public Node actualPos;
	public Node destination;
	public Queue<ActionType> queueOfActions;
	public bool processing = false;
	public Weapon weapon;

	public void MovePrefab(Node start, Node end)
	{
		if (destination != null && actualPos != null)
		{
			if (destination == actualPos)
			{
				Debug.Log($" cant click on same Node  ");
			}
			destination.color = Color.black;

			path = FindPath.getPathToDestination(start, end);

			if (path.Count > 0)
			{
				turnPoints = FindPath.createWayPoint(path);
				StartCoroutine(move(turnPoints));
			}
		}
	}

	public IEnumerator move(Vector3[] turnPoints)
	{
		if (turnPoints.Length > 0)
		{
			//grid.path = path;
			//grid.turnPoints = turnPoints;
			Vector3 currentPoint = turnPoints[0];
			int index = 0;
			// this while loop simulate the update methode
			while (true)
			{
				if (transform.position == currentPoint)
				{
					index++;
					if (index >= turnPoints.Length)
					{
						//PathRequestManager.Instance.finishedProcessingPath();

						break;
					}
					currentPoint = turnPoints[index];
				}

				transform.position = Vector3.MoveTowards(transform.position, currentPoint, 5f * Time.deltaTime);
				// this yield return null waits until the next frame reached ( dont
				// exit the methode )
				yield return null;
			}
		}

		//Debug.Log($"finish moving");
		finishAction();
		yield return null;
	}

	public void finishAction()
	{
		processing = false;
		ExecuteActionInQueue();
	}

	public void moveAction(Vector3[] turnPoints)
	{
		StartCoroutine(move(turnPoints));
	}

	public IEnumerator shoot()
	{
		//string res = "start shooting ";
		//Debug.Log($"{res}");
		for (int i = 0; i < 4; i++)
		{
			yield return new WaitForSeconds(0.5f);
		}
		//res = "finish shooting";
		//Debug.Log($"{res}");
		finishAction();
		yield return null;
	}

	public void shootAction()
	{
		StartCoroutine(weapon.startShooting());
	}

	public void Update()
	{
		grid.resetGrid();
		actualPos = grid.getNodeFromTransformPosition(transform);
		if (Input.GetMouseButtonDown(0))
		{
			Node oldDest = destination;
			destination = grid.getNodeFromMousePosition();
			Debug.Log($"destination {destination} coord = {destination?.coord}");
			if (destination != null)
			{
				if (oldDest == null || destination == actualPos)
					oldDest = actualPos;

				MoveAction move = new MoveAction(MovePrefab, "Move", oldDest, destination);
				Enqueue(move);
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			StartCoroutine(weapon.startShooting());
		}
	}

	public void Enqueue(ActionType action)
	{
		queueOfActions.Enqueue(action);
		ExecuteActionInQueue();
	}

	public void ExecuteActionInQueue()
	{
		if (processing == false && queueOfActions.Count > 0)
		{
			processing = true;
			ActionType action = queueOfActions.Dequeue();
			action.TryExecuteAction();
		}
	}

	public void OnDrawGizmos()
	{
		if (grid != null && grid.graph != null)
		{
			foreach (Node node in grid?.graph)
			{
				//string[] collidableLayers = { "Player", "Unwalkable" };
				string[] collidableLayers = { "Unwalkable" };
				int layerToCheck = LayerMask.GetMask(collidableLayers);

				Collider[] hitColliders = Physics.OverlapSphere(node.coord, grid.nodeSize / 2, layerToCheck);
				node.isObstacle = hitColliders.Length > 0 ? true : false;
				node.color = node.isObstacle ? Color.red : node.inRange ? node.firstRange ? Color.yellow : Color.black : Color.cyan;
				if (node.inRange && node.firstRange) node.color = Color.yellow;
				if (path.Contains(node)) node.color = Color.gray;

				foreach (var n in turnPoints)
				{
					if (n == node.coord)
					{
						node.color = Color.green;
						break;
					}
				}
				if (node == destination) { node.color = Color.black; }
				if (node == actualPos) { node.color = Color.blue; }
				Gizmos.color = node.color;

				Gizmos.DrawCube(node.coord, new Vector3(grid.nodeSize - 0.1f, 0.1f, grid.nodeSize - 0.1f));
			}
		}
	}

	public void Awake()
	{
		grid = FindObjectOfType<NodeGrid>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
		queueOfActions = new Queue<ActionType>();
		actions = new ActionType[0];
	}

	public void Start()
	{
	}
}

public class MoveAction : ActionType
{
	public new Action<Node, Node> executeAction;
	private Node start, end;

	public MoveAction(Action<Node, Node> callback, string name, Node start, Node end)
	{
		executeAction = callback;

		this.name = name;
		this.start = start;
		this.end = end;
	}

	public override void TryExecuteAction()
	{
		executeAction(start, end);
	}

	public override string ToString()
	{
		return $"{base.ToString() } moving from {start} to {end}";
	}
}

public class ShootAction : ActionType
{
	public ShootAction(Action callback, string name)
	{
		executeAction = callback;

		this.name = name;
	}

	public override void TryExecuteAction()
	{
		executeAction();
	}
}

[Serializable]
public class ActionType
{
	public Action executeAction;
	public string name;

	public virtual void TryExecuteAction()
	{
	}
}
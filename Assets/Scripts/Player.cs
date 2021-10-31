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
	public Enemy enemy;
	private float playerHeight;

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

		checkforFlinkPosition(enemy.position);
	}

	public void checkforFlinkPosition(Node node)
	{
		RaycastHit hit;
		int X = node.x;
		int Y = node.y;
		float limitX = X, limitY = Y;
		Debug.Log($"fDown {node.flinkedDown} fUp {node.flinkedUp} fleft {node.flinkedLeft} fRight {node.flinkedRight}");
		if (actualPos.x > X)
		{
			if (actualPos.y > Y)
			{
				Debug.Log($" im at Right-top of enemy");
				if (node.flinkedRight && node.flinkedUp)
				{
					CheckForTargetWithRayCast();
					Debug.Log($"im flinking the enemy ");
				}
			}
			else if (actualPos.y < Y)
			{
				Debug.Log($" im at Right-Down of enemy");
				if (node.flinkedRight && node.flinkedDown)
				{
					CheckForTargetWithRayCast();

					Debug.Log($"im flinking the enemy ");
				}
			}
			else
			{
				Debug.Log($" im on the Horizental line of the enemy");
			}
		}
		else if (actualPos.x < X)
		{
			if (actualPos.y > Y)
			{
				Debug.Log($" im at left-top of enemy");
				if (node.flinkedLeft && node.flinkedUp)
				{
					CheckForTargetWithRayCast();

					Debug.Log($"im flinking the enemy ");
				}
			}
			else if (actualPos.y < Y)
			{
				Debug.Log($" im at left-Down of enemy");
				if (node.flinkedLeft && node.flinkedDown)
				{
					CheckForTargetWithRayCast();

					Debug.Log($"im flinking the enemy ");
				}
			}
			else
			{
				Debug.Log($" im on the Horizental line of the enemy");
			}
		}
		else
		{
			Debug.Log($" im on the Vertical line of the enemy");
		}
	}

	public void CheckForTargetWithRayCast()
	{
		Vector3 dir = enemy.position.coord - actualPos.coord;
		LayerMask enemyLayer = LayerMask.GetMask("Enemy");
		Ray raytest = new Ray();
		raytest.direction = dir;
		raytest.origin = actualPos.coord + Vector3.up * playerHeight;
		RaycastHit hit;
		Debug.Log($" raycast  {Physics.Raycast(raytest, out hit, enemyLayer)}");
		if (Physics.Raycast(raytest, out hit, enemyLayer))
		{
			Debug.Log($"hit => {hit.transform.tag} name = {hit.transform?.name}");
		}
		Debug.DrawRay(raytest.origin, raytest.direction, Color.red);
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
				if (node == enemy.position) { node.color = enemy.isFlanked == false ? Color.magenta : Color.yellow; }
				Gizmos.color = node.color;

				Gizmos.DrawCube(node.coord, new Vector3(grid.nodeSize - 0.1f, 0.1f, grid.nodeSize - 0.1f));
			}

			//Vector3 dir = enemy.position.coord + Vector3.up * 3 - actualPos.coord;
			//LayerMask enemyLayer = LayerMask.GetMask("Enemy");
			//Gizmos.DrawLine(enemy.position.coord, actualPos.coord);
			//RaycastHit hit;
			//if (Physics.Raycast(actualPos.coord, dir, out hit))
			//{
			//	//Debug.Log($"hit is tagged enemy {hit.transform.CompareTag("Enemy")}");
			//	//Debug.Log($"{hit.transform.gameObject.layer == enemyLayer}");
			//}
		}
	}

	public void Awake()
	{
		grid = FindObjectOfType<NodeGrid>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
		queueOfActions = new Queue<ActionType>();
		actions = new ActionType[0];
		playerHeight = transform.GetComponent<Renderer>().bounds.;
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
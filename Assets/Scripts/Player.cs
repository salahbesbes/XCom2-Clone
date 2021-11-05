using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnit : MonoBehaviour
{
	//public ActionType[] actions;
	protected List<Node> path;

	public Queue<ActionBase> queueOfActions;

	protected Vector3[] turnPoints;
	protected NodeGrid grid;
	public Node currentPos;
	public Node destination;
	public bool processing = false;
	public Weapon weapon;

	private void Awake()
	{
		grid = FindObjectOfType<NodeGrid>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
	}

	public void MoveActionCallback(MoveAction actionInstance, Node start, Node end)
	{
		if (destination != null && currentPos != null)
		{
			if (destination == currentPos)
			{
				Debug.Log($" cant click on same Node  ");
			}
			destination.color = Color.black;

			path = FindPath.getPathToDestination(start, end);

			if (path.Count > 0)
			{
				turnPoints = FindPath.createWayPoint(path);
				StartCoroutine(move(actionInstance, turnPoints));
			}
		}
	}

	public IEnumerator move(MoveAction moveInstance, Vector3[] turnPoints)
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
		FinishAction(moveInstance);
		//onActionFinish();
		yield return null;
	}

	public void FinishAction(ActionBase action)
	{
		processing = false;
		// update the cost
		GetComponent<PlayerStats>().ActionPoint -= action.cost;
		ExecuteActionInQueue();
	}

	public void ReloadAction(ReloadAction reload)
	{
		StartCoroutine(weapon.Reload(reload));
	}

	public void ShootAction(ShootAction soot)
	{
		StartCoroutine(weapon.startShooting(soot));
	}

	public void Enqueue(ActionBase action)
	{
		queueOfActions.Enqueue(action);
		ExecuteActionInQueue();
	}

	public void ExecuteActionInQueue()
	{
		if (processing == false && queueOfActions.Count > 0)
		{
			processing = true;
			ActionBase action = queueOfActions.Dequeue();
			action.TryExecuteAction();
		}
	}

	public Action getOnClickEvent(string ActionName)
	{
		switch (ActionName)
		{
			case "Shoot":
				ShootAction shoot = new ShootAction(ShootAction, ActionName);
				return () => ShootAction(shoot);

			case "Reload":
				ReloadAction reload = new ReloadAction(ReloadAction, ActionName);
				return () => ReloadAction(reload);

			default:
				Debug.Log($" {ActionName} Action does not exist");
				break;
		}
		return null;
	}
}

public class Player : BaseUnit
{
	public Enemy enemy;
	public Transform shootingPoint;

	public List<ActionBase> actions = new List<ActionBase>();
	public Transform UIholder;
	public GameObject Action_Prefab;

	public void Update()
	{
		currentPos = grid.getNodeFromTransformPosition(transform);

		if (Input.GetMouseButtonDown(0))
		{
			Node oldDest = destination;
			destination = grid.getNodeFromMousePosition();
			//Debug.Log($"destination {destination} coord = {destination?.coord}");
			if (destination != null)
			{
				if (oldDest == null || destination == currentPos)
					oldDest = currentPos;

				MoveAction move = new MoveAction(MoveActionCallback, "Move", oldDest, destination);
				Enqueue(move);
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			ShootAction shoot = new ShootAction(ShootAction, "Shoot");
			Enqueue(shoot);
			//StartCoroutine(weapon.startShooting(shoot));
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			ReloadAction reload = new ReloadAction(ReloadAction, "Reload");
			Enqueue(reload);
			//StartCoroutine(weapon.Reload(reload));
		}
		checkforFlinkPosition(enemy.position);
	}

	public void checkforFlinkPosition(Node node)
	{
		if (node != null)
		{
			//todo: optimize the flanking check system
			transform.LookAt(enemy.transform);

			int X = node.x;
			int Y = node.y;
			float limitX = X, limitY = Y;
			//Debug.Log($"fDown {node.flinkedDown} fUp {node.flinkedUp} fleft {node.flinkedLeft} fRight {node.flinkedRight}");
			// every frame i suppose that the player is not flanking the enemy then i update the property only when i flank it
			enemy.isFlanked = false;
			if (currentPos.x > X)
			{
				if (currentPos.y > Y)
				{
					Debug.Log($" im at Right-top of enemy");
					if (currentPos.x == X + 1 || currentPos.y == Y + 1)
					{
						if (currentPos.DownCover.Exist && currentPos.LeftCover.Exist == false)
						{
							if (node.flinkedUp)
								CheckForTargetWithRayCast(Vector3.forward + Vector3.forward * 0.5f);
						}

						if (currentPos.LeftCover.Exist && currentPos.DownCover.Exist == false)
						{
							if (node.flinkedRight)
								CheckForTargetWithRayCast(Vector3.right + Vector3.forward * 0.5f);
						}
					}
					else if (node.flinkedRight || node.flinkedUp)
					{
						CheckForTargetWithRayCast();
					}
				}
				else if (currentPos.y < Y)
				{
					Debug.Log($" im at Right-Down of enemy");

					if (currentPos.x == X + 1 || currentPos.y == Y - 1)
					{
						if (currentPos.UpCover.Exist && currentPos.LeftCover.Exist == false)
						{
							if (node.flinkedDown)
								CheckForTargetWithRayCast(Vector3.left + Vector3.back * 0.5f);
						}
						if (currentPos.LeftCover.Exist)
						{
							if (node.flinkedRight)
								CheckForTargetWithRayCast(Vector3.right + Vector3.forward * 0.5f);
						}
					}
					else if (node.flinkedRight || node.flinkedDown)
					{
						CheckForTargetWithRayCast();

						Debug.Log($"im flinking the enemy ");
					}
				}
				else
				{
					CheckForTargetWithRayCast();
					Debug.Log($" im on the Horizental line of the enemy");
				}
			}
			else if (currentPos.x < X)
			{
				if (currentPos.y > Y)
				{
					Debug.Log($" im at left-top of enemy");

					if (currentPos.x == X - 1)
					{
						if (currentPos.DownCover.Exist && currentPos.RightCover.Exist == false)
						{
							if (node.flinkedUp)
								CheckForTargetWithRayCast(Vector3.right + Vector3.forward * 0.5f);
						}
					}
					else if (node.flinkedLeft || node.flinkedUp)
					{
						CheckForTargetWithRayCast();

						Debug.Log($"im flinking the enemy ");
					}
				}
				else if (currentPos.y < Y)
				{
					Debug.Log($" im at left-Down of enemy");
					if (currentPos.x == X - 1 || currentPos.y == Y - 1)
					{
						if (currentPos.UpCover.Exist)
						{
							if (node.flinkedDown)
								CheckForTargetWithRayCast(Vector3.right + Vector3.back * 0.5f);
						}
						if (currentPos.RightCover.Exist)
						{
							if (node.flinkedLeft)
								CheckForTargetWithRayCast(Vector3.forward + Vector3.back * 0.5f);
						}
					}
					else if (node.flinkedLeft || node.flinkedDown)
					{
						CheckForTargetWithRayCast();
						Debug.Log($"im flinking the enemy ");
					}
				}
				else
				{
					CheckForTargetWithRayCast();
					Debug.Log($" im on the Horizental line of the enemy");
				}
			}
			else
			{
				CheckForTargetWithRayCast();

				Debug.Log($" im on the Vertical line of the enemy");
			}
		}
	}

	public void CheckForTargetWithRayCast(Vector3? offset = null)
	{
		RaycastHit hit;
		offset = offset == null ? Vector3.zero : offset;
		Vector3 dir = enemy.position.coord - (shootingPoint.position + (Vector3)offset);
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
		if (Physics.Raycast(shootingPoint.position + (Vector3)offset, dir, out hit, layerToCheck))
		{
			// to compaire the layer of the object we hit to the "Enemy" layer.
			// LayerMask return an bitMask int type different to the gameObject.layer
			// int type => (index) convert index of the layer Enemy to the BitMast type
			// to compair it
			if ((LayerMask.GetMask("Enemy") & 1 << hit.transform.gameObject.layer) != 0)
				enemy.isFlanked = true;
			else
				enemy.isFlanked = false;
		}
		else
		{
			enemy.isFlanked = false;
		}
		Debug.DrawRay(shootingPoint.position + (Vector3)offset, dir, Color.yellow);
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
				if (node == currentPos) { node.color = Color.blue; }
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
		queueOfActions = new Queue<ActionBase>();
		//actions = new ActionType[0];
		//playerHeight = transform.GetComponent<Renderer>().bounds.size.y;
	}

	public void Start()
	{
		foreach (ActionBase action in actions)
		{
			GameObject obj = Instantiate(Action_Prefab);
			obj.transform.name = action.name + "_Action";
			Button btn = obj.GetComponentInChildren<Button>();
			btn.GetComponent<Image>().sprite = action.icon;
			btn.onClick.AddListener(delegate { getOnClickEvent(action.name)(); });

			obj.transform.SetParent(UIholder);
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
		return $"{base.ToString() } moving from {start} to {end}";
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
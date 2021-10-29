using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public List<Node> path;
	public Vector3[] turnPoints;
	private NodeGrid grid;

	public void MovePrefab()
	{
		if (grid.destination != null && grid.start != null)
		{
			if (grid.destination == grid.start)
			{
				Debug.Log($" cant click on same Node  ");
			}
			grid.destination.color = Color.black;

			path = FindPath.getPathToDestination(grid.start, grid.destination);
			turnPoints = FindPath.createWayPoint(path);

			if (turnPoints.Length > 0)
			{
				StartCoroutine(move(turnPoints));
				//resetGrid();
			}
		}
	}

	public IEnumerator move(Vector3[] turnPoints)
	{
		string res = $"start moving start";
		Debug.Log($"{res}");

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
						Debug.Log($"finish moving");

						yield break; ;
					}
					currentPoint = turnPoints[index];
				}

				transform.position = Vector3.MoveTowards(transform.position, currentPoint, 2f * Time.deltaTime);
				// this yield return null waits until the next frame reached ( dont
				// exit the methode )
				yield return null;
			}
		}
	}

	public void moveAction(Vector3[] turnPoints)
	{
		StartCoroutine(move(turnPoints));
	}

	public IEnumerator shoot()
	{
		string res = "start shooting ";
		Debug.Log($"{res}");
		for (int i = 0; i < 4; i++)
		{
			yield return new WaitForSeconds(0.5f);
		}
		res = $"=> finish shooting counter = {PathRequestManager.counter}";
		Debug.Log($"{res}");
		PathRequestManager.Instance.finishedProcessingPath();
	}

	public void shootAction()
	{
		StartCoroutine(shoot());
	}

	public void Update()
	{
		PathRequestManager.Instance.TryProcessNext();
		if (Input.GetMouseButtonDown(0))
		{
			grid.destination = grid.getNodeFromMousePosition();
			//List<Node> path = new List<Node>();
			//Vector3[] turnPoints = new Vector3[0];

			//path = FindPath.getPathToDestination(grid.GetNode(transform.position.x, transform.position.z), grid.destination);
			//turnPoints = FindPath.createWayPoint(path);
			//PathRequestManager.Instance.Enqueue(new MoveActionPlayer(moveAction, "move", turnPoints));

			MovePrefab();
		}
		if (Input.GetMouseButtonDown(1))
		{
			//pathRequest.Enqueue(new playerAction(shootAction, "shoot"));
		}
	}

	public void OnDrawGizmos()
	{
		foreach (Node node in path)
		{
			node.color = Color.gray;
		}

		foreach (Vector3 v in turnPoints)
		{
			Node node = grid.GetNode(v.x, v.z);
			node.color = Color.green;
		}
	}

	public void Awake()
	{
		grid = FindObjectOfType<NodeGrid>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
	}

	public void Start()
	{
	}
}

public class MoveActionPlayer : playerAction
{
	public new Action<Vector3[]> callback;
	public Vector3[] turnPoints;

	public MoveActionPlayer(Action<Vector3[]> callback, string name, Vector3[] turnPoints)
	{
		this.callback = callback;

		this.name = name;
		this.turnPoints = turnPoints;
	}

	public override void executeAction()
	{
		callback(turnPoints);
		PathRequestManager.Instance.StopAllCoroutines();
		PathRequestManager.Instance.finishedProcessingPath();
	}

	public override string ToString()
	{
		return $"move action destination is {turnPoints}";
	}
}

public abstract class playerAction
{
	public Action callback;
	public string name;

	public abstract void executeAction();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
	public static ActionsManager Instance;
	public Transform playerPrefab;
	private Unit player;
	private NodeGrid grid;
	/// <summary>
	/// this Action manager need to fill the Player Action on Awake not On Start
	/// </summary>

	private void Awake()
	{
		if (Instance == null)
		{
			player = playerPrefab.GetComponent<Unit>();
			grid = FindObjectOfType<NodeGrid>();
			Instance = this;
			ActionType move = new MoveAction(grid.start, grid.destination);

			player.actions.Add(move);
		}
	}

	/// <summary>
	/// move the playerPrefab toward the destination var sent from the grid to Gridpath var.
	/// this methode start on mouse douwn frame and the player start moving on the next frame
	/// until it reaches the goal. thats why we are using the carroutine. to simulate the update
	/// methode we use a while loop the problem is that the while loop is too rapid ( high
	/// frequency iteration) to iterate with the same frequence of the update methode we use
	/// yield return null or some other tools the wait for certain time "WaitForSeconds"
	/// </summary>
	/// <param name="playerPrefab"> Transform playerPrefab </param>
	/// <param name="path"> Array of position to </param>
	private IEnumerator startMove(float speed, Node currentPosition, Node destination)
	{
		// yield break exit out the caroutine
		//if (turnPoints.Length == 0) yield break;
		if (playerPrefab == null) yield break;

		List<Node> path = new List<Node>();
		Vector3[] turnPoints = new Vector3[0];
		bool foundPath = FindPath.getPathToDestination(currentPosition, destination, out turnPoints, out path);
		if (foundPath)
		{
			grid.path = path;
			grid.turnPoints = turnPoints;
			Vector3 currentPoint = turnPoints[0];
			int index = 0;
			// this while loop simulate the update methode
			while (true)
			{
				if (playerPrefab.position == currentPoint)
				{
					index++;
					if (index >= turnPoints.Length)
					{
						player.finishProcessingAction();
						yield break;
					}
					currentPoint = turnPoints[index];
				}

				playerPrefab.position = Vector3.MoveTowards(playerPrefab.position, currentPoint, speed * Time.deltaTime);
				// this yield return null waits until the next frame reached ( dont
				// exit the methode )
				yield return null;
			}
		}

		//Debug.Log($"start walking ... wait 3s ");
		//for (int i = 0; i < 3; i++)
		//{
		//	Debug.Log($"{i}");
		//	yield return new WaitForSeconds(1);
		//}
		//Debug.Log($"stop walking");
		//player.finishProcessingAction();
	}

	public void moveAction(Node start, Node end)
	{
		Node destination = end;
		Node currentPosition = start;
		Debug.Log($"{currentPosition} {destination}");
		if (destination != null && currentPosition != null)
		{
			if (destination == currentPosition)
			{
				Debug.Log($" cant click on same Node  ");
				return;
			}
			destination.color = Color.black;

			//bool foundPath = FindPath.getPathToDestination(currentPosition, destination, out turnPoints, out path);
			//grid.path = path;
			////grid.turnPoints = turnPoints;

			//if (foundPath)
			//{
			//	StartCoroutine(startMove(3f, turnPoints));
			//	grid.resetGrid();
			//}

			StartCoroutine(startMove(3f, currentPosition, destination));
		}
	}

	private IEnumerator startShoot()
	{
		for (int i = 0; i < 3; i++)
		{
			Debug.Log($" shooting .... ");
			// spend 0.25 sec to spawn 1 enemy in a wave
			yield return new WaitForSeconds(1);
		}
	}

	public void shootAction()
	{
		StartCoroutine(startShoot());
	}

	public bool finishAction()
	{
		//player.EnQueue("shoot");
		Debug.Log($" on action finish lenth of queue{ player.actionsInQueue.Count}");
		player.processing = false;
		//player.tryExecuteNextAction();
		//player.ExecuteActionsInQueue();
		return true;
	}
}
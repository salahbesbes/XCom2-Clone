using UnityEngine;

public class RangeMovement : MonoBehaviour
{
	private NodeGrid grid;
	public bool isSelected = false;
	public int movementRange = 20;
	private Node oldDestination;

	private void Start()
	{
		grid = NodeGrid.Instance;
	}

	// Update is called once per frame
	private void Update()
	{
		if (oldDestination == null) oldDestination = grid.start;
		if (grid)
		{
			if (Input.GetMouseButtonDown(1))
			{
				if (isSelected == true)
				{
					//Todo: implement a class than handle multiple player in the scene and the User can only chose to select one player to play with
					grid.playerPrefab = null;
					isSelected = false;
				}
				else isSelected = true;
			}
			if (isSelected == true)
			{
				grid.playerPrefab = transform;

				grid.start = grid.getNodeFromTransformPosition(transform);

				//grid.resetGrid();

				//Debug.Log($" in range {test}");
				// update only on idel state ( arrive at desti or didnt move yet )
				if (grid.start == grid.destination || grid.destination == null)
				{
					grid.resetGrid();
					bool test = updateRangeMove(grid.start, 0, false);
					oldDestination = grid.destination;
					GetComponent<Unit>().tryExecuteNextAction();
				}
				// this execust every frame so we need to check fom some required

				grid.destination = grid.getNodeFromMousePosition();
				if (Input.GetMouseButtonDown(0))
				{
					if (grid.destination.firstRange == true)
					{
						bool inRange = updateRangeMove(oldDestination, 0, false);
						Unit unit = GetComponent<Unit>();
						MoveAction action = new MoveAction(oldDestination, grid.destination);
						action.MoveMethod = ActionsManager.Instance.moveAction;
						Debug.Log($" old dest is {oldDestination} in range {inRange}");
						unit.EnQueue(action);
					}
					else if (grid.destination.inRange && grid.destination.firstRange == false)
					{
						bool inRange = updateRangeMove(oldDestination, 0, false);
						Unit unit = GetComponent<Unit>();
						MoveAction action = new MoveAction(oldDestination, grid.destination);
						action.MoveMethod = ActionsManager.Instance.moveAction;
						Debug.Log($" old dest is {oldDestination} in range {inRange}");
						unit.EnQueue(action);
					}
				}
			}
		}
	}

	public bool updateRangeMove(Node node, int range, bool inRange)
	{
		if (range == movementRange - 1)
			return inRange;
		if (range <= (movementRange - 1) / 2)
			node.firstRange = true;

		foreach (var neighbor in node.neighbours)
		{
			if (grid.destination == neighbor)
				inRange = true;
			if (neighbor.inRange == false)
			{
				neighbor.inRange = true;
			}

			updateRangeMove(neighbor, range + 1, inRange);
		}
		return true;
	}
}
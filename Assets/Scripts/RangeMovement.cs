using UnityEngine;

public class RangeMovement : MonoBehaviour
{
	private NodeGrid grid;
	public bool isSelected = false;
	public int movementRange = 20;

	private void Start()
	{
		grid = NodeGrid.Instance;
	}

	// Update is called once per frame
	private void Update()
	{
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

				// update only on idel state ( arrive at desti or didnt move yet )
				if (grid.start == grid.destination || grid.destination == null)
				{
					updateRangeMove(grid.start, 0);

				}
				else
				{
					checkDestination();
				}

			}
		}
	}

	/// <summary> every node in range of the player position is updated to inRagnge </summary>
	/// <param name="node"> current node position </param>
	/// <param name="range"> start from 0 range always </param>
	public void updateRangeMove(Node node, int range)
	{
		if (range == movementRange - 1)
			return;
		if (range <= (movementRange - 1) / 2)
			node.firstRange = true;

		foreach (var neighbor in node.neighbours)
		{
			if (neighbor.inRange == false)
			{
				neighbor.inRange = true;
			}

			updateRangeMove(neighbor, range + 1);
		}
	}


	void checkDestination()
	{
		// while player is moving (before he reach destination all nodes are being reset to default status
		Node dest = grid.destination;
		if (dest.firstRange)
		{
			Debug.Log($"inside FIRST RANGE  cost 1");
			grid.MovePrefab();
			return;
		}
		if (dest.inRange && dest.firstRange == false)
		{
			Debug.Log($"inside SECOND RANGE cost 2");
			grid.MovePrefab();
			return;
		}


	}

}
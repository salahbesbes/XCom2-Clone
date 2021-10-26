using UnityEngine;

public class RangeMovement : MonoBehaviour
{
	private NodeGrid grid;
	public bool isSelected = false;
	public int movementRange = 4;

	private void Start()
	{
		grid = NodeGrid.Instance;
	}

	// Update is called once per frame
	private void Update()
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
		}
	}

	/// <summary> every node in range of the player position is updated to inRagnge </summary>
	/// <param name="node"> current node position </param>
	/// <param name="range"> start from 0 range always </param>
	public void updateRangeMove(Node node, int range)
	{
		foreach (var neighbor in node.neighbours)
		{
			if (neighbor.inRange == false)
			{
				neighbor.inRange = true;
			}
			if (range == movementRange - 1)
				return;
			updateRangeMove(neighbor, range + 1);
		}
	}
}
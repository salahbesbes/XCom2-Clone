using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
	LineRenderer lr;
	Node destination;
	Node currentPos;

	List<Node> path = new List<Node>();
	Vector3[] turnPoints;
	Node potentialDest;
	Node oldDestination;
	List<Vector3> positions = new List<Vector3>();

	void Start()
	{
		lr = GetComponent<LineRenderer>();
		currentPos = NodeGrid.Instance.getNodeFromTransformPosition(transform);
		turnPoints = new Vector3[0];
	}

	// Update is called once per frame
	void Update()
	{
		NodeGrid.Instance.resetGrid();
		currentPos = NodeGrid.Instance.getNodeFromTransformPosition(transform);
		lr.positionCount = path.Count;
		positions = path.Select(el => el.coord).Select(el => new Vector3(el.x, 0.5f, el.z)).ToList();

		List<Vector3> newList = makechangeson(2, positions);
		lr.SetPositions(newList.ToArray());
		potentialDest = onNodeHover(oldDestination);
	}

	private List<Vector3> makechangeson(int idx, List<Vector3> positions)
	{








		return positions;

	}






	public Node onNodeHover(Node oldPotentialDest)
	{
		//Node oldDestination = destination;
		if (NodeGrid.Instance == null) return null;
		Node res;

		res = NodeGrid.Instance.getNodeFromMousePosition();

		Node potentialDestination = res;

		if (potentialDestination != null && potentialDestination != destination && potentialDestination != currentPos)
		{
			List<Node> potentialPath = FindPath.AStarAlgo(currentPos, potentialDestination);
			if (potentialPath.Count == 0) return null;
			Vector3[] turns = FindPath.createWayPoint(potentialPath);

			//lineConponent.SetUpLine(turnPoints);

			path = potentialPath;
			turnPoints = turns;
			foreach (Node node in path)
			{
				if (turnPoints.Contains(node.coord))
					node.tile.obj.GetComponent<Renderer>().material.color = Color.green;
				else
				{
					node.tile.obj.GetComponent<Renderer>().material.color = Color.gray;
				}
			}
			potentialDestination.tile.obj.GetComponent<Renderer>().material.color = Color.blue;

			if (Input.GetMouseButtonDown(0))
			{
				//ActionData move = actions.FirstOrDefault((el) => el is MovementAction);
				//move.Actionevent.Raise();
				//CreateNewMoveAction();
			}

			if (oldPotentialDest != null && oldPotentialDest != potentialDestination)
			{
				oldPotentialDest.tile.destroyAllActiveCover();
				oldPotentialDest.tile.mouseOnTile = false;
			}
			return potentialDestination;
		}
		// if potentialDestination is null(hover over some unwalckabale) we return the
		// oldDestination
		return oldPotentialDest;
	}
}

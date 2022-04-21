using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
	private LineRenderer lr;
	private Node destination;
	private Node currentPos { get; set; }

	private List<Node> path = new List<Node>();
	private Vector3[] turnPoints;
	private Node potentialDest;
	private Node oldDestination;
	private List<Vector3> positions = new List<Vector3>();


	private void Start()
	{
		lr = GetComponent<LineRenderer>();
		currentPos = NodeGrid.Instance.getNodeFromTransformPosition(transform);
		Debug.Log($"{currentPos}");
		turnPoints = new Vector3[0];
	}

	// Update is called once per frame
	private void Update()
	{
		NodeGrid.Instance.resetGrid();
		currentPos = NodeGrid.Instance.getNodeFromTransformPosition(transform);

		Vector3[] newList = onNodeHover();
		lr.positionCount = newList.Length;
		lr.SetPositions(newList.ToArray());
	}

	public Vector3[] onNodeHover()
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
			Vector3[] points = FindPath.createWayPoint(potentialPath);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				move(points);
			}
			return points;
		}
		// if potentialDestination is null(hover over some unwalckabale) we return the
		// oldDestination
		return turnPoints;
	}

	public async Task move(Vector3[] points)
	{
		if (points.Length > 0)
		{
			float playerHeight = transform.GetComponent<Collider>().bounds.size.y;
			Vector3 currentPoint = points[0] + (Vector3.up * playerHeight / 2);
			int index = 0;
			Vector3 dir = currentPoint - transform.position;
			// this while loop simulate the update methode
			Quaternion targetRotation = Quaternion.LookRotation(dir);
			transform.Rotate(0, targetRotation.eulerAngles.y, 0);

			while (true)
			{
				if (transform.position == currentPoint)
				{
					index++;
					if (index >= points.Length)
					{
						//PathRequestManager.Instance.finishedProcessingPath();
						break;
					}
					dir = (points[index] - transform.position).normalized;
					currentPoint = points[index] + (Vector3.up * playerHeight / 2);
				}
				transform.position = Vector3.MoveTowards(transform.position, currentPoint, 5 * Time.deltaTime);

				await Task.Yield();
			}
		}

		Debug.Log($"finish moving");
		//onActionFinish();
		await Task.Yield();
	}
}
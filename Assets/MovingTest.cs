using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MovingTest : MonoBehaviour
{
	//public ActionType[] actions;
	protected List<Node> path;

	public Queue<ActionBase> queueOfActions;

	[HideInInspector]
	public NodeGrid grid;

	[SerializeField]
	public Node currentPos;
	public Node destination;
	private Rigidbody rb;
	private LineRenderer lr;
	public Transform target;
	public float factor = 0;
	public float Jumpeight = 5f;
	public List<Transform> traject = new List<Transform>();
	private List<Vector3> points = new List<Vector3>();
	public Transform point;

	private void Start()
	{
		points = traject.Select(el => el.position).ToList();

		lr = GetComponent<LineRenderer>();
		lr.positionCount = 30;
		grid = NodeGrid.Instance;
		//gameStateManager = FindObjectOfType<GameStateManager>();
		//currentTarget = gameStateManager.SelectedEnemy;

		currentPos = grid.getNodeFromTransformPosition(transform);
		path = new List<Node>();
		//actions = new ActionType[0];
		//playerHeight = transform.GetComponent<Renderer>().bounds.size.y;
		currentPos = grid.getNodeFromTransformPosition(transform);
	}

	private async void Update()
	{
		grid.resetGrid();
		currentPos = grid.getNodeFromTransformPosition(transform);
		if (Input.GetKeyDown(KeyCode.Space))
		{
			await move();
			points.Reverse();
		}
		Debug.DrawRay(point.position, transform.forward, Color.red);
		Debug.DrawRay(point.position, -transform.up * 2, Color.green);

		//onNodeHover();
		//DrowTrajectory();
	}

	private void moveSphere()
	{
	}

	public async void onNodeHover()
	{
		//Node oldDestination = destination;
		Node res;
		res = NodeGrid.Instance.getNodeFromMousePosition();

		Node potentialDestination = res;

		if (potentialDestination != null && potentialDestination != destination && potentialDestination != currentPos)
		{
			List<Node> potentialPath = FindPath.AStarAlgo(currentPos, potentialDestination);
			if (potentialPath.Count == 0) return;
			Vector3[] turns = FindPath.createWayPoint(potentialPath);

			//lineConponent.SetUpLine(turnPoints);

			path = potentialPath;
			//turnPoints = turns;
			foreach (Node node in path)
			{
				//if (turnPoints.Contains(node.coord))
				//	node.tile.obj.GetComponent<Renderer>().material.color = Color.green;
				//else
				//{
				//	node.tile.obj.GetComponent<Renderer>().material.color = Color.gray;
				//}
			}
			potentialDestination.tile.obj.GetComponent<Renderer>().material.color = Color.blue;

			if (Input.GetMouseButtonDown(0))
			{
				//await move();
			}
		}
		// if potentialDestination is null(hover over some unwalckabale) we return the
		// oldDestination
	}

	public async Task move()
	{
		if (points.Count > 0)
		{
			Vector3 currentPoint = points[0];
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
					if (index >= points.Count)
					{
						//PathRequestManager.Instance.finishedProcessingPath();

						break;
					}
					dir = (points[index] - transform.position).normalized;
					currentPoint = points[index];
					transform.LookAt(currentPoint);
				}
				transform.position = Vector3.MoveTowards(transform.position, currentPoint, 5 * Time.deltaTime);
				RaycastHit hit;
				if (Physics.Raycast(point.position, transform.forward, out hit, 1))
				{
					Debug.Log($"{points.Count}");

					// modify the points array
					Vector3 hitPoint = hit.point;
					points.Insert(index, hitPoint);
					// look at top
					Vector3 top = new Vector3(hitPoint.x, 20, hitPoint.z);
					transform.LookAt(top);
					points.Insert(index + 1, hitPoint);
					index--;
					Debug.Log($"{points.Count}");
				}

				await Task.Yield();
			}
		}

		Debug.Log($"finish moving");
		//onActionFinish();
		await Task.Yield();
	}

	private LunchData calculateLunchVelocity()
	{
		float displacmentY = Mathf.Abs(Jumpeight - transform.position.y + target.position.y);
		Vector3 displacementXZ = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * displacmentY);
		float timeToreachDestination = (Mathf.Sqrt(-2 * displacmentY / Physics.gravity.y) + Mathf.Sqrt(-2 * Mathf.Abs(displacmentY + transform.position.y - target.position.y) / Physics.gravity.y));
		Vector3 velocityXZ = displacementXZ / timeToreachDestination;
		return new LunchData(velocityXZ + velocityY, timeToreachDestination);
	}

	public struct LunchData
	{
		public readonly Vector3 initialVelocity;
		public readonly float timeToTarget;

		public LunchData(Vector3 initialVelocity, float time)
		{
			this.initialVelocity = initialVelocity;
			timeToTarget = time;
		}
	}

	private Vector3[] DrowTrajectory()
	{
		LunchData data = calculateLunchVelocity();
		Vector3 previousDrowPoint = transform.position;
		List<Vector3> trajectory = new List<Vector3>();
		for (int i = 0; i < 30; i++)
		{
			float simulationTime = i / (float)30 * data.timeToTarget;
			Vector3 displacement = data.initialVelocity * simulationTime + Physics.gravity * simulationTime * simulationTime / 2f;
			Vector3 drowPoint = transform.position + displacement;
			lr.SetPosition(i, drowPoint);
			trajectory.Add(drowPoint);
			Debug.DrawLine(previousDrowPoint, drowPoint, Color.green);
			previousDrowPoint = drowPoint;
		}

		return trajectory.ToArray();
	}

	private async void jump()
	{
		Vector3[] trajectory = DrowTrajectory();
		//await move();
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < points.Count - 1; i++)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(points[i], points[i + 1]);
		}
	}
}
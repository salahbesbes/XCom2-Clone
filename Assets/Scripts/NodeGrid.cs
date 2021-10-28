using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
	public static NodeGrid Instance;
	public float nodeSize = 1;
	public Vector2 wordSizeGrid;
	public Transform playerPrefab;
	public LayerMask Unwalkable;
	public LayerMask nodeLayer;
	public LayerMask playerLayer;

	[HideInInspector]
	public Node[,] graph;

	[HideInInspector]
	public int width, height;

	[HideInInspector]
	public Vector3 buttonLeft;

	[HideInInspector]
	public Node destination, start;

	[HideInInspector]
	public float nodeRadius;

	[HideInInspector]
	public Vector3[] turnPoints;

	[HideInInspector]
	public List<Node> path = new List<Node>();

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			turnPoints = new Vector3[0];

			nodeRadius = nodeSize / 2;
			height = Mathf.RoundToInt(wordSizeGrid.x / nodeSize);
			width = Mathf.RoundToInt(wordSizeGrid.y / nodeSize);

			buttonLeft = transform.position - (Vector3.right * wordSizeGrid.x / 2) - (Vector3.forward * wordSizeGrid.y / 2);
			generateGrid();
			start = graph[0, 0];

			DontDestroyOnLoad(gameObject);
			transform.localScale = new Vector3((float)width / 10, 1, (float)height / 10);
			FindObjectOfType<Camera>().transform.position += new Vector3(0, 45 * (width / 50), 0);
		}
	}

	public void Update()
	{
		//// updating start node => tracking the player prefab
		start = getNodeFromTransformPosition(playerPrefab);
		//generateGrid();
	}

	/// <summary>
	/// this methode get the position of an GameObj and translate it to node coordonate and
	/// return the node, even if the player moves within a single node size the nethode will not
	/// return new node until the player exit this node
	/// </summary>
	/// <param name="prefab"> Transform obj </param>
	/// <returns> node </returns>
	public Node getNodeFromTransformPosition(Transform prefab)
	{
		if (prefab != null)
		{
			Vector3 pos = prefab.position;
			float posX = pos.x;
			float posY = pos.z;

			float percentX = Mathf.Floor(posX) + nodeRadius;
			float percentY = Mathf.Floor(posY) + nodeRadius;

			return GetNode(percentX, percentY);
		}
		return start;
	}

	public void resetGrid()
	{
		foreach (Node node in graph)
		{
			node.h = float.PositiveInfinity;
			node.g = float.PositiveInfinity;
			node.parent = null;
			//node.path = new List<Node>();
			node.isObstacle = Physics.CheckSphere(node.coord, nodeSize / 2, Unwalkable);
			node.color = node.isObstacle ? Color.red : Color.cyan;
			node.inRange = false;
			node.firstRange = false;
		}
	}

	/// <summary>
	/// move the unit toward the destination var sent from the grid to Gridpath var. this
	/// methode start on mouse douwn frame and the player start moving on the next frame until
	/// it reaches the goal. thats why we are using the carroutine. to simulate the update
	/// methode we use a while loop the problem is that the while loop is too rapid ( high
	/// frequency iteration) to iterate with the same frequence of the update methode we use
	/// yield return null or some other tools the wait for certain time "WaitForSeconds"
	/// </summary>
	/// <param name="unit"> Transform unit </param>
	/// <param name="path"> Array of position to </param>
	public IEnumerator followPath(Transform unit, Vector3[] path, float speed)
	{
		// yield break exit out the caroutine
		if (path.Length == 0) yield break;
		if (unit == null) yield break;

		Vector3 currentPoint = path[0];
		int index = 0;
		// this while loop simulate the update methode
		while (true)
		{
			if (unit.position == currentPoint)
			{
				index++;
				if (index >= path.Length)
				{
					yield break;
				}
				currentPoint = path[index];
			}

			unit.position = Vector3.MoveTowards(unit.position, currentPoint, speed * Time.deltaTime);
			// this yield return null waits until the next frame reached ( dont exit the
			// methode )
			yield return null;
		}
	}

	public Node getNodeFromMousePosition()
	{
		Plane plane = new Plane(Vector3.up, 0);
		float distance;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (plane.Raycast(ray, out distance))
		{
			// todo: we get the position of the mouse toward the grid we create we need
			// to implement the logic we need
			Vector3 worldPosition = ray.GetPoint(distance);
			if (worldPosition.x >= transform.position.x - wordSizeGrid.x / 2 && worldPosition.x <= wordSizeGrid.x / 2 + transform.position.x
				&& worldPosition.z >= transform.position.z - wordSizeGrid.y / 2 && worldPosition.z <= wordSizeGrid.y / 2 + transform.position.z)
			{
				float roundX = Mathf.Floor(worldPosition.x) + nodeRadius;
				float roundY = Mathf.Floor(worldPosition.z) + nodeRadius;
				Node selectedNode = GetNode(roundX, roundY);
				if (selectedNode.isObstacle == true) return null;
				else return selectedNode;
			}
		}
		return null;
	}

	public void MovePrefab()
	{
		if (destination != null && start != null)
		{
			if (destination == start)
			{
				Debug.Log($" cant click on same Node  ");
			}
			destination.color = Color.black;

			bool foundPath = FindPath.getPathToDestination(start, destination, out turnPoints, out path);

			if (foundPath)
			{
				StartCoroutine(followPath(playerPrefab, turnPoints, 30f));
				resetGrid();
			}
		}
	}

	private void generateGrid()
	{
		graph = new Node[width, height];
		//initialize graph
		for (int x = 0; x < height; x++)
		{
			for (int y = 0; y < width; y++)
			{
				Vector3 offset = new Vector3(nodeSize / 2, 0.1f, nodeSize / 2);
				Vector3 nodeCoord = buttonLeft + offset + Vector3.right * nodeSize * x + Vector3.forward * nodeSize * y;
				// create node
				graph[x, y] = new Node(nodeCoord, x, y);
				// project a sphere to check with the Layer Unwalkable or Player if
				// some thing above the node
				string[] collidableLayers = { "Unwalkable" };
				int layerToCheck = LayerMask.GetMask(collidableLayers);
				//graph[x, y].isObstacle = Physics.CheckSphere(nodeCoord, nodeSize / 2, layerToCheck);

				Collider[] hitColliders = Physics.OverlapSphere(nodeCoord, nodeSize / 2, layerToCheck);
				graph[x, y].isObstacle = hitColliders.Length > 0 ? true : false;
			}
		}

		//calculate neighbours
		for (int x = 0; x < height; x++)
		{
			for (int y = 0; y < width; y++)
			{
				//X is not 0, then we can add left (x - 1)
				if (x > 0)
				{
					graph[x, y].neighbours.Add(graph[x - 1, y]);
				}
				//X is not mapSizeX - 1, then we can add right (x + 1)
				if (x < height - 1)
				{
					graph[x, y].neighbours.Add(graph[x + 1, y]);
				}
				//Y is not 0, then we can add downwards (y - 1 )
				if (y > 0)
				{
					graph[x, y].neighbours.Add(graph[x, y - 1]);
				}
				//Y is not mapSizeY -1, then we can add upwards (y + 1)
				if (y < width - 1)
				{
					graph[x, y].neighbours.Add(graph[x, y + 1]);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		//Gizmos.DrawCube(transform.position, new Vector3(wordSizeGrid.x, 0.1f, wordSizeGrid.y));

		if (graph != null)
		{
			foreach (Node node in graph)
			{
				//string[] collidableLayers = { "Player", "Unwalkable" };
				string[] collidableLayers = { "Unwalkable" };
				int layerToCheck = LayerMask.GetMask(collidableLayers);

				Collider[] hitColliders = Physics.OverlapSphere(node.coord, nodeSize / 2, layerToCheck);
				node.isObstacle = hitColliders.Length > 0 ? true : false;
				node.color = node.isObstacle ? Color.red : node.inRange ? node.firstRange ? Color.yellow : Color.black : Color.cyan;
				//if (node.inRange && node.firstRange) node.color = Color.yellow;
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
				if (node == start) { node.color = Color.blue; }
				Gizmos.color = node.color;

				Gizmos.DrawCube(node.coord, new Vector3(nodeSize - 0.1f, 0.1f, nodeSize - 0.1f));
			}
		}
	}

	public Node GetNode(float i, float j)
	{
		for (int x = 0; x < height; x++)
		{
			for (int y = 0; y < width; y++)
			{
				if (Instance.graph != null)
				{
					if (Instance.graph[x, y].coord.x == i && Instance.graph[x, y].coord.z == j)
					{
						return Instance.graph[x, y];
					}
				}
			}
		}
		return null;
	}
}
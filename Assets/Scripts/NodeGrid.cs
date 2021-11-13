using System.Collections;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
	public static NodeGrid Instance;

	[HideInInspector]
	public Vector3 buttonLeft;

	//[HideInInspector]
	//public Node destination, start;

	[HideInInspector]
	public Node[,] graph;

	//private LayerMask nodeLayer;

	[HideInInspector]
	public float nodeRadius;

	public float nodeSize = 1;
	private LayerMask playerLayer;
	public Transform playerPrefab;
	private LayerMask Unwalkable;

	[HideInInspector]
	public int width, height;

	public Vector2 wordSizeGrid;

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

	/// <summary>
	/// this methode get the position of an GameObj and translate it to node coordonate and
	/// return the node, even if the player moves within a single node size the nethode will not
	/// return new node until the player exit this node
	/// </summary>
	/// <param name="prefab"> Transform obj </param>
	/// <returns> node </returns>
	public Node getNodeFromTransformPosition(Transform prefab, Vector3? vect3 = null)
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
		else if (prefab == null && vect3 != null)
		{
			float posX = vect3.Value.x;
			float posY = vect3.Value.z;

			float percentX = Mathf.Floor(posX) + nodeRadius;
			float percentY = Mathf.Floor(posY) + nodeRadius;

			return GetNode(percentX, percentY);
		}
		return null;
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

	public void Update()
	{
		//resetGrid();
	}

	//[HideInInspector]
	//public Vector3[] turnPoints;

	//[HideInInspector]
	//public List<Node> path = new List<Node>();

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			//turnPoints = new Vector3[0];

			nodeRadius = nodeSize / 2;
			height = Mathf.RoundToInt(wordSizeGrid.x / nodeSize);
			width = Mathf.RoundToInt(wordSizeGrid.y / nodeSize);

			buttonLeft = transform.position - (Vector3.right * wordSizeGrid.x / 2) - (Vector3.forward * wordSizeGrid.y / 2);
			generateGrid();
			//start = graph[0, 0];

			DontDestroyOnLoad(gameObject);
			transform.localScale = new Vector3((float)width / 10, 1, (float)height / 10);
			FindObjectOfType<Camera>().transform.position += new Vector3(0, 45 * (width / 50), 0);

			//nodeLayer = LayerMask.GetMask("Node");
			playerLayer = LayerMask.GetMask("Player");
			Unwalkable = LayerMask.GetMask("Unwalkable");
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
				// project a sphere to check with the Layer Unwalkable if some thing
				// with the layer Unwalkable above it
				string[] collidableLayers = { "Unwalkable", "Enemy", "Player" };
				int layerToCheck = LayerMask.GetMask(collidableLayers);
				//graph[x, y].isObstacle = Physics.CheckSphere(nodeCoord, nodeSize / 2, layerToCheck);

				Collider[] hitColliders = Physics.OverlapSphere(nodeCoord, nodeSize / 2, layerToCheck);
				graph[x, y].isObstacle = hitColliders.Length > 0 ? true : false;
			}
		}

		//calculate neighbours and create a cover for each node
		for (int x = 0; x < height; x++)
		{
			for (int y = 0; y < width; y++)
			{
				Node currentNode = graph[x, y];
				//X is not 0, then we can add left (x - 1)
				if (x > 0)
				{
					currentNode.neighbours.Add(graph[x - 1, y]);
					if (graph[x - 1, y].isObstacle == true)
					{
						currentNode.LeftCover = new Cover(true);
						currentNode.flinkedLeft = false;
					}
				}
				//X is not mapSizeX - 1, then we can add right (x + 1)
				if (x < height - 1)
				{
					currentNode.neighbours.Add(graph[x + 1, y]);
					if (graph[x + 1, y].isObstacle == true)
					{
						currentNode.RightCover = new Cover(true);
						currentNode.flinkedRight = false;
					}
				}
				//Y is not 0, then we can add downwards (y - 1 )
				if (y > 0)
				{
					currentNode.neighbours.Add(graph[x, y - 1]);
					if (graph[x, y - 1].isObstacle == true)
					{
						currentNode.DownCover = new Cover(true);
						currentNode.flinkedDown = false;
					}
				}
				//Y is not mapSizeY -1, then we can add upwards (y + 1)
				if (y < width - 1)
				{
					currentNode.neighbours.Add(graph[x, y + 1]);
					if (graph[x, y + 1].isObstacle == true)
					{
						currentNode.UpCover = new Cover(true);
						currentNode.flinkedUp = false;
					}
				}
			}
		}
	}
}
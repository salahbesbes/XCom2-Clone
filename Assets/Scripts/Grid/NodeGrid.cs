using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
	[Range(0.1f, 10)]
	public float nodeSize = 1;
	public int scale = 1;
	private LayerMask playerLayer;
	private LayerMask Unwalkable;

	[HideInInspector]
	public float width, height;

	//public Vector2 wordSizeGrid;
	public List<Tile> tiles { get; private set; } = new List<Tile>();
	public Transform quadHolder;
	public Tile GroundTilePrefab;
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
				if (graph != null)
				{
					if (graph[x, y].coord.x == i && graph[x, y].coord.z == j)
					{
						return graph[x, y];
					}
				}
			}
		}
		return null;
	}

	public Node getNodeFromMousePosition(Camera cam = null)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			//Debug.Log($" ui in the way  ");
			return null;
		}
		Plane plane = new Plane(Vector3.up, 0);
		float distance;

		if (cam == null) cam = Camera.main;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		if (plane.Raycast(ray, out distance))
		{
			// todo: we get the position of the mouse toward the grid we create we need
			// to implement the logic we need
			Vector3 worldPosition = ray.GetPoint(distance);
			if (worldPosition.x >= transform.position.x - width / 2 && worldPosition.x <= width / 2 + transform.position.x
				&& worldPosition.z >= transform.position.z - height / 2 && worldPosition.z <= height / 2 + transform.position.z)
			{
				float roundX = Mathf.Floor(worldPosition.x) + nodeRadius;
				float roundY = Mathf.Floor(worldPosition.z) + nodeRadius;
				Node selectedNode = GetNode(roundX, roundY);
				if (selectedNode != null && selectedNode.isObstacle == true) return null;
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

		if (Instance?.graph == null) return;
		foreach (Node node in graph)
		{
			Debug.DrawLine(node.coord, node.coord + Vector3.up, Color.red);
			node.h = float.PositiveInfinity;
			node.g = float.PositiveInfinity;
			node.parent = null;
			//node.path = new List<Node>();
			string[] collidableLayers = { "Unwalkable", "Player", "Enemy" };
			int layerToCheck = LayerMask.GetMask(collidableLayers);
			node.isObstacle = Physics.CheckSphere(node.coord, nodeSize / 2, layerToCheck);
			node.color = node.isObstacle ? Color.red : Color.white;
			node.inRange = false;
			node.firstRange = false;
			node.groundTile.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", node.color);
			node.tile.colliderOnTop = node.tile.getPrefabOnTopOfTheTile();


			//if (node.tile.obj != null) node.groundTile.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.black);
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
			//turnPoints = new Vector3[0];
			Instance = this;
			nodeRadius = nodeSize / 2;

			scale = (int)transform.localScale.z;
			height = Mathf.RoundToInt(transform.localScale.z * 10) / nodeSize;
			width = Mathf.RoundToInt(transform.localScale.x * 10) / nodeSize;
			graph = new Node[(int)width, (int)height];
			buttonLeft = transform.position - (Vector3.right * transform.localScale.x * 5) - (Vector3.forward * transform.localScale.z * 5);
			Debug.Log($"grid [{width},{height}]");
			generateGrid();
			//start = graph[0, 0];

			//DontDestroyOnLoad(gameObject);
			//transform.localScale = new Vector3((float)wordSizeGrid.x / 10, 1, (float)wordSizeGrid.y / 10);
			FindObjectOfType<Camera>().transform.position += new Vector3(0, 45 * (width / 50), 0);

			//nodeLayer = LayerMask.GetMask("Node");
			playerLayer = LayerMask.GetMask("Player");
			Unwalkable = LayerMask.GetMask("Unwalkable");
		}
	}

	private void generateGrid()
	{
		int count = 0;
		//initialize graph
		for (int x = 0; x < height; x++)
		{
			for (int y = 0; y < width; y++)
			{
				Vector3 offset = new Vector3(nodeSize / 2, 0, nodeSize / 2);
				Vector3 nodeCoord = buttonLeft + offset + Vector3.right * nodeSize * x + Vector3.forward * nodeSize * y;
				// create node
				graph[x, y] = new Node(nodeCoord, x, y);
				float quadSize = nodeSize / transform.localScale.x;

				Tile newTile = Instantiate(GroundTilePrefab, graph[x, y].coord, Quaternion.identity, quadHolder);
				newTile.node = graph[x, y];
				newTile.size = quadSize;
				newTile.transform.localScale = new Vector3(quadSize, quadSize, quadSize);
				Vector3 offsetCreation = new Vector3(0, -quadSize / 2 + 0.01f, 0);
				newTile.transform.position += offsetCreation;
				graph[x, y].groundTile = newTile.gameObject;
				graph[x, y].tile = newTile;

				//new Tile(graph[x, y], quadHolder, tiles, quadSize);
				// project a sphere to check with the Layer Unwalkable if some thing
				// with the layer Unwalkable above it
				string[] collidableLayers = { "Unwalkable", "Enemy", "Player", "Default" };
				int layerToCheck = LayerMask.GetMask(collidableLayers);
				//graph[x, y].isObstacle = Physics.CheckSphere(nodeCoord, nodeSize / 2, layerToCheck);

				Collider[] hitColliders = Physics.OverlapSphere(nodeCoord, nodeSize / 2, layerToCheck);

				foreach (var item in hitColliders)
				{
					if (item.CompareTag("mug")) graph[x, y].nodeCost = 10;
					else if (item.CompareTag("grass")) graph[x, y].nodeCost = 5;
					else graph[x, y].isObstacle = true;
				}
				count++;
				//graph[x, y].isObstacle = hitColliders.Length > 0 ? true : false;
			}
		}
		Debug.Log($"  {count} nodes");
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
				}
				//X is not mapSizeX - 1, then we can add right (x + 1)
				if (x < height - 1)
				{
					currentNode.neighbours.Add(graph[x + 1, y]);
				}
				//Y is not 0, then we can add downwards (y - 1 )
				if (y > 0)
				{
					currentNode.neighbours.Add(graph[x, y - 1]);
				}
				//Y is not mapSizeY -1, then we can add upwards (y + 1)
				if (y < width - 1)
				{
					currentNode.neighbours.Add(graph[x, y + 1]);
				}
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawCube(buttonLeft, Vector3.one);
		int localheight = Mathf.RoundToInt(transform.localScale.z * 10);
		int localwidth = Mathf.RoundToInt(transform.localScale.x * 10);
		buttonLeft = transform.position - (Vector3.right * transform.localScale.x * 5) - (Vector3.forward * transform.localScale.z * 5);
		//Debug.Log($"grid [{localwidth / nodeSize},{localheight / nodeSize}]");
		for (float x = 0; x < localheight; x += nodeSize)
		{
			Debug.DrawLine(buttonLeft + new Vector3(0, 0.00f, x), new Vector3(localwidth + buttonLeft.x, 0.00f, (x + buttonLeft.z)), new Color(0, 0, 0, 0.5f));
		}
		for (float x = 0; x < localwidth; x += nodeSize)
		{
			Debug.DrawLine(buttonLeft + new Vector3(x, 0.00f, 0), new Vector3(x + buttonLeft.x, 0.00f, (localheight + buttonLeft.z)), new Color(0, 0, 0, 0.5f));
		}

		//resetGrid();

		//foreach (var item in Instance.graph)
		//{
		//	Gizmos.color = item.color;
		//	Gizmos.DrawCube(item.coord, new Vector3(nodeSize - 0.1f, 0.1f, nodeSize - 0.1f));
		//}
		//if (GameStateManager.Instance?.SelectedUnit?.partToRotate != null)
		//{
		//	Transform points = GameStateManager.Instance.SelectedUnit.partToRotate.Find("points");

		//	foreach (Transform point in points)
		//	{
		//		Gizmos.color = Color.grey;
		//		Gizmos.DrawSphere(point.position, 0.5f);
		//	}
		//}
	}

	public Node getNode(int x, int y)
	{
		if (x >= 0 && y >= 0 && x < height && y < width)
		{
			return graph[x, y];
		}
		return null;
	}
}
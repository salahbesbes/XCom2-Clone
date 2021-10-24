using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
	public float nodeSize = 1;
	public Vector2 wordSizeGrid;
	public Transform playerPrefab;
	public LayerMask Unwalkable;
	public LayerMask nodeLayer;
	public LayerMask playerLayer;
	private Node[,] graph;
	private int width;
	private int height;
	private Vector3 buttonLeft;
	private Node destination;
	private Node start;
	private float nodeRadius;
	private List<Node> path;

	private void Awake()
	{
		path = new List<Node>();
		nodeRadius = nodeSize / 2;
		height = Mathf.RoundToInt(wordSizeGrid.x / nodeSize);
		width = Mathf.RoundToInt(wordSizeGrid.y / nodeSize);

		buttonLeft = transform.position - (Vector3.right * wordSizeGrid.x / 2) - (Vector3.forward * wordSizeGrid.y / 2);
		generateGrid();

		start = graph[0, 0];
		start.g = 0;
		//start.color = Color.blue;


	}

	public void Update()
	{
		// updating start node =>  tracking the player prefab
		start = getNodeFromTransformPosition(playerPrefab);

		if (Input.GetMouseButton(0))
		{
			getNodeFromMousePosition();
		}
	}

	/// <summary> this methode get the  position of an GameObj and translate it to node coordonate and return the node,
	/// even if the player moves within a single node size the nethode will not return new node until the player exit this node
	/// </summary>
	/// <param name="prefab"> Transform obj </param>
	/// <returns> node </returns>
	Node getNodeFromTransformPosition(Transform prefab)
	{
		Vector3 pos = prefab.position;
		float posX = pos.x;
		float posY = pos.z;

		float percentX = Mathf.Floor(posX) + nodeRadius;
		float percentY = Mathf.Floor(posY) + nodeRadius;

		return GetNode(percentX, percentY);

	}

	void resetGrid()
	{
		foreach (Node node in graph)
		{
			node.h = float.PositiveInfinity;
			node.g = float.PositiveInfinity;
			node.parent = null;
			//node.path = new List<Node>();
			node.isObstacle = Physics.CheckSphere(node.coord, nodeSize / 2, Unwalkable);
			node.color = node.isObstacle ? Color.red : Color.cyan;
		}
		// after resetting every single node in the graph 
		// we want to persist the previous start node
		//graph[start.x, start.y].color = Color.blue;
		graph[start.x, start.y].g = 0;

		// for every node in the path variable we want to change its color
		//foreach (Node node in path)
		//{
		//	node.color = Color.green;
		//}
		//graph[destination.x, destination.y].color = Color.black;

	}
	private Node getNodeFromMousePosition()
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
				if (selectedNode != null)
				{
					if (selectedNode.isObstacle == true)
					{
						return start;
					}
					destination = selectedNode;
					//destination.color = Color.black;

					FindPath.AStarAlgo(start, destination);
					resetGrid();
				}
			}
		}
		return start;
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
				// project a sphere to check with the Layer Unwalkable or Player if some thing
				// above the node
				string[] collidableLayers = { "Player", "Unwalkable" };
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


				node.color = node.isObstacle ? Color.red : Color.cyan;
				if (path.Contains(node))
				{
					node.color = Color.green;
				}
				if (node == destination)
				{
					node.color = Color.black;
				}
				if (node == start)
				{
					node.color = Color.blue;
				}
				Gizmos.color = node.color;

				Gizmos.DrawCube(node.coord, new Vector3(nodeSize - 0.1f, 0.1f, nodeSize - 0.1f));
				//Gizmos.DrawSphere(node.coord, nodeSize / 2);
			}
		}
	}



	private Node GetNode(float i, float j)
	{
		for (int x = 0; x < height; x++)
		{
			for (int y = 0; y < width; y++)
			{
				if (graph[x, y].coord.x == i && graph[x, y].coord.z == j)
				{
					return graph[x, y];
				}
			}
		}
		return null;
	}
}


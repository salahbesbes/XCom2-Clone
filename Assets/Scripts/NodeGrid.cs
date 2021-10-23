using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{
	public GameObject groundPlane;
	public Node[,] graph;
	public float nodeSize = 1;
	private int width;
	private int height;
	private Vector3 buttonLeft;
	public Vector2 wordSizeGrid;
	public LayerMask Unwalkable;
	public LayerMask nodeLayer;
	public Transform player;
	private Node destination;
	private Node start;

	private void Start()
	{
		height = Mathf.RoundToInt(wordSizeGrid.x / nodeSize);
		width = Mathf.RoundToInt(wordSizeGrid.y / nodeSize);

		buttonLeft = transform.position - (Vector3.right * wordSizeGrid.x / 2) - (Vector3.forward * wordSizeGrid.y / 2);
		generateGrid();
		start = graph[0, 0];
		start.g = 0;
		start.icon = "S";
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			fromWorldPosToGridPos(Input.mousePosition);
		}
	}

	private Node fromWorldPosToGridPos(Vector3 worldPos)
	{
		Plane plane = new Plane(Vector3.up, 0);
		float distance;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (plane.Raycast(ray, out distance))
		{
			// todo: we get the position of the mouse toward the grid we create we need
			// to implement the logic we need
			Vector3 worldPosition = ray.GetPoint(distance);
			if (worldPosition.x >= -wordSizeGrid.x / 2 + transform.position.x && worldPosition.x <= wordSizeGrid.x / 2 + transform.position.x
				&& worldPosition.y >= -wordSizeGrid.y / 2 && worldPosition.y <= wordSizeGrid.y / 2)
				Debug.Log($"{worldPosition}");
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
				graph[x, y] = new Node(nodeCoord);
				// project a sphere to check with the Layer Unwalkable if some thing
				// above the node
				graph[x, y].isObstacle = !Physics.CheckSphere(nodeCoord, nodeSize / 2, Unwalkable);
				Debug.Log($"{nodeCoord}");
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
				Gizmos.color = node.isObstacle ? Color.red : Color.cyan;
				Gizmos.DrawCube(node.coord, new Vector3(nodeSize - 0.1f, 0.1f, nodeSize - 0.1f));
				//Gizmos.DrawSphere(node.coord, nodeSize / 2);
			}
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(graph[0, 0].coord, new Vector3(nodeSize - 0.1f, 0.1f, nodeSize - 0.1f));
		}
	}

	public static float CalcG(Node a, Node b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}

	public static float CalcH(Node a, Node e)
	{
		return Mathf.Abs(a.x - e.x) + Mathf.Abs(a.y - e.y);
	}

	/// <summary>
	/// "using Graph dataType" grid of Nodes is needed to find the closest path between the
	/// start node to the destination node
	/// </summary>
	/// <param name="startNode"> selected Node </param>
	/// <param name="destination"> destination to reach </param>
	// link to visit https://www.youtube.com/watch?v=icZj67PTFhc
	public bool AStarAlgo(Node startNode, Node destination)
	{
		List<Node> QueueNotTested = new List<Node>();
		QueueNotTested.Insert(0, startNode);
		while (QueueNotTested.Count >= 0)
		{
			// sort the list in acending order by the heuretic value
			QueueNotTested = QueueNotTested.OrderBy(item => item.h).ToList();

			//we can loop throw an empty list in that case we are sure we didnt
			//find any path
			if (QueueNotTested.Count == 0)
			{
				Console.WriteLine("cant find path");
				return false;
			}

			// select the first node of the list which have the less value of the
			// heuritic value
			Node current = QueueNotTested[0];
			// make it visited
			current.visited = true;
			//remove it from the rhe list
			QueueNotTested.RemoveAt(0);

			// if the current == end we find the en point
			if (current == destination)
			{
				Console.WriteLine("Found End :) ");
				//printGrid(current);
				//getThePaht(startNode, current);
				break;
			}
			//printGrid(current);

			if (current.isObstacle == true) continue;

			for (int i = 0; i < current.neighbours.Count; i++)
			{
				// foreach neighbor which is not an obstacle
				Node neighbor = current.neighbours[i];
				if (neighbor.isObstacle == true) continue;
				else
				{
					// calculate the g val (toWard the parent)
					float neighborG = CalcG(current, neighbor);
					// calculate the new possible g value (toWard the start
					// node)
					float tempG = current.g + neighborG;
					// by default the neighbor.g is positif Infinit but after
					// setting g val to a neighbor we can revisit this node and
					// at this time the g val is not infinit so we wan do the
					// comparison

					if (tempG < neighbor.g)
					{
						neighbor.parent = current;
						neighbor.g = tempG;
						neighbor.h = neighbor.g + CalcH(neighbor, destination);
						// update the list we are worrking on
						QueueNotTested.Add(neighbor);
					}
				}
			}
		}
		return true;
	}
}

public class Node
{
	public float x;
	public float y;
	public Vector3 coord;
	public List<Node> neighbours;
	public bool isObstacle = false;
	public bool visited = false;
	public float h = float.PositiveInfinity;
	public float g = float.PositiveInfinity;
	public Node parent = null;
	public string icon = "*";
	public List<Node> path;

	public Node(Vector3 coord)
	{
		this.coord = coord;
		x = coord.x;
		y = coord.y;
		neighbours = new List<Node>();
		path = new List<Node>();
	}
}
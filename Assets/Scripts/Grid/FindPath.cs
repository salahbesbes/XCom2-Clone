using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FindPath
{
	/// <summary>
	/// "using Graph dataType" grid of Nodes is needed to find the closest path between the
	/// start node to the destination node
	/// </summary>
	/// <param name="startNode"> selected Node </param>
	/// <param name="destination"> destination to reach </param>
	// link to visit https://www.youtube.com/watch?v=icZj67PTFhc
	public static List<Node> AStarAlgo(Node startNode, Node destination)
	{
		startNode.g = 0;
		startNode.h = CalcH(startNode, destination);
		startNode.f = startNode.g + startNode.h;
		List<Node> openList = new List<Node>() { startNode };
		List<Node> closedLsit = new List<Node>();
		Node current;
		List<Node> res = new List<Node>();
		while (openList.Count > 0)
		{
			// first sort the list by nodeCost then by the h value (distance to the destination)
			// this give us the shortest path and not expansive
			if (NodeGrid.Instance.ActivateCostPath)
			{
				openList = openList.OrderBy(item => item.f).OrderBy(item => item.g).ToList();
			}
			else
			{
				openList = openList.OrderBy(item => item.g).ToList();
			}
			current = openList[0];


			if (current == destination)
			{
				res = getThePath(startNode, current);
				return res;
			}


			openList.Remove(current);
			closedLsit.Add(current);

			foreach (Node neighbour in current.neighbours)
			{
				if (closedLsit.Contains(neighbour) || neighbour.isUnwalkable) continue;

				float tmpG = current.g + CalcG(current, neighbour);
				// if the tmpG is less then the current G on the neighbour node
				if (neighbour.g > tmpG)
				{
					neighbour.g = tmpG;
					neighbour.parent = current;
					neighbour.h = CalcH(neighbour, destination);
					neighbour.f = neighbour.g + neighbour.h + neighbour.nodeCost;
					if (!openList.Contains(neighbour))
						openList.Add(neighbour);
				}
			}


		}

		Debug.Log($"cant find path in the map ");
		return res;

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
	/// create list of nodes of the shortest path between the start and end, start and end node
	/// included. save the path to the class variable GridPath
	/// </summary>
	/// <param name="startNode"> start node </param>
	/// <param name="current"> destiation </param>
	public static List<Node> getThePath(Node startNode, Node current)
	{
		Node tmp = current;
		// delete previous path
		List<Node> path = new List<Node>();
		startNode.path = new List<Node>();

		int pathCost = 0;
		while (tmp.parent != null)
		{
			// fill the path variable
			pathCost += tmp.nodeCost;
			path.Add(tmp);
			tmp = tmp.parent;
			tmp.color = Color.green;
		}
		path.Add(startNode);
		path.Reverse();

		startNode.path = path;
		if (NodeGrid.Instance.DemoScene && NodeGrid.Instance.pathCost != null)
		{
			NodeGrid.Instance.pathCost.text = $"Path cost: {pathCost} \n {path.Count} nodes";
		}
		else
		{
			//Debug.Log($"Path cost: {pathCost}, {path.Count} nodes");
		}
		return path;
	}

	/// <summary> return an array of position where the unit change direction </summary>
	/// <param name="path"> path between start and end nodes </param>
	/// <returns> array of position where the unit change direction </returns>
	public static Vector3[] createWayPoint(List<Node> path)
	{

		List<Vector3> pathPoint = new List<Vector3>();
		for (int i = 1; i < path.Count; i++)
		{
			Node currentNode = path[i];

			Vector3 point = currentNode.coord;

			if (currentNode.tile != null)
			{
				Node prevNode = path[i - 1];
				pathPoint.Add(prevNode.coord);
				Vector3 prevUP = new Vector3(prevNode.coord.x, 1, prevNode.coord.z);
				pathPoint.Add(prevUP);
			}

			pathPoint.Add(point);
			// todo: create a reference on the object sits ontop of the tile so that i know how tall he is

		}

		return pathPoint.ToArray();


	}
	public static Vector3[] createWayPointOriginal(List<Node> path)
	{
		Vector2 oldDirection = Vector2.zero;
		List<Vector3> wayPoints = new List<Vector3>();

		for (int i = 1; i < path.Count; i++)
		{
			Vector2 prevNodePos = new Vector2(path[i - 1].coord.x, path[i - 1].coord.z);
			Vector2 currentNodePos = new Vector2(path[i].coord.x, path[i].coord.z);

			Vector2 directionNew = currentNodePos - prevNodePos;
			if (directionNew != oldDirection)
			{
				wayPoints.Add(path[i - 1].coord);
			}

			oldDirection = directionNew;
		}
		Vector3 lastNodeCoord = path[path.Count - 1].coord;
		if (wayPoints.Contains(lastNodeCoord) == false)
		{
			wayPoints.Add(lastNodeCoord);
		}

		return wayPoints.ToArray();
	}
}


public class Node
{
	public Color color;
	public Vector3 coord;
	public bool firstRange = false;
	public float g = float.PositiveInfinity;
	public float h = float.PositiveInfinity;
	public float f = float.PositiveInfinity;
	public int nodeCost = 0;
	public bool inRange = false;
	public bool isUnwalkable = false;
	public bool isObstacle = false;
	public List<Node> neighbours;
	public Node parent = null;
	public List<Node> path;
	public bool visited = false;
	public int x;
	public int y;

	public Tile tile;

	public Node(Vector3 coord, int x, int y)
	{
		this.coord = coord;
		this.x = x;
		this.y = y;
		neighbours = new List<Node>();
		path = new List<Node>();
	}

	public override string ToString()
	{
		return $" node ({x}, {y}) ";
	}
}
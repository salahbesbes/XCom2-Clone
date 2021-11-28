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
		List<Node> QueueNotTested = new List<Node>();
		List<Node> result = new List<Node>();
		bool success = false;

		QueueNotTested.Insert(0, startNode);
		while (QueueNotTested.Count >= 0)
		{
			// sort the list in acending order by the heuretic value
			QueueNotTested = QueueNotTested.OrderBy(item => item.h).ToList();
			//we can loop throw an empty list in that case we are sure we didnt
			//find any path
			if (QueueNotTested.Count == 0)
			{
				Debug.Log($"cant find path ");
				success = false;
				break;
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
				//Debug.Log($"Found End :) ");
				result = getThePath(startNode, current);
				success = true;
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

		// we pass the result var to the get methode and we set the gridPath any way (empty
		// or full)
		return result;
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
	/// get the path from the end node to start node, and make the unit prefab moves toward that
	/// destination folowing that path
	/// </summary>
	/// <param name="unit"> the prefab </param>
	/// <param name="currentUnitNode"> start node </param>
	/// <param name="endUnitNode"> destination </param>
	/// <param name="turnPoints"> class variable sent from Grid to save turn Points </param>
	/// <param name="gridPath"> class variable sent from the Grid to save the Hole path </param>
	/// <returns> </returns>
	public static List<Node> getPathToDestination(Node currentUnitNode, Node endUnitNode)
	{
		return AStarAlgo(currentUnitNode, endUnitNode);
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
		while (tmp.parent != null)
		{
			// fill the path variable
			path.Add(tmp);
			tmp = tmp.parent;
			tmp.color = Color.green;
		}
		path.Add(startNode);
		path.Reverse();

		startNode.path = path;

		return path;
	}

	/// <summary> return an array of position where the unit change position </summary>
	/// <param name="path"> path between start and end nodes </param>
	/// <returns> array of position where the unit change direction </returns>
	public static Vector3[] createWayPoint(List<Node> path)
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
	public bool inRange = false;
	public bool isObstacle = false;
	public List<Node> neighbours;
	public Node parent = null;
	public List<Node> path;
	public bool visited = false;
	public int x;
	public int y;

	public bool flinkedLeft = true;
	public bool flinkedRight = true;
	public bool flinkedUp = true;
	public bool flinkedDown = true;
	public bool sameline = false;

	public Cover UpCover;
	public Cover DownCover;
	public Cover LeftCover;
	public Cover RightCover;

	public GameObject quad;
	public Transform tile;

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
		return $" ({x}, {y}) ";
	}
}

public struct Cover
{
	private float _value;
	public bool Exist { get; set; }

	public float Value
	{
		get { return _value; }
		set { _value = value; }
	}

	public Cover(bool exist = false, float coverValue = 45f)
	{
		_value = 0;
		Exist = exist;
		Value = coverValue;
	}

	public override string ToString()
	{
		return $"cover exist {Exist} => to check existance use .Exist bool";
	}
}
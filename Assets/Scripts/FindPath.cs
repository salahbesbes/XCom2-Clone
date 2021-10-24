using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class FindPath : MonoBehaviour
{
	public static void StartFindPath(Node startNode, Node destination)
	{
		//coroutine = AStarAlgo(startNode, destination);
		//StartCoroutine("AStarAlgo", startNode, destination);
		StartCoroutine(AStarAlgo(startNode, destination));
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
	static IEnumerator AStarAlgo(Node startNode, Node destination)
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
				Debug.Log("cant find path");
				yield return null;
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
				Debug.Log("Found End :) ");
				//printGrid(current);
				//yield return getThePath(startNode, current);
				yield return null;
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
		yield return null;
	}

	/// <summary> save the shortest path between the start and end, start and end node included </summary>
	/// <param name="startNode"> start node </param>
	/// <param name="current"> destiation </param>
	public static List<Node> getThePath(Node startNode, Node current)
	{
		Node tmp = current;
		// delete previous path
		List<Node> path = new List<Node>();
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
}


public class Node
{
	public int x;
	public int y;
	public Vector3 coord;
	public List<Node> neighbours;
	public bool isObstacle = false;
	public bool visited = false;
	public float h = float.PositiveInfinity;
	public float g = float.PositiveInfinity;
	public Node parent = null;
	public Color color;
	public List<Node> path;

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

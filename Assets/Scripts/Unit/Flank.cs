using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flank : MonoBehaviour
{
	private Transform targetPoints;
	private Transform MyPoints;
	private Transform targetTop;
	private Transform targetLeft;
	private Transform targetRight;

	private Transform myTop;
	private Transform myLeft;
	private Transform myRight;

	public AnyClass thisPlayer;
	private bool foundOneMatch = false;

	private void Start()
	{
		//targetPoints = thisPlayer.currentTarget.pointsRayCast;
		//targetRight = targetPoints.Find("RightPoint");
		//targetLeft = targetPoints.Find("LeftPoint ");
		//targetTop = targetPoints.Find("TopPoint");

		//MyPoints = thisPlayer.pointsRayCast;
		myRight = MyPoints.Find("RightPoint");
		myLeft = MyPoints.Find("LeftPoint");
		myTop = MyPoints.Find("TopPoint");

		// todo: to avoid wrong target collision when shooting we can make the model crouche
		// when he is not being targeting, when he is he stand up or maybe adding rb to the
		// model and playing with some config

		//visibleTiles();
	}

	private void Update()
	{
		//targetPoints = thisPlayer.currentTarget.pointsRayCast;
		//targetRight = targetPoints.Find("RightPoint");
		//targetLeft = targetPoints.Find("LeftPoint");
		//targetTop = targetPoints.Find("TopPoint");
		//int layer = ~LayerMask.GetMask("");
		////Ray topPos = new Ray(child.position, Vector3.forward);
		////Ray rightPos = new Ray(child.position, child.right);
		////Ray leftPos = new Ray(child.position, -child.right);
		//RaycastHit hit;
		//foundOneMatch = false;
		//if (Physics.Raycast(myLeft.position, targetRight.position - myLeft.position, out hit, Mathf.Infinity, layer))
		//{
		//	if (hit.transform.tag == "point")
		//	{
		//		foundOneMatch = true;
		//		Debug.DrawRay(myLeft.position, targetRight.position - myLeft.position, Color.red);
		//		//thisPlayer.currentTarget.currentPos.tile.GetComponent<Renderer>().material.color = Color.cyan;
		//	}
		//}
		//if (foundOneMatch == false)
		//	visibleTiles();
		//if (Physics.Raycast(myRight.position, targetLeft.position - myRight.position, out hit, Mathf.Infinity, layer))
		//{
		//	if (hit.transform.tag == "point")
		//	{
		//		foundOneMatch = true;
		//		Debug.DrawRay(myRight.position, targetLeft.position - myRight.position, Color.blue);
		//		//thisPlayer.currentTarget.currentPos.tile.GetComponent<Renderer>().material.color = Color.cyan;
		//	}
		//}
		//if (foundOneMatch == false)
		//	visibleTiles();
		//if (Physics.Raycast(myTop.position, targetTop.position - myTop.position, out hit, Mathf.Infinity, layer))
		//{
		//	if (hit.transform.tag == "point")
		//	{
		//		foundOneMatch = true;
		//		Debug.DrawRay(myTop.position, targetTop.position - myTop.position, Color.cyan);
		//		//thisPlayer.currentTarget.currentPos.tile.GetComponent<Renderer>().material.color = Color.cyan;
		//	}
		//}
		//if (foundOneMatch == false)
		//	visibleTiles();

		//if (Physics.Raycast(rightPos, 10, layer))
		//{
		//	Debug.DrawRay(child.position, child.right, Color.blue);
		//};
		//if (Physics.Raycast(leftPos, 10, layer))
		//{
		//	Debug.DrawRay(child.position, -child.right, Color.green);
		//};
		//int layer = LayerMask.GetMask("box", "Unwalkable");
		//foreach (Transform child in targetPoints)
		//{
		//	Ray topPos = new Ray(child.position, child.forward);
		//	Ray rightPos = new Ray(child.position, child.right);
		//	Ray leftPos = new Ray(child.position, -child.right);
		//	RaycastHit hit;
		//	if (Physics.Raycast(child.position, child.forward, out hit, Mathf.Infinity, layer))
		//	{
		//		Debug.DrawRay(child.position, child.forward, Color.red);
		//		Debug.Log($"{hit.collider.name}");
		//	};
		//	if (Physics.Raycast(rightPos, 10, layer))
		//	{
		//		Debug.DrawRay(child.position, child.right, Color.blue);
		//	};
		//	if (Physics.Raycast(leftPos, 10, layer))
		//	{
		//		Debug.DrawRay(child.position, -child.right, Color.green);
		//	};
		//}

		//foreach (Transform child in targetPoints)
		//{
		//	Ray topPos = new Ray(playerSource.position, child.position - playerSource.position);
		//	Ray rightPos = new Ray(playerSource.position, child.position - playerSource.position);
		//	Ray leftPos = new Ray(playerSource.position, child.position - playerSource.position);
		//	RaycastHit hit;

		// int layer = LayerMask.GetMask("box", "Unwalkable");

		//	if (Physics.Raycast(playerSource.position, child.position - playerSource.position, out hit, Mathf.Infinity, layer))
		//	{
		//		if (hit.collider.name == "box")
		//		{
		//			Debug.DrawRay(playerSource.position, child.position - playerSource.position, Color.black);
		//			Debug.Log($"{hit.collider.name}");
		//		}
		//	}
		//}
	}

	private void visibleTiles()
	{
		int layer = ~LayerMask.GetMask("", "box");

		RaycastHit hit;
		Debug.DrawRay(myTop.position, myTop.forward, Color.red);
		if (Physics.Raycast(myTop.position, myTop.forward, out hit, int.MaxValue, layer))
		{
			NodeGrid grid = NodeGrid.Instance;
			Node res = grid.getNodeFromTransformPosition(hit.transform);
			res.tile.obj.GetComponent<Renderer>().material.color = Color.cyan;

			List<Node> groupOfObstacles = selectVisibleTiles(res, new List<Node>());

			Node[] obstaclesEdges = groupOfObstacles.Where(el => lambda(el)).ToArray();

			foreach (Node edge in obstaclesEdges)
			{
				foreach (var neibour in edge.neighbours)
				{
					if (!neibour.isObstacle)
						neibour.tile.obj.GetComponent<Renderer>().material.color = Color.yellow;
				}
			}
		}
	}

	private bool lambda(Node node)
	{
		int count = 0;
		foreach (Node nei in node.neighbours)
		{
			if (!nei.isObstacle)
				count++;
		}
		return count == 3;
	}

	private List<Node> selectVisibleTiles(Node node, List<Node> obstacles)
	{
		obstacles.Add(node);
		node.tile.obj.GetComponent<Renderer>().material.color = Color.red;

		foreach (Node neihbour in node.neighbours)
		{
			if (neihbour.isObstacle && !obstacles.Contains(neihbour))
				selectVisibleTiles(neihbour, obstacles);
			if (!neihbour.isObstacle)
				neihbour.tile.obj.GetComponent<Renderer>().material.color = Color.cyan;
		}
		return obstacles;
	}

	private void OnDrawGizmos()
	{
		int layer = LayerMask.NameToLayer("Enemy");
		//Physics.OverlapSphere(transform.position, 2f, layer);
		//Gizmos.color = new Color(1, 1, 1, 0.3f);
		//Gizmos.DrawCube(new Vector3(transform.position.x, 0.5f, transform.position.z), new Vector3(1.5f, 1.5f, 1.5f));

		//Gizmos.DrawRay(top.position, top.forward);
		//Gizmos.DrawRay(Right.position, Right.right);
		//Gizmos.DrawRay(Left.position, -Left.right);
		//Gizmos.DrawRay(bottom.position, -bottom.forward);
	}
}
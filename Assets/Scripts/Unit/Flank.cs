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

	private void Start()
	{
		//targetPoints = thisPlayer.currentTarget.pointsRayCast;
		//targetRight = targetPoints.Find("RightPoint");
		//targetLeft = targetPoints.Find("LeftPoint ");
		//targetTop = targetPoints.Find("TopPoint");

		MyPoints = thisPlayer.pointsRayCast;
		myRight = MyPoints.Find("RightPoint");
		myLeft = MyPoints.Find("LeftPoint");
		myTop = MyPoints.Find("TopPoint");



		// todo: to avoid wrong target collision when shooting we can make the model crouche when he is not being targeting, when he is he stand up
		// or maybe  adding rb to the model and playing  with some config

	}

	private void Update()
	{
		targetPoints = thisPlayer.currentTarget.pointsRayCast;
		targetRight = targetPoints.Find("RightPoint");
		targetLeft = targetPoints.Find("LeftPoint ");
		targetTop = targetPoints.Find("TopPoint");
		int layer = ~LayerMask.GetMask("", "box");
		//Ray topPos = new Ray(child.position, Vector3.forward);
		//Ray rightPos = new Ray(child.position, child.right);
		//Ray leftPos = new Ray(child.position, -child.right);
		RaycastHit hit;
		if (Physics.Raycast(myLeft.position, targetRight.position - myLeft.position, out hit, Mathf.Infinity, layer))
		{
			Debug.Log($"some thing");
			if (hit.transform.tag == "point")
			{
				Debug.DrawRay(myLeft.position, targetRight.position - myLeft.position, Color.red);
				thisPlayer.currentTarget.currentPos.tile.GetComponent<Renderer>().material.color = Color.cyan;
			}
		};
		if (Physics.Raycast(myRight.position, targetLeft.position - myRight.position, out hit, Mathf.Infinity, layer))
		{
			if (hit.transform.tag == "point")
			{
				Debug.DrawRay(myRight.position, targetLeft.position - myRight.position, Color.blue);
				thisPlayer.currentTarget.currentPos.tile.GetComponent<Renderer>().material.color = Color.cyan;

			}
		};
		if (Physics.Raycast(myTop.position, targetTop.position - myTop.position, out hit, Mathf.Infinity, layer))
		{
			if (hit.transform.tag == "point")
			{
				Debug.DrawRay(myTop.position, targetTop.position - myTop.position, Color.cyan);
				thisPlayer.currentTarget.currentPos.tile.GetComponent<Renderer>().material.color = Color.cyan;

			}
		};
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
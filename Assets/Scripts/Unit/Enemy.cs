using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : PlayerClass
{
	public bool isFlanked = false;
	protected GameStateManager gameStateManager;
	protected Player currentTarget;

	private void Awake()
	{
		grid = FindObjectOfType<NodeGrid>();
		queueOfActions = new Queue<ActionBase>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
		Debug.Log($"start enemy {currentPos}");
		gameStateManager = FindObjectOfType<GameStateManager>();
		currentTarget = gameStateManager.selectedPlayer;
	}

	private void Update()
	{
		currentPos = grid.getNodeFromTransformPosition(transform);

		//NodeCoord = grid.getNodeFromTransformPosition(transform);
		//if (Input.GetKeyDown(KeyCode.LeftShift))
		//{
		//	SelectNextPlayer();
		//}
		//currentPos = grid.getNodeFromTransformPosition(transform);

		//if (Input.GetMouseButtonDown(0))
		//{
		//	Debug.Log($"mouse down");
		//	CreateNewMoveAction();
		//}
		//if (Input.GetMouseButtonDown(1))
		//{
		//	CreateNewShootAction();
		//}
		//if (Input.GetKeyDown(KeyCode.R))
		//{
		//	CreateNewReloadAction();
		//}
		//LockOnTarger();
		//checkFlank(currentTarget?.currentPos);
	}

	private bool checkPointIfSameLineOrColumAsTarget(Vector3 target, Vector3 pointNode)
	{
		if (pointNode != null)
		{
			if (target.x == pointNode.x || target.z == pointNode.z)
			{
				return true;
			}
		}
		return false;
	}

	public void checkFlank(Node target)
	{
		if (currentPos == null) return;

		Transform points = transform.Find("Points");
		Vector3 selectedPointCood = Vector3.zero;
		Vector3 selectedPoint;
		if (target != null && points != null)
		{
			Dictionary<Vector3, float> ordredDictByMagnitude = new Dictionary<Vector3, float>();

			for (int i = 0; i < points.childCount; i++)
			{
				Transform point = points.GetChild(i);
				float mag = (target.coord - point.position).magnitude;
				ordredDictByMagnitude.Add(point.position, mag);
			}

			ordredDictByMagnitude = ordredDictByMagnitude.OrderBy((item) => item.Value)
									.ToDictionary(t => t.Key, t => t.Value);

			// default node is the nearest one to the target (first one in the dict)
			Vector3 defaultPoint = ordredDictByMagnitude.First().Key;
			selectedPoint = defaultPoint;

			bool foundPotentialPositionToFlank = false;
			foreach (var item in ordredDictByMagnitude)
			{
				Vector3 point = item.Key;
				if (checkPointIfSameLineOrColumAsTarget(target.coord, point))
				{
					foundPotentialPositionToFlank = true;
					// update selected Point Coord
					selectedPoint = point;
					break;
				}
			}
			if (foundPotentialPositionToFlank)
			{
				if (defaultPoint == selectedPoint)
				{
					selectedPoint = ordredDictByMagnitude.ElementAt(1).Key;
				}
				CheckForTargetWithRayCast(selectedPoint, target.coord);
			}
			else
			{
				CheckForTargetWithRayCast(defaultPoint, target.coord);
			}
		}
	}

	public void CheckForTargetWithRayCast(Vector3 pointPosition, Vector3 targetPosition)
	{
		RaycastHit hit;
		Vector3 dir = targetPosition - pointPosition;
		string[] collidableLayers = { "Unwalkable", "Enemy" };
		int layerToCheck = LayerMask.GetMask(collidableLayers);
		// if i can see the player directly without any Obstacle in between => im flanking
		// it else if i see some thing other the enemy in the way => im not
		/* note i dont understand why layer to check is not working properly, if the collidableLayers
		// contain 2 layer, the raycast should detect only object with those layer, and the
		// first object the ray hit it return that hit object, but it doesnot do i thought,
		// when it found an "unwalckable" object the ray conteniou and detect the "Enemy".
		// in my case since im always looking at the enemy the raycast always return True
		// even if the obsacle is in front of the enemy
		*/
		if (Physics.Raycast(pointPosition, dir, out hit, layerToCheck))
		{
			// to compaire the layer of the object we hit to the "Enemy" layer.
			// LayerMask return an bitMask int type different to the gameObject.layer
			// int type => (index) convert index of the layer Enemy to the BitMast type
			// to compair it
			if ((LayerMask.GetMask("Enemy") & 1 << hit.transform.gameObject.layer) != 0)
				currentTarget.isFlanked = true;
			else
				currentTarget.isFlanked = false;
		}
		else
		{
			currentTarget.isFlanked = false;
		}
		Debug.DrawRay(pointPosition, dir, Color.yellow);
	}

	public void LockOnTarger()
	{
		if (currentPos == null) return;
		if (currentTarget == null) return;
		if (currentPos != null && currentTarget?.currentPos != null)
		{
			// handle rotation on axe Y
			Vector3 dir = currentTarget.currentPos.coord - currentPos.coord;
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			// smooth the rotation of the turrent
			Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
							lookRotation,
							Time.deltaTime * 5f)
							.eulerAngles;
			partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
		}
	}

	public void SelectNextPlayer()
	{
		List<Player> players = gameStateManager.players;
		int nbPlyaers = players.Count;
		Debug.Log($"{nbPlyaers}");
		if (currentTarget != null)
		{
			int currentTargetIndex = players.FindIndex(instance => instance == currentTarget);
			currentTarget = players[(currentTargetIndex + 1) % nbPlyaers];
		}
	}

	public override string ToString()
	{
		return $"Enemy {transform.name} is selected  at position {currentPos?.coord}";
	}
}

public class Charachter : MonoBehaviour
{
}
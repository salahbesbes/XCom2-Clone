using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AnyClass))]
public class CoverLogic : MonoBehaviour
{
	private AnyClass unit;
	private GameStateManager gameManger;
	private Node lastKnownPosition;
	private NodeGrid grid;

	public Node front;
	public Node back;
	public Node right;
	public Node left;

	private List<NewCover> covers = new List<NewCover>();
	internal bool alreadyFluncked = false;

	public int CoverValue
	{
		get
		{
			int sum = covers.Select(el => el.Value).DefaultIfEmpty()
					.Aggregate((total, next) => total + next);

			return sum;
		}
	}

	private void Start()
	{
		grid = NodeGrid.Instance;
		gameManger = GameStateManager.Instance;
		unit = GetComponent<AnyClass>();
		UpdateNorthPositionTowardTarget(gameManger.SelectedUnit);
		CalculateCoverValue();

		lastKnownPosition = grid.getNodeFromTransformPosition(gameManger.SelectedUnit.transform);
		unit.CoverBihaviour = this;
	}

	private async void Rotate()
	{
		await unit.rotateTowardDirection(unit.partToRotate, gameManger.SelectedUnit.transform.position - unit.transform.position, 0.4f);
		// I need to wait full rotation so that I can update the new node direction
		unit.CurrentTarget = gameManger.SelectedUnit;
	}

	private void Update()
	{
		if (gameManger.SelectedUnit != unit)
		{
			if (gameManger.SelectedUnit.currentPos != lastKnownPosition)
			{
				Rotate();
				UpdateNorthPositionTowardTarget(gameManger.SelectedUnit);
				CalculateCoverValue();

				lastKnownPosition = gameManger.SelectedUnit.currentPos;
			}
		}
	}

	public void UpdateNorthPositionTowardTarget(AnyClass target)
	{
		if (unit.currentPos == null)
		{
			unit.currentPos = grid.getNodeFromTransformPosition(unit.transform);
		}

		if (target == null || unit.currentPos == null)
		{
			Debug.Log($"{unit.stats.name} has {target} or {unit.currentPos} null");
			return;
		}
		Transform points = unit.partToRotate.Find("points");

		front = NodeGrid.Instance.getNodeFromTransformPosition(points.GetChild(0));
		back = NodeGrid.Instance.getNodeFromTransformPosition(points.GetChild(1));
		right = NodeGrid.Instance.getNodeFromTransformPosition(points.GetChild(2));
		left = NodeGrid.Instance.getNodeFromTransformPosition(points.GetChild(3));

		if (checkForDiagonal(front))
			front = grid.getNode(unit.currentPos.x, front.y);
		if (checkForDiagonal(back))
			back = grid.getNode(unit.currentPos.x, back.y);
		if (checkForDiagonal(right))
			right = grid.getNode(right.x, unit.currentPos.y);
		if (checkForDiagonal(left))
			left = grid.getNode(left.x, unit.currentPos.y);

		if (front != null)
			front.tile.obj.GetComponent<Renderer>().material.color = Color.yellow;
		else
			Debug.Log($"front is null");

		if (back != null)
			back.tile.obj.GetComponent<Renderer>().material.color = Color.red;
		else
			Debug.Log($"back is null");

		if (right != null)
			right.tile.obj.GetComponent<Renderer>().material.color = Color.blue;
		else
			Debug.Log($"right is null");

		if (left != null)
			left.tile.obj.GetComponent<Renderer>().material.color = Color.green;
		else
			Debug.Log($"left is null");
		//Debug.Log($"front {front}, back {back}, right {right}, left {left} player {currentPos}");
	}

	public void CalculateCoverValue()
	{
		// clearn previous Covers
		covers.Clear();

		// add new Covers
		if (front.tile.colliderOnTop != null)
		{
			covers.Add(new NewCover(CoverDirection.front, CoverType.low, front));
			//Debug.Log($"{unit.name} has cover on the FRONT node ");
		}
		if (right.tile.colliderOnTop != null)
		{
			covers.Add(new NewCover(CoverDirection.right, CoverType.low, right));
			//Debug.Log($"{unit.name} has cover on the RIGHT node ");
		}
		if (left.tile.colliderOnTop != null)
		{
			covers.Add(new NewCover(CoverDirection.left, CoverType.low, left));
			//Debug.Log($"{unit.name} has cover on the LEFT node ");
		}
		//Debug.Log($" {unit.name} has Cover Value => {CoverValue}");
	}

	private bool checkForDiagonal(Node node)
	{
		if (node == null) return false;
		return Mathf.Abs(unit.currentPos.x - node.x) == Mathf.Abs(unit.currentPos.y - node.y);
	}
}
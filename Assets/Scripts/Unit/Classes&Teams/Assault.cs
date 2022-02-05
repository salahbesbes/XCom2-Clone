using System.Collections.Generic;
using UnityEngine;

public class Assault : PlayerStateManager, IClassAssault
{
	private Ak47 myWeapon;

	public void Start()
	{
		grid = NodeGrid.Instance;
		gameStateManager = GameStateManager.Instance;

		currentPos = grid.getNodeFromTransformPosition(transform);
		queueOfActions = new Queue<ActionBase>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
		currentPos = grid.getNodeFromTransformPosition(transform);
		animator = model.GetComponent<Animator>();
		stats = GetComponent<Stats>();
		myWeapon = weapon as Ak47;
		enabled = this == gameStateManager.SelectedUnit ? true : false;
	}
}

internal interface IClassAssault
{
}

internal interface IClassRanger
{
}

internal interface IClassGrenadier
{
	public void CreateLunchGrenadeAction();

	public void LunchGrenadeCallback(LunchGrenadeAction action, Node destination);
}

public interface IBaseActions
{
	public void CreateNewMoveAction();

	public void CreateNewReloadAction();

	public void CreateNewShootAction();
}
using System.Collections.Generic;
using UnityEngine;

public class Grenadier : PlayerStateManager, IClassGrenadier
{
	private HeavyWeapon myWeapon;

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
		myWeapon = weapon as HeavyWeapon;
		enabled = this == gameStateManager.SelectedUnit ? true : false;
	}


	public override void customUpdate()
	{
		weapon.onUpdate();
	}

	public void CreateLunchGrenadeAction()
	{
		Node potentialDestination = NodeGrid.Instance.getNodeFromMousePosition(secondCam);
		LunchGrenadeAction action = new LunchGrenadeAction(LunchGrenadeCallback, "LunchGrenade", potentialDestination);
		Enqueue(action);
	}

	public void LunchGrenadeCallback(LunchGrenadeAction action, Node dest)
	{
		myWeapon.lunchGrenade(action, dest);
	}
}
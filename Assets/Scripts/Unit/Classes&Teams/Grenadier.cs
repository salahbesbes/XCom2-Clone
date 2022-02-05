using System.Collections.Generic;
using System.Linq;
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

	public override void onHover()
	{
		Node potentialDestination = NodeGrid.Instance.getNodeFromMousePosition(secondCam);
		if (potentialDestination != null && potentialDestination != destination && potentialDestination != currentPos)
		{
			//lineConponent.SetUpLine(turnPoints);

			potentialDestination.tile.obj.GetComponent<Renderer>().material.color = Color.blue;
			myWeapon.DrowTrajectory(potentialDestination.coord);
			if (Input.GetMouseButtonDown(0))
			{
				ActionData action = actions.FirstOrDefault((el) => el is LunchGrenadeData);
				currentActionAnimation = AnimationType.shoot;
				SwitchState(doingAction);
				action?.Actionevent?.Raise();
			}
		}
	}

	public override void customUpdate()
	{
		onHover();
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
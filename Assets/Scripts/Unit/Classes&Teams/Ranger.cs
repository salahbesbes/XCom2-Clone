using System.Collections.Generic;
using UnityEngine;

public class Ranger : PlayerStateManager, IClassRanger
{
	private ShutGun myWeapon;

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
		myWeapon = weapon as ShutGun;
		enabled = this == gameStateManager.SelectedUnit ? true : false;
	}


	public override void customUpdate()
	{
		weapon.onUpdate();
	}
}
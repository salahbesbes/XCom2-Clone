using gameEventNameSpace;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnyClass : Unit
{
	public Transform hand;
	public Inventory inventory;
	public List<ActionData> actions = new List<ActionData>();
	public Team team;
	public Stats stats;
	public Transform ActionHolder;
	public GameObject Action_prefab;
	public Transform HealthBarHolder;
	public GameObject listners;
	public Camera fpsCam;
	public Camera secondCam;
	protected GameStateManager gameStateManager;

	public Transform aimPoint;
	public bool isFlanked;

	[HideInInspector]
	public List<Transform> sportPoints;

	public VoidEvent onChangeTarget;
	public NotifyGameManagerEvent notifyGameManagerEvent;
	public UpdateInventoryEvent onUpdateInventoryEvent;
	public VoidEvent openInventory;
	public Item newWeapon;
	private float _targetAimValue;
	public Weapon weapon;

	public Direction targetDirection;
	public CoverLogic CoverBihaviour;
	public FlunckDirection flunckTargetTop;
	public FlunckDirection flunckTargetRight;
	public FlunckDirection flunckTargetLeft;

	public List<Material> modelMaterials = new List<Material>();

	public void Start()
	{
		grid = NodeGrid.Instance;
		gameStateManager = GameStateManager.Instance;
		flunckTargetTop = flunckTargetRight = flunckTargetLeft = FlunckDirection.None;

		currentPos = grid.getNodeFromTransformPosition(transform);
		queueOfActions = new Queue<ActionBase>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
		animator = model.GetComponent<Animator>();
		stats = GetComponent<Stats>();
		CoverBihaviour.UpdateNorthPositionTowardTarget(gameStateManager.SelectedUnit);
		CoverBihaviour.CalculateCoverValue();


		if (this != gameStateManager.SelectedUnit)
		{
			//TheNorth(gameStateManager.SelectedUnit);
			enabled = false;
		}
		else
		{
		}
	}

	public float TargetAimPercent
	{
		get => _targetAimValue;
		set
		{
			value = Mathf.Clamp(value, 0, int.MaxValue);
			_targetAimValue = value;
		}
	}

	public AnyClass CurrentTarget
	{
		get => _currentTarger;
		set
		{
			_currentTarger?.stopGlowing();
			if (GameStateManager.Instance.SelectedUnit != value)
			{
				GameStateManager.Instance.clearPreviousSelectedUnitFromAllWeaponEvent(_currentTarger);
				GameStateManager.Instance.clearPreviousSelectedUnitFromAllBoolEvent(_currentTarger);

				_currentTarger = value;
				//CoverBihaviour.UpdateNorthPositionTowardTarget(value);
				//CheckForFlunks();

				rotateTowardDirection(partToRotate, _currentTarger.aimPoint.position - transform.position);

				GameStateManager.Instance.MakeOnlySelectedUnitListingToWeaponEvent(_currentTarger, stats?.onWeaponFinishShooting);
				GameStateManager.Instance.MakeOnlySelectedUnitListingToBoolEvent(_currentTarger, stats?.FlunckingTarget);
			}
			else // if the Target is the selected unit we dont want to mess with the listener neither rotate it
			{
				_currentTarger = value;
			}
		}
	}

	public void UpdateDirectionTowardTarget(AnyClass target = null)
	{
		target = target ?? CurrentTarget;

		Vector3 frontVector = CoverBihaviour.front.coord - currentPos.coord;
		frontVector.Normalize();
		Vector3 dir = target.currentPos.coord - currentPos.coord;
		dir.Normalize();

		// rotation angle between the front and the current target
		float rotationAngle = Quaternion.FromToRotation(frontVector, dir).eulerAngles.y;

		if (rotationAngle == 0)
		{
			targetDirection = Direction.front;
		}
		else if (rotationAngle < 90)
		{
			targetDirection = Direction.topright;
		}
		else
		{
			targetDirection = Direction.topLeft;
		}

		//Debug.Log($"rotationAngle {rotationAngle} pos  {targetDirection}");

		//Debug.Log($"target {CurrentTarget.currentPos}");
		//Debug.Log($"front {CoverBihaviour.front}");
	}

	public void CheckForFlunks(AnyClass target = null)
	{
		target = target ?? CurrentTarget;
		UpdateDirectionTowardTarget(target);
		CoverLogic TargerCover = target.CoverBihaviour;
		//Debug.Log($" im {transform.name} and my target is at  selected target {target.name} is at the  {targetDirection}=> with cover Val = {TargerCover.CoverValue}");
		bool flunckTop = false;
		bool flunckLeft = false;
		bool flunckRight = false;

		target.stopGlowing();
		if (targetDirection == Direction.front)
		{
			if (TargerCover.front != null && TargerCover.front.tile.colliderOnTop != null)
			{
				flunckTop = false;
			}
			else
			{
				//Debug.Log($"my target {target.name} does not have cover on the front");
				flunckTop = true;
			}
			if (flunckTop == true)
			{
				Debug.Log($" im {name} =>> flucking my target {target.name} IN THE FRONT");

				target.makeMeGlow();
			}
		}
		else
		{
			if (target.targetDirection == Direction.topLeft && targetDirection == Direction.topLeft)
			{
				//Debug.Log($"both me {name} and mu target {target.name} are in the left ");
				if (TargerCover.front != null && TargerCover.front.tile.colliderOnTop != null)
				{
					flunckTop = false;
				}
				else
				{
					//Debug.Log($"my target {target.name} does not have cover on the front");
					flunckTop = true;
				}
				if (TargerCover.left != null && TargerCover.left.tile.colliderOnTop != null)
				{
					flunckLeft = false;
				}
				else
				{
					//Debug.Log($"my target {target.name} does not have cover on the left");
					flunckLeft = true;
				}
				if (flunckTop && flunckLeft)
				{
					Debug.Log($" im {name} =>> flucking my target {target.name} on the LEFT sie");
					target.makeMeGlow();
				}
			}

			if (target.targetDirection == Direction.topright && targetDirection == Direction.topright)
			{
				//Debug.Log($"both me {name} and mu target {target.name} are in the right ");
				if (TargerCover.front != null && TargerCover.front.tile.colliderOnTop != null)
				{
					flunckTop = false;
				}
				else
				{
					//Debug.Log($"my target {target.name} does not have cover on the front");
					flunckTop = true;
				}
				if (TargerCover.right != null && TargerCover.right.tile.colliderOnTop != null)
				{
					flunckRight = false;
				}
				else
				{
					//Debug.Log($"my target {target.name} does not have cover on the right");
					flunckRight = true;
				}
				if (flunckTop && flunckRight)
				{
					Debug.Log($" im {name} =>> flucking my target {target.name} on the Right sie");
					target.makeMeGlow();

				}
			}
		}
	}

	private void makeMeGlow()
	{
		if (modelMaterials == null) return;
		Debug.Log($" {name} materials length = {modelMaterials.Count}");
		foreach (Material material in modelMaterials)
		{
			if (material.GetFloat("_fluncked") == 0)
				material.SetFloat("_fluncked", 1);
		}
	}

	private void stopGlowing()
	{
		if (modelMaterials == null) return;
		foreach (Material material in modelMaterials)
		{
			if (material.GetFloat("_fluncked") == 1)
				material.SetFloat("_fluncked", 0);
		}
	}

	public void CheckForFlunksoriginal(AnyClass target = null)
	{
		target = target ?? CurrentTarget;
		UpdateDirectionTowardTarget(target);
		CoverLogic TargerCover = target.CoverBihaviour;

		//Debug.Log($" selected target {target.name} is at the  {targetDirection}=> with cover Val = {TargerCover.CoverValue}");
		if (TargerCover.front != null && TargerCover.front.tile.colliderOnTop != null)
		{
			flunckTargetTop = FlunckDirection.None;
			//Debug.Log($" the target have FRONT Cover => target Cover Val = {TargerCover.CoverValue}");
			if (targetDirection == Direction.topLeft)
			{
				checkLefFlunck(TargerCover);
			}

			if (targetDirection == Direction.topright)
			{
				checkRightFlunck(TargerCover);
			}
		}
		else
		{
			flunckTargetTop = FlunckDirection.front;
			checkRightFlunck(TargerCover);
			checkLefFlunck(TargerCover);
			//Debug.Log($" selected target an he is Flanked on the FRONT side => target Cover Val = {TargerCover.CoverValue}");
		}
		//if (CoverBihaviour.front != null && CoverBihaviour.front.tile.colliderOnTop != null)
		//{
		//	Debug.Log($" the player has a FRONT cover");
		//}
		//else
		//{
		//	Debug.Log($" the player HAS NO COVER ON THE FRONT");
		//}

		if (flunckTargetTop == FlunckDirection.front)
		{
			if (targetDirection == Direction.topLeft && flunckTargetLeft == FlunckDirection.left)
			{
				Debug.Log($"flunking enemy on the Left");
				stats.FlunckingTarget.Raise(true);
			}
			if (targetDirection == Direction.topright && flunckTargetRight == FlunckDirection.right)
			{
				Debug.Log($"flunking enemy on the Right");
				stats.FlunckingTarget.Raise(true);
			}
			if (targetDirection == Direction.front)
			{
				Debug.Log($"flunking enemy on the Front");
				stats.FlunckingTarget.Raise(true);
			}
			//else
			//{
			//	// not fluncking
			//	stats.FlunckingTarget.Raise(false);
			//}
		}
		else
		{
			// not flunking
			stats.FlunckingTarget.Raise(false);
		}

		//Debug.Log($"fluncked left {flunckTargetLeft == FlunckDirection.left} fluncked right {flunckTargetRight == FlunckDirection.right} fluncked top {flunckTargetTop == FlunckDirection.front} ");
	}

	private void checkLefFlunck(CoverLogic TargerCover)
	{
		if (TargerCover.left != null && TargerCover.left.tile.colliderOnTop != null)
		{
			//Debug.Log($" selected target have LEFT Cover and  => target Cover Val = {TargerCover.CoverValue}");
			flunckTargetLeft = FlunckDirection.None;
		}
		else
		{
			//Debug.Log($" selected target he is Flanked on the LEFT side  => target Cover Val = {TargerCover.CoverValue}");
			flunckTargetLeft = FlunckDirection.left;
		}
	}

	private void checkRightFlunck(CoverLogic TargerCover)
	{
		if (TargerCover.right != null && TargerCover.right.tile.colliderOnTop != null)
		{
			flunckTargetRight = FlunckDirection.None;
			//Debug.Log($" selected target have RIGHT Cover => target Cover Val = {TargerCover.CoverValue}");
		}
		else
		{
			flunckTargetRight = FlunckDirection.right;
			//Debug.Log($" selected target he is Flanked on the RIGHT side => target Cover Val = {TargerCover.CoverValue}");
		}
	}

	private void OnDisable()
	{
		if (CurrentTarget != null)
		{
			GameStateManager.Instance.clearPreviousSelectedUnitFromAllWeaponEvent(_currentTarger);
		}
	}

	public async void SelectNextTarget(AnyClass currentUnit)
	{
		if (team is RedTeam)
		{
			List<PlayerStateManager> players = gameStateManager.players;
			players = players.Where(unit => unit.State is Idel).ToList();
			int nbPlyaers = players.Count;
			int currentTargetIndex = players.FindIndex(instance => instance == CurrentTarget);
			if (players.Count == 0)
			{
				Debug.Log($" No More Targets All Dead  ");
				return;
			}
			CurrentTarget = players[(currentTargetIndex + 1) % nbPlyaers];
			// todo: find an alternative this cast can cause problem i nthe future
			//gameStateManager.SelectedPlayer = (Player)currentTarget;
			await rotateTowardDirection(partToRotate, CurrentTarget.aimPoint.position - aimPoint.position, 1);
			//rotateTowardDirection(CurrentTarget.partToRotate, aimPoint.position - CurrentTarget.aimPoint.position);

			onChangeTarget.Raise();
			CoverBihaviour.UpdateNorthPositionTowardTarget(CurrentTarget);
			CurrentTarget.CoverBihaviour.UpdateNorthPositionTowardTarget(this);
			CoverBihaviour.CalculateCoverValue();
			CheckForFlunks();

		}
		else if (team is GreanTeam)
		{
			List<PlayerStateManager> enemies = gameStateManager.enemies;
			enemies = enemies.Where(unit => unit.State is Idel).ToList();

			Debug.Log($"enemies count is {enemies.Count} target isi => {enemies.FirstOrDefault()}");

			if (enemies.Count == 0)
			{
				Debug.Log($" No More Targets All Dead  ");
				return;
			}
			int currentTargetIndex = enemies.FindIndex(instance => instance == CurrentTarget);
			CurrentTarget = enemies[(currentTargetIndex + 1) % enemies.Count];

			//Debug.Log($"switch to target {CurrentTarget}, enemies idel {enemies.Count}");
			// todo: find an alternative this cast can cause problem i nthe future
			//gameStateManager.SelectedEnemy = (Enemy)currentTarget;
			await rotateTowardDirection(partToRotate, CurrentTarget.aimPoint.position - aimPoint.position, 1);
			//rotateTowardDirection(CurrentTarget.partToRotate, aimPoint.position - CurrentTarget.aimPoint.position);

			onChangeTarget.Raise();
			CoverBihaviour.UpdateNorthPositionTowardTarget(CurrentTarget);
			CurrentTarget.CoverBihaviour.UpdateNorthPositionTowardTarget(this);
			CoverBihaviour.CalculateCoverValue();
			CheckForFlunks();
		}
	}


	public Node onNodeHover(Node oldPotentialDest)
	{
		//Node oldDestination = destination;
		if (NodeGrid.Instance == null) return null;
		Node res;

		if (fpsCam.enabled)
		{
			res = NodeGrid.Instance.getNodeFromMousePosition(fpsCam);
		}
		else
		{
			res = NodeGrid.Instance.getNodeFromMousePosition();
		}

		Node potentialDestination = res;

		if (potentialDestination != null && potentialDestination != destination && potentialDestination != currentPos)
		{
			List<Node> potentialPath = FindPath.AStarAlgo(currentPos, potentialDestination);
			if (potentialPath.Count == 0) return null;
			Vector3[] turns = FindPath.createWayPoint(potentialPath);

			//lineConponent.SetUpLine(turnPoints);

			path = potentialPath;
			turnPoints = turns;
			foreach (Node node in path)
			{
				if (turnPoints.Contains(node.coord))
					node.tile.obj.GetComponent<Renderer>().material.color = Color.green;
				else
				{
					node.tile.obj.GetComponent<Renderer>().material.color = Color.gray;
				}
			}
			potentialDestination.tile.obj.GetComponent<Renderer>().material.color = Color.blue;
			checkForCover(potentialDestination);

			if (Input.GetMouseButtonDown(0))
			{
				//ActionData move = actions.FirstOrDefault((el) => el is MovementAction);
				//move.Actionevent.Raise();
				CreateNewMoveAction();
			}

			if (oldPotentialDest != null && oldPotentialDest != potentialDestination)
			{
				oldPotentialDest.tile.destroyAllActiveCover();
				oldPotentialDest.tile.mouseOnTile = false;
			}
			return potentialDestination;
		}
		// if potentialDestination is null(hover over some unwalckabale) we return the
		// oldDestination
		return oldPotentialDest;
	}

	public void checkForCover(Node potentialDestination)
	{
		if (potentialDestination.tile.mouseOnTile == true) return;
		foreach (Node neighbour in potentialDestination.neighbours)
		{
			if (neighbour.isObstacle == true)
			{
				Vector3 side = (neighbour.coord - potentialDestination.coord).normalized;

				if (side == Vector3.right)
					potentialDestination.tile.createRightCover();
				else if (side == Vector3.left)
					potentialDestination.tile.createLeftCover();
				else if (side == Vector3.forward)
					potentialDestination.tile.createForwardCover();
				else if (side == Vector3.back)
					potentialDestination.tile.createBackCover();
			}
		}
		potentialDestination.tile.mouseOnTile = true;
	}

	public List<Node> CheckMovementRange()
	{
		// by default the first 4 neighbor are always in range

		if (currentPos?.neighbours == null) return new List<Node>();
		List<Node> lastLayerOfInrangeNeighbor = new List<Node>(currentPos.neighbours);
		List<Node> allAccceccibleNodes = new List<Node>();

		int firstRange = 8 / 2;
		bool depassMidDepth = true;
		int depth = 0;

		while (true)
		{
			allAccceccibleNodes.AddRange(lastLayerOfInrangeNeighbor);
			if (depth >= firstRange) depassMidDepth = false;

			lastLayerOfInrangeNeighbor = updateNeigbor(lastLayerOfInrangeNeighbor, currentPos, depassMidDepth);
			depth++;
			if (depth == 8) break;
		}

		foreach (Node item in allAccceccibleNodes)
		{
			//if (item.firstRange == true)
			//	item.tile.obj.GetComponent<Renderer>().material.color = Color.black;
			//else
			//	item.tile.obj.GetComponent<Renderer>().material.color = Color.yellow;
		}

		return allAccceccibleNodes;
	}

	public List<Node> updateNeigbor(List<Node> neighbors, Node origin, bool depassMidDepth)
	{
		List<Node> newLastLayer = new List<Node>();
		// every neighbor is updated to inrage is true
		foreach (Node node in neighbors)
		{
			node.inRange = true;
			if (depassMidDepth == false) node.firstRange = true;
		}

		// loop again to create new list of neighbor which are adjacent to the old one
		foreach (Node node in neighbors)
		{
			// for each node loop throw the neighbor which are not inRange and are not
			// in the local newLastLayer list, if they are add them the newLastLayer
			foreach (Node n in node.neighbours.Where((nei) => nei.inRange == false && nei != origin).ToList())
			{
				if (newLastLayer.Contains(n) == false)
					newLastLayer.Add(n);
			}
		}
		return newLastLayer;
	}

	public void CreateNewMoveAction()
	{
		// cant have more that 2 actions

		//int actionPoints = GetComponent<PlayerStats>().ActionPoint;
		//if (actionPoints == 0 || (processing && queueOfActions.Count >= 1))
		//{
		//	Debug.Log($" No action point Left !!!");
		//	return;
		//}
		Node res;

		if (fpsCam.enabled)
		{
			res = NodeGrid.Instance.getNodeFromMousePosition(fpsCam);
		}
		else
		{
			res = NodeGrid.Instance.getNodeFromMousePosition();
		}
		Node oldDest = destination;
		if (res != null)
		{
			destination = res;
		}

		//Debug.Log($"destination {destination} coord = {destination?.coord}");
		if (destination != null)
		{
			if (oldDest == null || destination == currentPos)
				oldDest = currentPos;
			MoveAction move = new MoveAction(MoveActionCallback, "Move", oldDest, destination);
			PlayerStateManager thisUnit = (PlayerStateManager)this;
			thisUnit.SwitchState(thisUnit.doingAction);
			Enqueue(move);
		}
	}

	public void CreateNewReloadAction()
	{
		// cant have more that 2 actions
		//int actionPoints = GetComponent<PlayerStats>().ActionPoint;
		//if (actionPoints <= 0 || (processing && queueOfActions.Count >= 1))
		//{
		//	Debug.Log($" No action point Left !!!");
		//	return;
		//}
		ReloadAction reload = new ReloadAction(ReloadActionCallBack, "Reload");
		Enqueue(reload);
	}

	public void ReloadActionCallBack(ReloadAction reload)
	{
		weapon.Reload(reload);
	}

	public void CreateNewShootAction()
	{
		// cant have more that 2 actions
		//int actionPoints = GetComponent<PlayerStats>().ActionPoint;
		//if (actionPoints <= 0 || (processing && queueOfActions.Count >= 1))
		//{
		//	Debug.Log($" No action point Left !!!");
		//	return;
		//}

		ShootAction shoot = new ShootAction(ShootActionCallBack, "Shoot");
		Enqueue(shoot);
	}

	public void ShootActionCallBack(ShootAction soot)
	{
		weapon.startShooting(soot);
	}
}
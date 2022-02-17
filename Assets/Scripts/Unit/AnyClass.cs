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

	public void Start()
	{
		inventory.unit = this;
		grid = NodeGrid.Instance;
		gameStateManager = GameStateManager.Instance;
		currentPos = grid.getNodeFromTransformPosition(transform);
		queueOfActions = new Queue<ActionBase>();
		path = new List<Node>();
		turnPoints = new Vector3[0];
		currentPos = grid.getNodeFromTransformPosition(transform);
		animator = model.GetComponent<Animator>();
		stats = GetComponent<Stats>();
		//stateManager = GetComponent<PlayerStateManager>();
		//Debug.Log($"start of any class ");
		enabled = this == gameStateManager.SelectedUnit ? true : false;
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
			GameStateManager.Instance.clearPreviousSelectedUnitFromAllWeaponEvent(_currentTarger);
			_currentTarger = value;
			rotateTowardDirection(partToRotate, _currentTarger.aimPoint.position - transform.position);

			GameStateManager.Instance.MakeOnlySelectedUnitListingToWeaponEvent(_currentTarger, stats?.onWeaponFinishShooting);
		}
	}

	private void OnDisable()
	{
		if (CurrentTarget != null)
		{
			GameStateManager.Instance.clearPreviousSelectedUnitFromAllWeaponEvent(_currentTarger);
		}
	}

	public void SelectNextTarget(AnyClass currentUnit)
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
			rotateTowardDirection(partToRotate, CurrentTarget.aimPoint.position - aimPoint.position);
			rotateTowardDirection(CurrentTarget.partToRotate, aimPoint.position - CurrentTarget.aimPoint.position);

			TargetAimPercent = 0;
			float percentVisibility = weapon.howMuchVisibleTheTArgetIs();
			TargetAimPercent = percentVisibility;

			float coverValue = howMuchCoverTheCurrentTArgetHave();
			TargetAimPercent = TargetAimPercent - coverValue;

			onChangeTarget.Raise();
		}
		else if (team is GreanTeam)
		{
			List<PlayerStateManager> enemies = gameStateManager.enemies;
			enemies = enemies.Where(unit => unit.State == unit.idelState).ToList();

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
			rotateTowardDirection(partToRotate, CurrentTarget.aimPoint.position - aimPoint.position);
			rotateTowardDirection(CurrentTarget.partToRotate, aimPoint.position - CurrentTarget.aimPoint.position);

			TargetAimPercent = 0;
			float percentVisibility = weapon.howMuchVisibleTheTArgetIs();
			TargetAimPercent = percentVisibility;

			float coverValue = howMuchCoverTheCurrentTArgetHave();
			TargetAimPercent = TargetAimPercent - coverValue;
			//Debug.Log($"{TargetAimPercent}");
			Debug.Log($"{ transform.name } raise onchange event ");
			onChangeTarget.Raise();
		}
	}

	private void OnValidate()
	{
		//Debug.Log($"validate execute");
	}

	private float howMuchCoverTheCurrentTArgetHave()
	{
		Vector3 ori = new Vector3(CurrentTarget.partToRotate.transform.position.x, 0.5f, CurrentTarget.partToRotate.transform.position.z);
		RaycastHit hit;
		Debug.DrawRay(ori, CurrentTarget.partToRotate.forward);
		if (Physics.Raycast(ori, CurrentTarget.partToRotate.forward, out hit, Vector3.forward.magnitude * 2))
		{
			//Debug.Log($" target have some obstacle =>  {hit.collider.name}");
			return 20.2f;
		}
		return 0;
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

	public void getCoversValueFromStandingNode()
	{
		//float myTotalCover = 0;
		//foreach (Cover cover in currentPos.tile.listOfActiveCover)
		//{
		//	myTotalCover += cover.Value;
		//}
		//Debug.Log($"total cover = {myTotalCover}");
	}

	//public void checkTargetCoverDirection(Node targetNode)
	//{
	//	foreach (Cover cover in targetNode.tile.listOfActiveCover)
	//	{
	//		Vector3 dir = new Vector3(cover.coverObj.transform.position.x - targetNode.coord.x, 0, cover.coverObj.transform.position.z - targetNode.coord.z).normalized;
	//		Debug.DrawRay(targetNode.coord + Vector3.up, dir);
	//	}
	//}
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

	public virtual void onHover()
	{ }
}
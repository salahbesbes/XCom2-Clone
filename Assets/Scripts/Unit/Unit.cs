using gameEventNameSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public Transform hand;
	public Inventory inventory;
	public List<ActionData> actions = new List<ActionData>();
	public Team team;
	public Stats stats;
	public Transform ActionHolder;
	public GameObject Action_prefab;
	public Transform HealthBar;
	public GameObject listners;
	public Camera fpsCam;
	public Camera secondCam;
	protected GameStateManager gameStateManager;

	public Transform aimPoint;
	private bool _isFlunk = false;

	public bool IsFluncked
	{
		get
		{
			return _isFlunk;
		}
		set { _isFlunk = value; }
	}

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




	//public ActionType[] actions;
	private float _rotateBy = 0;

	public float rotateBy
	{
		get => _rotateBy; set
		{
			_rotateBy = value;
		}
	}

	protected List<Node> path;

	public Queue<ActionBase> queueOfActions;

	protected Vector3[] turnPoints;

	[HideInInspector]
	protected NodeGrid grid;

	[SerializeField]
	public Node currentPos { get; set; }

	protected Unit _currentTarger;

	//public AnyClass _currentTarger
	//{
	//	get => _currentTarger;
	//	set
	//	{
	//		_currentTarger = value;

	//	}
	//}

	public Node destination;
	public bool processing = false;

	//public Weapon weapon;
	public Transform partToRotate;
	public Transform model;
	protected Animator animator;
	public float speed = 5f;


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

		if (this != gameStateManager.SelectedUnit)
		{
			enabled = false;
		}
	}

	public float TargetAimPercent
	{
		get
		{
			return Mathf.Clamp(100 - CurrentTarget.CoverBihaviour.CoverValue, 0, int.MaxValue);
		}
		set
		{
			value = Mathf.Clamp(value, 0, int.MaxValue);
			_targetAimValue = value;
		}
	}

	public Unit CurrentTarget
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
				// this code run in syncrones way, problem is we want write "await" in the get method of some Attribute
				testDelay();
				GameStateManager.Instance.MakeOnlySelectedUnitListingToWeaponEvent(_currentTarger, stats?.onWeaponFinishShooting);
				GameStateManager.Instance.MakeOnlySelectedUnitListingToBoolEvent(_currentTarger, stats?.FlunckingTarget);
			}
			else // if the Target is the selected unit we dont want to mess with the listener neither rotate it
			{
				_currentTarger = value;
			}
			onChangeTarget.Raise();
		}
	}

	public async Task<Unit> getTarget()
	{
		await testDelay();

		return CurrentTarget;
	}

	public async Task testDelay()
	{
		Vector3 TargetDir = CurrentTarget.aimPoint.position - aimPoint.position;
		await rotateTowardDirection(partToRotate, TargetDir, 2);

		CoverBihaviour.UpdateNorthPositionTowardTarget(CurrentTarget);
		CurrentTarget.CoverBihaviour.UpdateNorthPositionTowardTarget(this);
		//bool targetIsFluncked = CheckForFlunks(CurrentTarget);
		//if (targetIsFluncked) CurrentTarget.makeMeGlow();
		CurrentTarget.IsFluncked = CheckForFlunks(CurrentTarget);
		if (CurrentTarget.IsFluncked) _currentTarger.makeMeGlow();
	}

	public void UpdateDirectionTowardTarget(Unit target = null)
	{


		target = target ?? CurrentTarget;
		currentPos = currentPos ?? NodeGrid.Instance.getNodeFromTransformPosition(transform);

		Vector3 frontVector = CoverBihaviour.front.coord - currentPos.coord;

		frontVector.Normalize();

		target.currentPos = target.currentPos ?? NodeGrid.Instance.getNodeFromTransformPosition(target.transform);

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

	public bool CheckForFlunks(Unit target = null)
	{
		target = target ?? CurrentTarget;
		//Debug.Log($" check for flunk called ");
		UpdateDirectionTowardTarget(target);

		CoverLogic TargerCover = target.CoverBihaviour;
		bool flunckTop, flunckLeft, flunckRight;

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
				//Debug.Log($" im {name} =>> flucking my target {target.name} IN THE FRONT");

				return true;
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
					//Debug.Log($" im {name} =>> flucking my target {target.name} on the LEFT sie");
					return true;
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
					//Debug.Log($" im {name} =>> flucking my target {target.name} on the Right sie");
					return true;
				}
			}
		}
		return false;
	}



	public void makeMeGlow()
	{
		if (modelMaterials == null) return;
		foreach (Material material in modelMaterials)
		{
			if (material.GetFloat("_fluncked") == 0)
				material.SetFloat("_fluncked", 1);
		}
	}




	public void stopGlowing()
	{
		if (modelMaterials == null) return;
		foreach (Material material in modelMaterials)
		{
			if (material.GetFloat("_fluncked") == 1)
				material.SetFloat("_fluncked", 0);
		}
	}

	public void CheckForFlunksoriginal(Unit target = null)
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

	public void SelectNextTarget()
	{
		if (team is GreanTeam)
		{
			List<PlayerStateManager> enemies = gameStateManager.enemies;
			enemies = enemies.Where(unit => unit.State is Idel).ToList();

			if (enemies.Count == 0)
			{
				Debug.Log($" No More Targets All Dead  ");
				return;
			}
			int currentTargetIndex = enemies.FindIndex(instance => instance == CurrentTarget);
			CurrentTarget = enemies[(currentTargetIndex + 1) % enemies.Count];
			//PlayerStateManager target = (PlayerStateManager)CurrentTarget;
			//if (target?.State is not Idel) SelectNextTarget();
		}
		else if (team is RedTeam)
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
		}
	}

	public Node onNodeHover(Node oldPotentialDest)
	{
		//Node oldDestination = destination;
		if (NodeGrid.Instance == null) return null;
		Node potentialDestination;

		if (fpsCam.enabled)
		{
			potentialDestination = NodeGrid.Instance.getNodeFromMousePosition(fpsCam);
		}
		else
		{
			potentialDestination = NodeGrid.Instance.getNodeFromMousePosition();
		}

		// if node exist and tile.colliderOnTop exist
		if (potentialDestination?.tile?.colliderOnTop)
		{
			// and collider with tag "LowObstacle" => cant set this node to destination (its is occupied)
			// but still can walk on it 
			if (potentialDestination.tile.colliderOnTop.transform.CompareTag("LowObstacle"))
			{
				return oldPotentialDest;
			}
		}
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
				{
					node.tile.hightLight(Color.green);
				}
				else
				{
					node.tile.hightLight(Color.gray);
				}
			}
			potentialDestination.tile.hightLight(Color.blue);

			checkForCover(potentialDestination);

			if (Input.GetMouseButtonDown(0))
			{
				if (potentialDestination.firstRange)
				{
					Debug.Log("fist range");
					CreateNewMoveAction(1);
				}
				else if (potentialDestination.inRange)
				{
					Debug.Log("second range");
					CreateNewMoveAction(2);
				}
			}

			if (oldPotentialDest != null && oldPotentialDest != potentialDestination)
			{
				oldPotentialDest.tile.destroyAllActiveCover();
				oldPotentialDest.tile.mouseAlreadyOnTile = false;
			}
			return potentialDestination;
		}
		// if potentialDestination is null(hover over some unwalckabale) we return the
		// oldDestination
		return oldPotentialDest;
	}

	public void checkForCover(Node potentialDestination)
	{
		if (potentialDestination.tile.mouseAlreadyOnTile == true) return;
		foreach (Node neighbour in potentialDestination.neighbours)
		{
			if (neighbour.isUnwalkable && neighbour.isObstacle)
			{
				Vector3 side = (neighbour.coord - potentialDestination.coord).normalized;
				CoverType typeOfTheCover = CoverType.low;
				if (neighbour.tile.colliderOnTop.CompareTag("LowObstacle"))
				{
					typeOfTheCover = CoverType.low;
				}
				else if (neighbour.tile.colliderOnTop.CompareTag("HighObstacle"))
				{
					typeOfTheCover = CoverType.high;
				}
				if (side == Vector3.right)
					potentialDestination.tile.createRightCover(typeOfTheCover);
				else if (side == Vector3.left)
					potentialDestination.tile.createLeftCover(typeOfTheCover);
				else if (side == Vector3.forward)
					potentialDestination.tile.createForwardCover(typeOfTheCover);
				else if (side == Vector3.back)
					potentialDestination.tile.createBackCover(typeOfTheCover);
			}
		}
		potentialDestination.tile.mouseAlreadyOnTile = true;
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
			if (item.firstRange == true)
				item.tile.hightLight(Color.cyan);
			else
				item.tile.hightLight(Color.yellow);
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
			if (depassMidDepth == true) node.firstRange = true;
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

	public void onCameraEnabeled()
	{
		if (GameStateManager.Instance?.players == null || GameStateManager.Instance?.enemies == null)
			return;
		foreach (Unit unit in GameStateManager.Instance.players)
		{
			BillBoard billBoard = unit.HealthBar.GetComponent<BillBoard>();
			billBoard.cam = fpsCam.transform;
		}
		foreach (Unit unit in GameStateManager.Instance.enemies)
		{
			BillBoard billBoard = unit.HealthBar.GetComponent<BillBoard>();
			billBoard.cam = fpsCam.transform;
		}
	}


	public void useActionCost(ActionBase action)
	{
		stats.unit.ActionPoint -= action.cost;
	}


	public void CreateNewMoveAction(int cost)
	{
		//cant have more that 2 actions

		int actionPoints = stats.unit.ActionPoint;

		if (actionPoints < cost || (processing && queueOfActions.Count >= 1))
		{
			Debug.Log($" No action point Left !!!");
			gameStateManager.notifyCanvas.Raise("No Action Point left");
			return;
		}
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
			MoveAction move = new MoveAction(MoveActionCallback, "Move", oldDest, destination, cost);
			PlayerStateManager thisUnit = (PlayerStateManager)this;
			thisUnit.SwitchState(thisUnit.doingAction);
			Enqueue(move);
		}
	}

	public void CreateNewReloadAction()
	{
		// cant have more that 2 actions
		int actionPoints = stats.unit.ActionPoint;

		if (actionPoints <= 0 || (processing && queueOfActions.Count >= 1))
		{
			Debug.Log($" No action point Left !!!");
			return;
		}
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
		int actionPoints = stats.unit.ActionPoint;

		if (actionPoints <= 0 || (processing && queueOfActions.Count >= 1))
		{
			Debug.Log($" No action point Left !!!");
			return;
		}

		ShootAction shoot = new ShootAction(ShootActionCallBack, "Shoot");
		Enqueue(shoot);
	}

	public void ShootActionCallBack(ShootAction soot)
	{
		weapon.startShooting(soot);
	}



	public void MoveActionCallback(MoveAction actionInstance, Node start, Node end)
	{
		PlayAnimation(AnimationType.run);
		move(actionInstance, turnPoints);
	}

	//private void Start()
	//{
	//	grid = NodeGrid.Instance;
	//}

	public void PlayAnimation(AnimationType anim)
	{
		animator = model.GetComponent<Animator>();
		foreach (AnimatorControllerParameter item in animator.parameters)
		{
			if (item.type is AnimatorControllerParameterType.Bool)
			{
				animator.SetBool(item.name, false);
			}
		}
		PlayerStateManager thisPlayer = (PlayerStateManager)this;
		thisPlayer.currentActionAnimation = anim;
		string CorrespondNameOfTheAnimation = Enum.GetName(typeof(AnimationType), anim);
		animator.SetBool(CorrespondNameOfTheAnimation, true);
	}

	public void PlayIdelAnimation()
	{
		PlayAnimation(AnimationType.idel);
	}

	public async Task originalRotation(Transform partToRotate, Vector3 dir, float timeToSpentTurning = 2)
	{
		float speed = 3;
		float timeElapsed = 0, lerpDuration = timeToSpentTurning;

		if (partToRotate == null) return;
		Quaternion startRotation = partToRotate.rotation;

		//Quaternion targetRotation = player.transform.rotation * Quaternion.Euler(dir);
		Quaternion targetRotation = Quaternion.LookRotation(dir);

		while (timeElapsed < lerpDuration)
		{
			Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
				    targetRotation,
				     timeElapsed / lerpDuration
				    )
				    .eulerAngles;
			//partToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
			timeElapsed += (speed * Time.deltaTime);
			//Debug.Log($"rotating");
			await Task.Yield();
			partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
			//partToRotate.Rotate(Vector3.up, rotation.y);
		}
		rotateBy = Quaternion.Angle(startRotation, this.partToRotate.rotation);
	}

	public async Task rotateTowardDirection(Transform partToRotate, Vector3 dir, float timeToSpentTurning = 2)
	{
		float speed = 3;
		float timeElapsed = 0, lerpDuration = timeToSpentTurning;

		if (partToRotate == null) return;

		try
		{
			Quaternion startRotation = partToRotate.rotation;

			Quaternion targetRotation = Quaternion.LookRotation(dir);

			while (timeElapsed < lerpDuration)
			{
				Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
					    targetRotation,
					     timeElapsed / lerpDuration
					    )
					    .eulerAngles;
				//partToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
				timeElapsed += (speed * Time.deltaTime);
				await Task.Yield();
				partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
			}
		}
		catch (Exception ex)
		{
			Debug.Log($"exception {ex.Message}");
		}
	}

	public void turnTheModel(Vector3 dir)
	{
		// handle rotation on axe Y
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		// smooth the rotation of the turrent
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
				lookRotation,
				Time.deltaTime * 2
				)
				.eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	public async Task move(MoveAction moveInstance, Vector3[] turnPoints)
	{
		if (turnPoints.Length > 0)
		{
			for (int i = 0; i < turnPoints.Length; i++)
			{
				turnPoints[i].y = transform.position.y;
			}
			//grid.path = path;
			//grid.turnPoints = turnPoints;
			Vector3 currentPoint = turnPoints[0];
			int index = 0;
			// this while loop simulate the update methode
			while (true)
			{
				if (transform.position == currentPoint)
				{
					index++;
					if (index >= turnPoints.Length)
					{
						//PathRequestManager.Instance.finishedProcessingPath();

						break;
					}
					// im moving the model not the part to rotate
					rotateTowardDirection(model, destination.coord - partToRotate.transform.position, 0.5f);
					currentPoint = turnPoints[index];
				}

				transform.position = Vector3.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);

				// this yield return null waits until the next frame reached ( dont
				// exit the methode )
				await Task.Yield();
			}
		}

		//Debug.Log($"finish moving");
		FinishAction(moveInstance);
		//onActionFinish();
		await Task.Yield();
	}

	public void LockOnTarger()
	{
		if (currentPos == null || destination == null) return;

		if (_currentTarger == null || currentPos.coord != destination.coord)
		{// handle rotation on axe Y
			Vector3 dir = destination.coord - currentPos.coord;
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			// smooth the rotation of the turrent
			Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
					lookRotation,
					Time.deltaTime * 5f)
					.eulerAngles;
			partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
			return;
		}
		if (destination == null || (currentPos.coord == destination.coord))
		{
			Vector3 dir = _currentTarger.aimPoint.position - transform.position;
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
			partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);

			return;
		}
	}

	public async void FinishAction(ActionBase action)
	{
		//todo: reset the grid

		PlayIdelAnimation();
		PlayerStateManager player = (PlayerStateManager)this;
		// triget event

		switch (action)
		{
			case ShootAction:
				player.stats.onWeaponFinishShooting.Raise(player.stats.unit);
				break;

			case LunchGrenadeAction:
				Debug.Log($"  Grenade Lunched ..!!!!!!! explosion in 1 Sec");
				//player.stats.onWeaponFinishShooting.Raise(player.stats.unit);
				break;

			case MoveAction:
				//rotateTowardDirection(_currentTarger.partToRotate, transform.position - _currentTarger.aimPoint.position);
				Vector3 ori = new Vector3(_currentTarger.partToRotate.transform.position.x, 0.5f, _currentTarger.partToRotate.transform.position.z);

				RaycastHit hit;
				if (Physics.Raycast(ori, _currentTarger.partToRotate.forward * 2, out hit, Vector3.forward.magnitude * 2))
				{
					//Debug.Log($"target have some obstacle => {hit.collider.name}");
				}
				break;

			default:
				Debug.Log($" action  {action} NOT FOUND");
				break;


		}
		Unit thisUnit = (Unit)this;
		thisUnit.useActionCost(action);
		player.SwitchState(player.idelState);

		await rotateTowardDirection(partToRotate, _currentTarger.aimPoint.position - partToRotate.position, 1f);
		// when moving i rotating the model not the part to rotate, so when reach
		// destination i have to rotate the model last time
		await rotateTowardDirection(model, _currentTarger.aimPoint.position - partToRotate.position, 0.5f);
		player.testDelay();
		processing = false;

		// update the cost
		//GetComponent<PlayerStats>().ActionPoint -= action.cost;

		ExecuteActionInQueue();
	}


	public void Enqueue(ActionBase action)
	{
		queueOfActions.Enqueue(action);
		ExecuteActionInQueue();
	}

	public void ExecuteActionInQueue()
	{
		if (processing == false && queueOfActions.Count > 0)
		{
			processing = true;
			ActionBase action = queueOfActions.Dequeue();
			action.TryExecuteAction();
		}
	}






}
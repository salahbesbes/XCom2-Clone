using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnyClass : Unit, IBaseActions
{
	public List<ActionData> actions = new List<ActionData>();
	public Transform ActionHolder;
	public GameObject Action_prefab;
	public Transform HealthBarHolder;
	public Camera fpsCam;
	protected GameStateManager gameStateManager;
	public AnyClass currentTarget;
	public bool isFlanked;

	public void updatePlayerActionUi()
	{
		foreach (Transform child in ActionHolder)
		{
			Destroy(child.gameObject);
		}

		foreach (ActionData action in actions)
		{
			GameObject actionUi = Instantiate(Action_prefab, ActionHolder);
			actionUi.name = $"{action.name}_btn";
			Button btn = actionUi.GetComponent<Button>();
			btn.image.sprite = action.icon;
			btn.onClick.AddListener(() =>
			{
				action?.Actionevent?.Raise();
			});
		}
	}

	public void SelectNextTarget(AnyClass currentUnit)
	{
		if (currentUnit is Enemy)
		{
			List<Player> players = gameStateManager.players;
			int nbPlyaers = players.Count;
			int currentTargetIndex = players.FindIndex(instance => instance == currentTarget);
			currentTarget = players[(currentTargetIndex + 1) % nbPlyaers];
		}
		else if (currentUnit is Player)
		{
			List<Enemy> enemies = gameStateManager.enemies;
			int nbEnemies = enemies.Count;
			int currentTargetIndex = enemies.FindIndex(instance => instance == currentTarget);
			currentTarget = enemies[(currentTargetIndex + 1) % nbEnemies];
		}
	}

	public void onNodeHover()
	{
		Node oldDestination = destination;
		Node res;
		if (fpsCam.enabled)
		{
			res = grid?.getNodeFromMousePosition(fpsCam);
		}
		else
		{
			res = grid?.getNodeFromMousePosition();
		}
		Node potentialDestination = res;
		if (potentialDestination != null && potentialDestination != destination && potentialDestination != currentPos)
		{
			List<Node> potentialPath = FindPath.AStarAlgo(currentPos, potentialDestination);
			Vector3[] turns = FindPath.createWayPoint(potentialPath);

			//lineConponent.SetUpLine(turnPoints);

			path = potentialPath;
			turnPoints = turns;
			foreach (Node node in path)
			{
				if (turnPoints.Contains(node.coord))
					node.tile.GetComponent<Renderer>().material.color = Color.green;
				else
				{
					node.tile.GetComponent<Renderer>().material.color = Color.gray;
				}
			}
			potentialDestination.tile.GetComponent<Renderer>().material.color = Color.blue;

			if (Input.GetMouseButtonDown(0))
			{
				ActionData move = actions.FirstOrDefault((el) => el is MovementAction);
				move.Actionevent.Raise();
				//gameStateManager.selectedPlayer.CreateNewMoveAction();
			}
		}
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
				item.tile.GetComponent<Renderer>().material.color = Color.black;
			else
				item.tile.GetComponent<Renderer>().material.color = Color.yellow;
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
		Camera fpsCam = transform.Find("PlayerPrefab").Find("fps_cam").GetComponent<Camera>();
		if (fpsCam.enabled)
		{
			res = grid.getNodeFromMousePosition(fpsCam);
		}
		else
		{
			res = grid.getNodeFromMousePosition();
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
}
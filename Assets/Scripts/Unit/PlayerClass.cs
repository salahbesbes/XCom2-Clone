using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClass : AnyClass, IBaseActions
{
	public List<ActionData> actions = new List<ActionData>();
	public Transform ActionHolder;
	public GameObject Action_prefab;

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
}

public class AnyClass : Unit
{
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
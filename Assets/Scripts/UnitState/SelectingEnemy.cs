using System.Linq;
using UnityEngine;

public class SelectingEnemy : AnyState<PlayerStateManager>
{
	public override AnyClass EnterState(PlayerStateManager player)
	{
		Debug.Log($"current state : {player.State.name}");
		player.fpsCam.enabled = false;
		player.secondCam.gameObject.SetActive(true);
		//player.secondCam.transform.LookAt(player.currentTarget.currentPos.coord);
		return null;
	}

	public override void Update(PlayerStateManager player)
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			player.SelectNextTarget(player);
			//player.SwitchState(player.idelState);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			player.SwitchState(player.idelState);
		}

		if (Input.GetMouseButtonDown(1))
		{
			ActionData shoot = player.actions.FirstOrDefault((el) => el is ShootingAction);
			shoot?.Actionevent?.Raise();
		}
	}

	public override void ExitState(PlayerStateManager player)
	{
		player.fpsCam.enabled = true;
		player.secondCam.gameObject.SetActive(false);
		player.secondCam.transform.SetParent(player.transform);
		player.secondCam.transform.position = player.fpsCam.transform.position;
		player.secondCam.gameObject.SetActive(false);
	}
}
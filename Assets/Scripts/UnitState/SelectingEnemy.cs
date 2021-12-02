using System.Linq;
using UnityEngine;

public class SelectingEnemy : AnyState<PlayerStateManager>
{
	public override AnyClass EnterState(PlayerStateManager player)
	{
		Debug.Log($"{player.name}  state : {player.State.name}");
		player.fpsCam.enabled = false;
		//player.secondCam.transform.LookAt(player.currentTarget.transform);
		player.secondCam.gameObject.SetActive(true);
		return null;
	}

	public override void Update(PlayerStateManager player)
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			player.SelectNextTarget(player);
			//player.secondCam.transform.LookAt(player.currentTarget.transform);
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
		player.secondCam.gameObject.SetActive(false);

		player.fpsCam.enabled = true;

	}
}
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

		player.PlayAnimation(AnimationType.aim);

		return null;
	}

	public override void Update(PlayerStateManager player)
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			player.SelectNextTarget(player);
			Camera.main.transform.LookAt(player.currentTarget.aimPoint);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			player.SwitchState(player.idelState);
		}

		if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space))
		{
			ActionData shoot = player.actions.FirstOrDefault((el) => el is ShootingAction);
			player.currentActionAnimation = AnimationType.shoot;
			player.SwitchState(player.doingAction);
			shoot?.Actionevent?.Raise();
		}
	}

	public override void ExitState(PlayerStateManager player)
	{
		player.secondCam.gameObject.SetActive(false);

		player.fpsCam.enabled = true;
	}
}
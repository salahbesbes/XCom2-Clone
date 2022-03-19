using UnityEngine;

public class SelectingEnemy : AnyState<PlayerStateManager>
{
	public override AnyClass EnterState(PlayerStateManager player)
	{
		player.weapon.enabled = true;
		//Debug.Log($"{player.name}  state : {player.State.name}");
		player.fpsCam.enabled = false;
		//player.secondCam.transform.LookAt(player.currentTarget.transform);
		player.secondCam.gameObject.SetActive(true);

		player.PlayAnimation(AnimationType.aim);

		return null;
	}

	public override void Update(PlayerStateManager player)
	{
		NodeGrid.Instance.resetGrid();
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			player.SelectNextTarget();
			Camera.main.transform.LookAt(player.CurrentTarget.aimPoint);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			player.SwitchState(player.idelState);
		}

		player.customUpdate();
	}

	public override void ExitState(PlayerStateManager player)
	{
		player.secondCam.gameObject.SetActive(false);

		player.fpsCam.enabled = true;
	}
}
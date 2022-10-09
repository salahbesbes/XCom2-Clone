using UnityEngine;

public class DoingAction : BaseState<PlayerStateManager>
{
	public override void EnterState(PlayerStateManager player)
	{
		//Debug.Log($"{player.name} current state : {player.State}");
		//Debug.Log($"{player.currentActionAnimation}");
		player.PlayAnimation(player.currentActionAnimation);
	}

	public override void Update(PlayerStateManager player)
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			player.SwitchState(player.idelState);
		}
	}

	public override void ExitState(PlayerStateManager player)
	{
		player.secondCam.gameObject.SetActive(false);

		player.fpsCam.enabled = true;
	}
}
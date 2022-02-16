using UnityEngine;

public class DoingAction : AnyState<PlayerStateManager>
{
	public override AnyClass EnterState(PlayerStateManager player)
	{
		//Debug.Log($"{player.name} current state : {player.State.name}");
		//Debug.Log($"{player.currentActionAnimation}");
		player.PlayAnimation(player.currentActionAnimation);
		return null;
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
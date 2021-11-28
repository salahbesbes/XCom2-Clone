using UnityEngine;

public class DoingAction : AnyState<PlayerStateManager>
{
	public override PlayerClass EnterState(PlayerStateManager player)
	{
		player.State.name = "doingAction";
		Debug.Log($"current state : {player.State.name}");
		return null;
	}

	public override void Update(PlayerStateManager player)
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			player.SwitchState(player.idelState);
		}
	}

	public override void ExitState(PlayerStateManager player)
	{
	}
}
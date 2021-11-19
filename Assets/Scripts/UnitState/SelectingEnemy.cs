using UnityEngine;

public class SelectingEnemy : AnyState<PlayerStateManager>
{
	public override void EnterState(PlayerStateManager player)
	{
		player.State.name = "selecting Enemy";
		Debug.Log($"current state : {player.State.name}");
	}

	public override void Update(PlayerStateManager player)
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			player.SwitchState(player.doingAction);
		}
	}

	public override void ExitState(PlayerStateManager player)
	{
	}
}
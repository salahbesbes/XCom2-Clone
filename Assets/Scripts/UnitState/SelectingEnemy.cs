using UnityEngine;

public class SelectingEnemy : AnyState<PlayerStateManager>
{
	public override PlayerClass EnterState(PlayerStateManager player)
	{
		player.State.name = "selecting Enemy";
		Debug.Log($"current state : {player.State.name}");
		return null;
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
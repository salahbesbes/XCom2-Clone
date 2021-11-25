using UnityEngine;

public class Idel : AnyState<PlayerStateManager>
{
	public override void EnterState(PlayerStateManager player)
	{
		player.State.name = "Idel";
		Debug.Log($"current state : {player.State.name}");
	}

	public override void Update(PlayerStateManager player)
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			player.SwitchState(player.selectingEnemy);
		}
	}

	public override void ExitState(PlayerStateManager player)
	{
	}
}
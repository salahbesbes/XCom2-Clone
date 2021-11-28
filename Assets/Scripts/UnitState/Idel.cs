using UnityEngine;

public class Idel : AnyState<PlayerStateManager>
{
	public override PlayerClass EnterState(PlayerStateManager player)
	{
		player.State.name = "Idel";
		Debug.Log($"current state : {player.State.name}");
		return null;
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
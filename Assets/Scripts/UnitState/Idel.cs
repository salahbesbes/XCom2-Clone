using UnityEngine;

public class Idel : AnyState<PlayerStateManager>
{
	public override AnyClass EnterState(PlayerStateManager player)
	{
		Debug.Log($" {player.name}  state : {player.State.name}");

		return null;
	}

	public override void Update(PlayerStateManager player)
	{
		player.grid.resetGrid();

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			player.SwitchState(player.selectingEnemy);
		}

		if (Input.GetKeyDown(KeyCode.K))
		{
			handleHealthUnitBar healthBar = player.HealthBarHolder.GetComponent<handleHealthUnitBar>();
			healthBar.onHeal(6);
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			handleHealthUnitBar healthBar = player.HealthBarHolder.GetComponent<handleHealthUnitBar>();
			healthBar.onDamage(4);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			player.CreateNewReloadAction();
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			player.SelectNextTarget(player);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			player.fpsCam.enabled = false;
		}

		player.CheckMovementRange();
		player.onNodeHover();


	}

	public override void ExitState(PlayerStateManager player)
	{
	}
}
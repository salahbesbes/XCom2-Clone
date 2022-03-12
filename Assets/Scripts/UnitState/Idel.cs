using System.Threading.Tasks;
using UnityEngine;

public class Idel : AnyState<PlayerStateManager>
{
	private Node oldDestination, potentialDest;

	public override AnyClass EnterState(PlayerStateManager player)
	{
		//Debug.Log($" {player.name}  state : {player.State.name}");
		player.weapon.enabled = false;
		return null;
	}

	public override void Update(PlayerStateManager player)
	{
		NodeGrid.Instance.resetGrid();

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			player.SwitchState(player.selectingEnemy);
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
		if (Input.GetKeyDown(KeyCode.K))
		{
			player.openInventory.Raise();
			player.inventory.Add(player.newWeapon);
			player.onUpdateInventoryEvent.Raise(player.inventory);
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			Debug.Log($"{player.openInventory}");
			player.openInventory.Raise();
		}
		player.CheckMovementRange();

		oldDestination = potentialDest;
		potentialDest = player.onNodeHover(oldDestination);
		//player.CoverBihaviour.UpdateNorthPositionTowardTarget(player.CurrentTarget);
		//player.CurrentTarget.CoverBihaviour.UpdateNorthPositionTowardTarget(player);
		//player.CoverBihaviour.CalculateCoverValue();
		//player.CheckForFlunks();
	}

	public async void rotateTowardDirection(PlayerStateManager player, Vector3 dir)
	{
		float speed = 3;
		float timeElapsed = 0;
		Quaternion startRotation = player.transform.rotation;

		//Quaternion targetRotation = player.transform.rotation * Quaternion.Euler(dir);
		Quaternion targetRotation = Quaternion.LookRotation(dir);

		while (player.transform.rotation != targetRotation)
		{
			player.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed);
			timeElapsed += (speed * Time.deltaTime);
			await Task.Yield();
		}
		player.transform.rotation = targetRotation;
	}

	public override void ExitState(PlayerStateManager player)
	{
		player.weapon.enabled = true;
	}
}
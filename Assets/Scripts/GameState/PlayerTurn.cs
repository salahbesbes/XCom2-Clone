using System.Linq;
using UnityEngine;

public class PlayerTurn : BaseState<GameStateManager>
{
	public override Unit EnterState(GameStateManager gameManager)
	{
		gameManager.SelectedUnit = gameManager.players.FirstOrDefault(unit => unit.State is Idel);
		gameManager.SelectedUnit.CurrentTarget = gameManager.enemies.FirstOrDefault();

		gameManager.SelectedUnit.enabled = true;
		gameManager.SelectedUnit.fpsCam.enabled = true;
		gameManager.SelectedUnit.onCameraEnabeled();
		//gameManager.PlayerChangeEvent.Raise();
		gameManager.SelectedUnit.onChangeTarget.Raise();

		return gameManager.SelectedUnit;
	}

	public override void Update(GameStateManager gameManager)
	{
		gameManager.SelectedUnit.currentPos = NodeGrid.Instance.getNodeFromTransformPosition(gameManager.SelectedUnit.transform);
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SelectNextPlayer(gameManager);
		}

		//gameManager.SelectedPlayer.checkFlank(gameManager?.SelectedEnemy?.currentPos);
	}

	public override void ExitState(GameStateManager gameManager)
	{
		if (gameManager.SelectedUnit != null)
		{
			gameManager.SelectedUnit.enabled = false;
			gameManager.SelectedUnit.fpsCam.enabled = false;
			//Debug.Log($"exit State {nameof(PlayerTurn)}");
			gameManager.clearPreviousSelectedUnitFromAllWeaponEvent(gameManager.SelectedUnit?.CurrentTarget);
		}

		foreach (PlayerStateManager player in gameManager.players)
		{
			player.stats.unit.resetActionPoints();
		}

	}

	public void SelectNextPlayer(GameStateManager gameManager)
	{
		int nbPlayers = gameManager.players.Count;

		if (gameManager != null)
		{
			gameManager.SelectedUnit.enabled = false;
			gameManager.SelectedUnit.SwitchState(gameManager.SelectedUnit.idelState);
			gameManager.SelectedUnit.fpsCam.enabled = false;

			//List<PlayerStateManager> availablePlayers = gameManager.players.Where(unit => unit.State is Idel).ToList();
			int currentPlayerIndex = gameManager.players.FindIndex(instance => instance == gameManager.SelectedUnit);
			gameManager.SelectedUnit = gameManager.players[(currentPlayerIndex + 1) % nbPlayers];

			gameManager.SelectedUnit.enabled = true;
			gameManager.SelectedUnit.fpsCam.enabled = true;
			gameManager.SelectedUnit.onCameraEnabeled();
			gameManager.SelectedUnit.CurrentTarget = gameManager.enemies.FirstOrDefault(unit => unit.State is Idel);
			//Vector3 TargetDir = gameManager.SelectedUnit.CurrentTarget.aimPoint.position - gameManager.SelectedUnit.aimPoint.position;
			//await gameManager.SelectedUnit.rotateTowardDirection(gameManager.SelectedUnit.partToRotate, TargetDir, 3);
			//await gameManager.SelectedUnit.rotateTowardDirection(gameManager.SelectedUnit.CurrentTarget.partToRotate, -TargetDir, 3);
			//gameManager.SelectedUnit.newFlunking(gameManager.SelectedUnit.CurrentTarget);
			//gameManager.MakeGAmeMAnagerListingToNewSelectedUnit(gameManager.SelectedPlayer);

			//gameManager.PlayerChangeEvent.Raise();
			//gameManager.SelectedUnit.CoverBihaviour.UpdateNorthPositionTowardTarget(gameManager.SelectedUnit.CurrentTarget);
			//gameManager.SelectedUnit.CoverBihaviour.CalculateCoverValue();
			//gameManager.SelectedUnit.CheckForFlunks();
		}
	}
}

public class EnemyTurn : BaseState<GameStateManager>
{
	public override Unit EnterState(GameStateManager gameManager)
	{
		gameManager.SelectedUnit = gameManager.enemies.FirstOrDefault(unit => unit.State is Idel);
		gameManager.SelectedUnit.CurrentTarget = gameManager.players.FirstOrDefault();
		//gameManager.SelectedEnemy.currentPos = gameManager.SelectedEnemy.grid.getNodeFromTransformPosition(gameManager.SelectedEnemy.transform);

		gameManager.SelectedUnit.enabled = true;
		gameManager.SelectedUnit.fpsCam.enabled = true;
		gameManager.SelectedUnit.onCameraEnabeled();

		//gameManager.PlayerChangeEvent.Raise();
		gameManager.SelectedUnit.onChangeTarget.Raise();
		return gameManager.SelectedUnit;
	}

	public override void Update(GameStateManager gameManager)
	{
		gameManager.SelectedUnit.currentPos = NodeGrid.Instance.getNodeFromTransformPosition(gameManager.SelectedUnit.transform);
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SelectNextEnemy(gameManager);
		}

		//gameManager.SelectedPlayer.checkFlank(gameManager?.SelectedEnemy?.currentPos);
	}

	public override void ExitState(GameStateManager gameManager)
	{
		if (gameManager.SelectedUnit != null)
		{
			gameManager.SelectedUnit.enabled = false;
			gameManager.SelectedUnit.fpsCam.enabled = false;
			gameManager.clearPreviousSelectedUnitFromAllWeaponEvent(gameManager.SelectedUnit?.CurrentTarget);
		}
		foreach (PlayerStateManager enemy in gameManager.enemies)
		{
			enemy.stats.unit.resetActionPoints();
		}
	}

	public void SelectNextEnemy(GameStateManager gameManager)
	{
		int nbEnemies = gameManager.enemies.Count;

		if (gameManager != null)
		{
			int nbPlayers = gameManager.enemies.Count;

			gameManager.SelectedUnit.enabled = false;
			gameManager.SelectedUnit.SwitchState(gameManager.SelectedUnit.idelState);
			gameManager.SelectedUnit.fpsCam.enabled = false;
			int currentPlayerIndex = gameManager.enemies.FindIndex(instance => instance == gameManager.SelectedUnit);
			gameManager.SelectedUnit = gameManager.enemies[(currentPlayerIndex + 1) % nbPlayers];

			gameManager.SelectedUnit.enabled = true;
			gameManager.SelectedUnit.fpsCam.enabled = true;
			gameManager.SelectedUnit.onCameraEnabeled();
			gameManager.SelectedUnit.CurrentTarget = gameManager.players.FirstOrDefault(unit => unit.State is Idel);
		}
	}
}
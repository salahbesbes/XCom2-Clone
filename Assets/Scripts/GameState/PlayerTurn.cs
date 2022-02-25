using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurn : AnyState<GameStateManager>
{
	private Color InitColor;

	public override AnyClass EnterState(GameStateManager gameManager)
	{
		gameManager.SelectedUnit = gameManager.players.FirstOrDefault();
		gameManager.SelectedUnit.CurrentTarget = gameManager.enemies.FirstOrDefault(unit => unit.State is Idel);

		gameManager.SelectedUnit.enabled = true;
		gameManager.SelectedUnit.fpsCam.enabled = true;
		gameManager.PlayerChangeEvent.Raise();
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
	}

	public void SelectNextPlayer(GameStateManager gameManager)
	{
		int nbPlayers = gameManager.players.Count;

		if (gameManager != null)
		{
			gameManager.SelectedUnit.enabled = false;
			gameManager.SelectedUnit.SwitchState(gameManager.SelectedUnit.idelState);
			gameManager.SelectedUnit.fpsCam.enabled = false;

			List<PlayerStateManager> availablePlayers = gameManager.players.Where(unit => unit.State is Idel).ToList();
			int currentPlayerIndex = gameManager.players.FindIndex(instance => instance == gameManager.SelectedUnit);
			gameManager.SelectedUnit = gameManager.players[(currentPlayerIndex + 1) % nbPlayers];
			//Debug.Log($"{availablePlayers.Count}");
			//gameManager.SelectedUnit = availablePlayers[(currentPlayerIndex + 1) % nbPlayers];
			// search for the next unit which is not dead if found
			//for (int i = currentPlayerIndex + 1; i < gameManager.players.Count; i++)
			//{
			//	if (gameManager.players[i % nbPlayers].State is Dead)
			//		continue;
			//	else
			//	{
			//		gameManager.SelectedUnit = gameManager.players[i % nbPlayers];
			//		break;
			//	}
			//}

			gameManager.SelectedUnit.enabled = true;
			gameManager.SelectedUnit.fpsCam.enabled = true;
			gameManager.SelectedUnit.CurrentTarget = gameManager.enemies.FirstOrDefault(unit => unit.State is Idel);

			//gameManager.MakeGAmeMAnagerListingToNewSelectedUnit(gameManager.SelectedPlayer);

			gameManager.PlayerChangeEvent.Raise();
			//gameManager.SelectedUnit.CoverBihaviour.UpdateNorthPositionTowardTarget(gameManager.SelectedUnit.CurrentTarget);
			//gameManager.SelectedUnit.CoverBihaviour.CalculateCoverValue();
			//gameManager.SelectedUnit.CheckForFlunks();
		}
	}
}

public class EnemyTurn : AnyState<GameStateManager>
{
	public override AnyClass EnterState(GameStateManager gameManager)
	{
		gameManager.SelectedUnit = gameManager.enemies.FirstOrDefault();
		gameManager.SelectedUnit.CurrentTarget = gameManager.players.FirstOrDefault();
		//gameManager.SelectedEnemy.currentPos = gameManager.SelectedEnemy.grid.getNodeFromTransformPosition(gameManager.SelectedEnemy.transform);

		gameManager.SelectedUnit.enabled = true;
		gameManager.SelectedUnit.fpsCam.enabled = true;

		gameManager.PlayerChangeEvent.Raise();
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
			gameManager.SelectedUnit = gameManager.enemies[currentPlayerIndex % nbPlayers];
			// search for the next unit which is not dead if found
			//for (int i = currentPlayerIndex + 1; i < gameManager.enemies.Count; i++)
			//{
			//	if (!(gameManager.enemies[i % nbPlayers].State is Dead))
			//	{
			//		gameManager.SelectedUnit = gameManager.enemies[i % nbPlayers];
			//		break;
			//	}
			//}
			gameManager.SelectedUnit.enabled = true;
			gameManager.SelectedUnit.fpsCam.enabled = true;
			gameManager.SelectedUnit.CurrentTarget = gameManager.players.FirstOrDefault(); ;

			gameManager.PlayerChangeEvent.Raise();
		}
	}
}
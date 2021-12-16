using System.Linq;
using UnityEngine;

public class PlayerTurn : AnyState<GameStateManager>
{
	private Color InitColor;

	public override AnyClass EnterState(GameStateManager gameManager)
	{
		gameManager.SelectedPlayer = gameManager.players.FirstOrDefault();
		gameManager.SelectedEnemy = gameManager.enemies.FirstOrDefault();
		gameManager.SelectedPlayer.currentTarget = gameManager.SelectedEnemy;
		gameManager.SelectedPlayer.enabled = true;
		gameManager.SelectedPlayer.fpsCam.enabled = true;

		return gameManager.SelectedPlayer;
	}

	public override void Update(GameStateManager gameManager)
	{
		gameManager.SelectedPlayer.currentPos = gameManager.grid.getNodeFromTransformPosition(gameManager.SelectedPlayer.transform);
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SelectNextPlayer(gameManager);
		}

		//gameManager.SelectedPlayer.checkFlank(gameManager?.SelectedEnemy?.currentPos);
	}

	public override void ExitState(GameStateManager gameManager)
	{
		if (gameManager.SelectedPlayer != null)
		{
			gameManager.SelectedPlayer.enabled = false;
			gameManager.SelectedPlayer.fpsCam.enabled = false;
			Debug.Log($"exit State {nameof(PlayerTurn)}");
		}
	}

	public void SelectNextPlayer(GameStateManager gameManager)
	{
		int nbPlayers = gameManager.players.Count;

		if (gameManager != null)
		{
			gameManager.SelectedPlayer.enabled = false;
			gameManager.SelectedPlayer.SwitchState(gameManager.SelectedPlayer.idelState);
			gameManager.SelectedPlayer.fpsCam.enabled = false;
			int currentPlayerIndex = gameManager.players.FindIndex(instance => instance == gameManager.SelectedPlayer);
			gameManager.SelectedPlayer = gameManager.players[(currentPlayerIndex + 1) % nbPlayers];

			gameManager.SelectedPlayer.enabled = true;
			gameManager.SelectedPlayer.fpsCam.enabled = true;

			//gameManager.MakeGAmeMAnagerListingToNewSelectedUnit(gameManager.SelectedPlayer);

			gameManager.PlayerChangeEvent.Raise();
			Debug.Log($"Selected  {gameManager.SelectedPlayer} ");
		}
	}
}

public class EnemyTurn : AnyState<GameStateManager>
{
	public override AnyClass EnterState(GameStateManager gameManager)
	{
		gameManager.SelectedEnemy = gameManager.enemies.FirstOrDefault();
		gameManager.SelectedPlayer = gameManager.players.FirstOrDefault();
		gameManager.SelectedEnemy.currentTarget = gameManager.SelectedPlayer;
		//gameManager.SelectedEnemy.currentPos = gameManager.SelectedEnemy.grid.getNodeFromTransformPosition(gameManager.SelectedEnemy.transform);

		gameManager.SelectedEnemy.enabled = true;
		gameManager.SelectedEnemy.fpsCam.enabled = true;

		return gameManager.SelectedEnemy;
	}

	public override void Update(GameStateManager gameManager)
	{
		gameManager.SelectedEnemy.currentPos = gameManager.grid.getNodeFromTransformPosition(gameManager.SelectedEnemy.transform);
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SelectNextEnemy(gameManager);
		}

		//gameManager.SelectedPlayer.checkFlank(gameManager?.SelectedEnemy?.currentPos);
	}

	public override void ExitState(GameStateManager gameManager)
	{
		if (gameManager.SelectedEnemy != null)
		{
			gameManager.SelectedEnemy.enabled = false;
			gameManager.SelectedEnemy.fpsCam.enabled = false;
			Debug.Log($"exit State {nameof(EnemyTurn)}");
		}
	}

	public void SelectNextEnemy(GameStateManager gameManager)
	{
		int nbEnemies = gameManager.enemies.Count;

		if (gameManager != null)
		{
			int nbPlayers = gameManager.enemies.Count;

			gameManager.SelectedEnemy.enabled = false;
			gameManager.SelectedEnemy.SwitchState(gameManager.SelectedEnemy.idelState);
			gameManager.SelectedEnemy.fpsCam.enabled = false;
			int currentPlayerIndex = gameManager.enemies.FindIndex(instance => instance == gameManager.SelectedEnemy);
			gameManager.SelectedEnemy = gameManager.enemies[(currentPlayerIndex + 1) % nbPlayers];

			gameManager.SelectedEnemy.enabled = true;
			gameManager.SelectedEnemy.fpsCam.enabled = true;


			gameManager.PlayerChangeEvent.Raise();
		}
	}
}
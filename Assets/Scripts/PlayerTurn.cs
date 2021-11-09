using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurn : AnyState
{
	private Color InitColor;

	public string StateName = "Default";
	public List<Player> players;

	public override void EnterState(GameStateManager gameManager)
	{
		StateName = nameof(PlayerTurn);
		players = gameManager.players.Select(el => el.GetComponent<Player>()).ToList();
		gameManager.selectedPlayer = players.FirstOrDefault();
		gameManager.selectedEnemy = gameManager.enemies.Select(el => el.GetComponent<Enemy>()).ToList().First();
		gameManager.selectedPlayer.enabled = true;
	}

	public override void Update(GameStateManager gameManager)
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SelectNextPlayer(gameManager);
		}
	}

	public override void ExitState(GameStateManager gameManager)
	{
		if (gameManager.selectedPlayer != null)
		{
			gameManager.selectedPlayer.enabled = false;
			Debug.Log($"exit State {nameof(PlayerTurn)}");
		}
	}

	public void SelectNextPlayer(GameStateManager gameManager)
	{
		int nbPlayers = players.Count;

		if (gameManager != null)
		{
			gameManager.selectedPlayer.enabled = false;
			int currentPlayerIndex = players.FindIndex(instance => instance == gameManager.selectedPlayer);

			gameManager.selectedPlayer = players[(currentPlayerIndex + 1) % nbPlayers];
			gameManager.selectedPlayer.enabled = true;

			Debug.Log($"Selected  {gameManager.selectedPlayer} ");
		}
	}
}

public class EnemyTurn : AnyState
{
	public string StateName = "Default";
	public List<Enemy> enemies;

	public override void EnterState(GameStateManager gameManager)
	{
		StateName = nameof(EnemyTurn);
		enemies = gameManager.enemies.Select(el => el.GetComponent<Enemy>()).ToList();
		gameManager.selectedPlayer = gameManager.players.Select(el => el.GetComponent<Player>()).ToList().First();
		gameManager.selectedEnemy = enemies.First();
		gameManager.selectedEnemy.enabled = true;
	}

	public override void Update(GameStateManager gameManager)
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SelectNextEnemy(gameManager);
		}
		base.ExecuteInAnyState();
	}

	public override void ExitState(GameStateManager gameManager)
	{
		if (gameManager.selectedEnemy != null)
		{
			gameManager.selectedEnemy.enabled = false;
			Debug.Log($"exit State {nameof(EnemyTurn)}");
		}
	}

	public void SelectNextEnemy(GameStateManager gameManager)
	{
		int nbEnemies = enemies.Count;

		if (gameManager != null)
		{
			gameManager.selectedEnemy.enabled = false;
			int currentEnemyIndex = enemies.FindIndex(instance => instance == gameManager.selectedEnemy);

			gameManager.selectedEnemy = enemies[(currentEnemyIndex + 1) % nbEnemies];
			gameManager.selectedEnemy.enabled = true;

			Debug.Log($"Selected  {gameManager.selectedEnemy} ");
		}
	}
}
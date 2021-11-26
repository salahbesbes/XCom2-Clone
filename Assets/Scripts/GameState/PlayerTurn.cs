using System.Linq;
using UnityEngine;

public class PlayerTurn : AnyState<GameStateManager>
{
	private Color InitColor;

	public string StateName = "Default";

	public override void EnterState(GameStateManager gameManager)
	{
		StateName = nameof(PlayerTurn);
		gameManager.selectedPlayer = gameManager.players.FirstOrDefault();
		gameManager.selectedEnemy = gameManager.enemies.FirstOrDefault();

		gameManager.selectedPlayer.enabled = true;
		//Debug.Log($" PlayerTurn Start ,  {gameManager.selectedPlayer.transform.name} is selected");
	}

	public override void Update(GameStateManager gameManager)
	{
		//currentPos = grid.getNodeFromTransformPosition(transform);

		gameManager.selectedPlayer.currentPos = gameManager.grid.getNodeFromTransformPosition(gameManager.selectedPlayer.transform);
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			SelectNextPlayer(gameManager);
		}

		gameManager.selectedPlayer.onNodeHover();

		if (Input.GetKeyDown(KeyCode.K))
		{
			handleHealthUnitBar healthBar = gameManager.selectedPlayer.HealthBarHolder.GetComponent<handleHealthUnitBar>();
			healthBar.onHeal(6);
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			handleHealthUnitBar healthBar = gameManager.selectedPlayer.HealthBarHolder.GetComponent<handleHealthUnitBar>();
			healthBar.onDamage(4);
		}
		if (Input.GetMouseButtonDown(1))
		{
			gameManager.selectedPlayer.CreateNewShootAction();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			gameManager.selectedPlayer.CreateNewReloadAction();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			gameManager.selectedPlayer.SelectNextEnemy();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			gameManager.selectedPlayer.transform.Find("PlayerPrefab").Find("fps_cam").GetComponent<Camera>().enabled = false;
		}
		gameManager.CheckMovementRange(gameManager.selectedPlayer);
		base.ExecuteInAnyGameState(gameManager.selectedPlayer);
		gameManager.selectedPlayer.LockOnTarger();
		gameManager.selectedPlayer.checkFlank(gameManager?.selectedEnemy?.currentPos);
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
		int nbPlayers = gameManager.players.Count;

		if (gameManager != null)
		{
			gameManager.selectedPlayer.enabled = false;
			gameManager.selectedPlayer.transform.Find("PlayerPrefab").Find("fps_cam").GetComponent<Camera>().enabled = false;

			int currentPlayerIndex = gameManager.players.FindIndex(instance => instance ==
			gameManager.selectedPlayer);

			gameManager.selectedPlayer = gameManager.players[(currentPlayerIndex + 1) %
			nbPlayers]; gameManager.selectedPlayer.enabled = true;
			gameManager.selectedPlayer.transform.Find("PlayerPrefab").Find("fps_cam").GetComponent<Camera>().enabled
			= true;

			Debug.Log($"Selected  {gameManager.selectedPlayer} ");
		}
	}
}

public class EnemyTurn : AnyState<GameStateManager>
{
	public string StateName = "Default";

	public override void EnterState(GameStateManager gameManager)
	{
		StateName = nameof(EnemyTurn);

		gameManager.selectedPlayer = gameManager.players.FirstOrDefault();
		gameManager.selectedEnemy = gameManager.enemies.FirstOrDefault();

		gameManager.selectedEnemy.enabled = true;
		//Debug.Log($" EnemyTurn Start ,  {gameManager.selectedEnemy.transform.name} is selected");
	}

	public override void Update(GameStateManager gameManager)
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			Debug.Log($"tab clicked");
			SelectNextEnemy(gameManager);
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Debug.Log($"shoft clicked");

			gameManager.selectedEnemy.SelectNextPlayer();
		}

		if (Input.GetMouseButtonDown(0))
		{
			gameManager.selectedEnemy.CreateNewMoveAction();
		}
		if (Input.GetMouseButtonDown(1))
		{
			gameManager.selectedEnemy.CreateNewShootAction();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			gameManager.selectedEnemy.CreateNewReloadAction();
		}

		gameManager.CheckMovementRange(gameManager.selectedEnemy);
		base.ExecuteInAnyGameState(gameManager.selectedEnemy);

		gameManager.selectedEnemy.LockOnTarger();
		//gameManager.selectedEnemy.checkFlank(gameManager.selectedPlayer.currentPos);
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
		int nbEnemies = gameManager.enemies.Count;

		if (gameManager != null)
		{
			gameManager.selectedEnemy.enabled = false;
			int currentEnemyIndex = gameManager.enemies.FindIndex(instance => instance == gameManager.selectedEnemy);

			gameManager.selectedEnemy = gameManager.enemies[(currentEnemyIndex + 1) % nbEnemies];
			gameManager.selectedEnemy.enabled = true;

			Debug.Log($" {gameManager.selectedEnemy.transform.name} is Selected  {gameManager.selectedEnemy} ");
		}
	}
}
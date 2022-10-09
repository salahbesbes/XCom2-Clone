using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
	public void ShowEndGamePanel()
	{
		gameObject.SetActive(true);
	}

	public void ReloadSameLevel()
	{
		PlayerStateManager[] allUnits = GameObject.FindObjectsOfType<PlayerStateManager>();
		foreach (PlayerStateManager unit in allUnits)
		{
			unit.stats.unit.Reset();
		}
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}
	public void ExitTheGame()
	{
		// save any game data here
#if UNITY_EDITOR
		// Application.Quit() does not work in the editor so
		// UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
		UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
	}
}

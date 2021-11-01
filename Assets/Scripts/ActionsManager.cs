using UnityEngine;

public class ActionsManager : MonoBehaviour
{
	public static ActionsManager Instance;
	public Transform playerPrefab;
	private NodeGrid grid;
	private Player player;

	/// <summary> this Action manager need to fill the Player Action on Awake not On Start </summary>

	private void Awake()
	{
		if (Instance == null)
		{
			player = playerPrefab.GetComponent<Player>();
			grid = FindObjectOfType<NodeGrid>();
			Instance = this;
		}
	}

	private void Update()
	{
	}

	/// <summary>
	/// move the playerPrefab toward the destination var sent from the grid to Gridpath var.
	/// this methode start on mouse douwn frame and the player start moving on the next frame
	/// until it reaches the goal. thats why we are using the carroutine. to simulate the update
	/// methode we use a while loop the problem is that the while loop is too rapid ( high
	/// frequency iteration) to iterate with the same frequence of the update methode we use
	/// yield return null or some other tools the wait for certain time "WaitForSeconds"
	/// </summary>
	/// <param name="playerPrefab"> Transform playerPrefab </param>
	/// <param name="path"> Array of position to </param>
}
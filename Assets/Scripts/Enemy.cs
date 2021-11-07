using UnityEngine;

public class Enemy : MonoBehaviour
{
	private NodeGrid grid;
	public Node NodeCoord;
	public bool isFlanked = false;

	private void Start()
	{
		grid = FindObjectOfType<NodeGrid>();
	}

	private void Update()
	{
		NodeCoord = grid.getNodeFromTransformPosition(transform);
	}
}
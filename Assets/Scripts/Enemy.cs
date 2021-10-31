using UnityEngine;

public class Enemy : MonoBehaviour
{
	private NodeGrid grid;
	public Node position;
	public bool isFlanked = false;

	private void Start()
	{
		grid = FindObjectOfType<NodeGrid>();
	}

	private void Update()
	{
		position = grid.getNodeFromTransformPosition(transform);
	}
}
using UnityEngine;

public class Enemy : Charachter
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

	public override string ToString()
	{
		return $"Enemy  at position {NodeCoord.coord}";
	}
}

public class Charachter : MonoBehaviour
{
}
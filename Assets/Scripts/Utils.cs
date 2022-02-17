using UnityEngine;

public class Utils : MonoBehaviour
{
	private Transform frontCoverTransform;
	private Transform backCoverTransform;
	private Transform leftCoverTransform;
	private Transform rightCoverTransform;

	public Node frontCover;
	public Node backCover;
	public Node leftCover;
	public Node rightCover;

	private void Start()
	{
		frontCoverTransform = transform.GetChild(0);
		backCoverTransform = transform.GetChild(1);
		leftCoverTransform = transform.GetChild(2);
		rightCoverTransform = transform.GetChild(3);
		updateCoversNode();
	}

	public void updateCoversNode()
	{
		frontCover = NodeGrid.Instance.getNodeFromTransformPosition(frontCoverTransform);
		frontCover.tile.obj.GetComponent<Renderer>().material.color = Color.yellow;

		backCover = NodeGrid.Instance.getNodeFromTransformPosition(backCoverTransform);
		backCover.tile.obj.GetComponent<Renderer>().material.color = Color.white;

		leftCover = NodeGrid.Instance.getNodeFromTransformPosition(leftCoverTransform);
		leftCover.tile.obj.GetComponent<Renderer>().material.color = Color.grey;

		rightCover = NodeGrid.Instance.getNodeFromTransformPosition(rightCoverTransform);
		rightCover.tile.obj.GetComponent<Renderer>().material.color = Color.green;

		//Debug.Log($"front {frontCover} back {backCover} left {leftCover} right {rightCover}");
	}

	internal void rotate90(float val)
	{
		Debug.Log($"rotate90");
		transform.rotation = Quaternion.Euler(0, val, 0);
	}
}
using UnityEngine;

public class LineController : MonoBehaviour
{
	private LineRenderer lr;
	private Vector3[] pointsToDrowLineWith;

	private void Start()
	{
		lr = GetComponent<LineRenderer>();
		pointsToDrowLineWith = new Vector3[0];
	}

	private void Update()
	{
		for (int i = 0; i < pointsToDrowLineWith.Length; i++)
		{
			lr.SetPosition(i, pointsToDrowLineWith[i]);
		}
	}

	public void SetUpLine(Vector3[] points)
	{
		lr.positionCount = points.Length;
		pointsToDrowLineWith = points;
	}
}
using UnityEngine;

public class unit : MonoBehaviour
{
	private Transform currentTarget;
	private int currentWayPointIndex = 0;
	private Vector3 currentPoint = Vector3.zero;
	private Vector3[] WayPoints = new Vector3[3] { new Vector3(-5, 0, -4), new Vector3(-3, 0, -3), new Vector3(-5, 0, -2) };
	private int index = 0;

	private void Start()
	{
		currentPoint = WayPoints[0];
	}

	// after we move the gameObject in the fixedupdate methode the camera detect new position in
	// the update methode
	private void Update()
	{
		if (transform.position == currentPoint)
		{
			index++;
			currentPoint = WayPoints[index];
			Debug.Log($"{currentPoint}");
		}

		transform.position = Vector3.MoveTowards(transform.position, currentPoint, 1f * Time.deltaTime);
	}
}
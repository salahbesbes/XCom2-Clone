using UnityEngine;

public class RenderTrajector : MonoBehaviour
{
	private Rigidbody rb;
	private LineRenderer lr;
	public float velocity;
	public float angle;
	public int resolution;
	private float g;
	private float radianAngle;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		lr = GetComponent<LineRenderer>();
		g = Mathf.Abs(Physics.gravity.y);
		//render();
	}

	private void render()
	{
		lr.positionCount = resolution + 1;
		lr.SetPositions(CalculateArcArray());
	}

	private Vector3[] CalculateArcArray()
	{
		Vector3[] arcArray = new Vector3[resolution + 1];

		radianAngle = Mathf.Deg2Rad * angle;
		float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;
		for (int i = 0; i <= resolution; i++)
		{
			float t = (float)i / (float)resolution;
			arcArray[i] = CalculateArcPoint(t, maxDistance);
		}
		return arcArray;
	}

	private Vector3 CalculateArcPoint(float t, float maxDistance)
	{
		float x = t * maxDistance;
		float y = x * Mathf.Tan(radianAngle) - ((g * x * x) - (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

		return new Vector3(x, x, y);
	}
}
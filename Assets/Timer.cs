using UnityEngine;

public class Timer : MonoBehaviour
{
	public static float predictedTime;
	public Motor objectB;
	public Motor objectA;

	private void Start()
	{
		float h = objectA.transform.position.x - objectB.transform.position.x;
		float a = objectB.acceleration - objectA.acceleration;
		float b = 2 * (objectB.initialVelocity - objectA.initialVelocity);
		float c = -2 * h;

		predictedTime = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
		Debug.Log($"predicted time {predictedTime}");
	}
}
using UnityEngine;

public class BallLancher : MonoBehaviour
{
	public Rigidbody ball;
	public float h = 25f;
	public Transform target;

	[Range(1, 20)]
	public float power = 1;

	public float gravity = -18;

	private void Start()
	{
		ball.useGravity = false;
		gravity *= power;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			lunch();
		}
	}

	private void lunch()
	{
		Physics.gravity = Vector3.up * gravity;
		ball.useGravity = true;
		ball.velocity = calculateLunchVelocity();
	}

	private Vector3 calculateLunchVelocity()
	{
		float displacmentY = target.position.y - ball.position.y;
		Vector3 displacementXZ = new Vector3(target.position.x - ball.position.x, 0, target.position.z - ball.position.z);
		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
		Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacmentY - h) / gravity));
		return velocityXZ + velocityY;
	}
}
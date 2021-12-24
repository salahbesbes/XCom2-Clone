using UnityEngine;

public class SecondCam : MonoBehaviour
{
	public AnyClass unit;

	private AnyClass currentTarget;
	public float speed = 4;
	public float angle;
	public float radius = 2;
	public float degreesPerSecond = 100;

	private void Start()
	{
		switchTrarget();
	}

	private void switchTrarget()
	{
		if (currentTarget == null) return;
		turnTheModel(currentTarget.aimPoint.position);
	}

	private void turnTheModel(Vector3 target)
	{
		Vector3 dir = target - transform.position;
		// handle rotation on axe Y
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		// smooth the rotation of the turrent
		Vector3 rotation = Quaternion.Lerp(transform.rotation,
						lookRotation,
						Time.deltaTime * speed
						)
						.eulerAngles;

		transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
	}

	private void rotateCam()
	{
		angle += degreesPerSecond * Time.deltaTime;

		angle %= 360;

		Vector3 orbit = (Vector3.forward + Vector3.up) * radius;
		orbit = Quaternion.Euler(0, angle, 0) * orbit;

		transform.position = unit.transform.position + orbit;
	}

	private void FixedUpdate()
	{
		// todo: create an event Listner
		currentTarget = unit.currentTarget;
		switchTrarget();
		if (currentTarget == null) return;
		Vector3 dir = currentTarget.transform.position - unit.transform.position;
		float distanceCam = (currentTarget.transform.position - transform.position).magnitude;
		if (dir.magnitude + 2 > distanceCam)
		{
			rotateCam();
		}
	}
}
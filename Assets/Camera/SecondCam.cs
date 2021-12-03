using UnityEngine;

public class SecondCam : MonoBehaviour
{
	public AnyClass unit;

	private AnyClass currentTarget;
	public float speed = 4;

	private void Start()
	{
		switchTrarget();
	}

	private void switchTrarget()
	{
		currentTarget = unit.currentTarget;
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
}
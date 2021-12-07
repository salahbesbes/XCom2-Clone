using UnityEngine;

public class procedualGeneration : MonoBehaviour
{
	public float speed = 5;
	public Transform targ;
	public float xVal = 0;
	public float angle;
	public float radius = 10;
	public float degreesPerSecond = 30;
	private LayerMask targetLayer;
	private Vector3 direction;

	private void Start()
	{
		targetLayer = LayerMask.GetMask("Unwalkable");
		direction = (transform.position - targ.transform.position).normalized;
		radius = Vector3.Distance(targ.transform.position, transform.position);
	}

	private void changedirection(Transform target)
	{
		Vector3 targetDir = (target.position - transform.position).normalized;
		float angle = Vector3.Angle(targetDir, transform.forward);
		Debug.Log($"{angle}");
		if (angle == 0)
		{
			Debug.Log($"angle between target is 0");
			return;
		}
		else
		{
			float oldangle = transform.rotation.y;
			oldangle = Quaternion.Angle(transform.rotation, target.rotation);
			Debug.Log($"old {oldangle}");
			transform.rotation = Quaternion.AngleAxis(oldangle - 90, Vector3.up);
		}
	}

	private void FixedUpdate()
	{
		angle += degreesPerSecond * Time.deltaTime;

		if (angle > 360)
		{
			angle -= 360;
		}

		Vector3 orbit = Vector3.forward * radius;
		orbit = Quaternion.LookRotation(direction) * Quaternion.Euler(0, angle, 0) * orbit;

		transform.position = targ.transform.position + orbit;
	}

	private void OnDrawGizmos()
	{
		Vector3 dir = targ.position - transform.position;
		Ray ray = new Ray(transform.position.normalized, dir);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(ray);
	}
}
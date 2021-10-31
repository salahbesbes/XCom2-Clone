using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public bool holdDownShooting = true;
	public bool shooting = false;
	public float bulletSpeed = 100f;
	public float bouncingForce = 0f;
	public bool readyToShoot = true;
	public bool reloading = false;
	public int bulletLeft;
	public int bulletsShot;
	public int maxMagazine = 100;
	public float bulletRange = 50f;
	public float spread = 1f;
	public float timeBetweenShooting = 0.2f;
	public float timeBetweenShots = 0.06f;
	public GameObject bulletPrefab;
	public Transform startPoint;

	public Camera fps_Cam;

	public void Start()
	{
		bulletLeft = maxMagazine;
	}

	public void Update()
	{
		CheckForInput();
	}

	public void CheckForInput()
	{
		if (holdDownShooting) shooting = Input.GetMouseButton(0);
		else shooting = Input.GetMouseButtonDown(0);
		Debug.Log($"{shooting} {readyToShoot} {!reloading} {bulletLeft > 0}");
		if (shooting && readyToShoot && !reloading && bulletLeft > 0)
		{
			StartCoroutine(Shoot());
		}
	}

	public IEnumerator Shoot()
	{
		// need to read documentation on the ViewportPointToRay method Vector3(0.5f, 0.5f,
		// 0) the ray is at the center of the camera view. The bottom-left of the camera is
		// (0,0); the top-right is (1,1).
		Ray ray = fps_Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			Debug.Log($"are shooting");
			Vector3 dir = hit.point - transform.position;
			float x = Random.Range(-spread, spread);
			float y = Random.Range(-spread, spread);
			dir = dir + new Vector3(x, y, 0);
			GameObject bullet = Instantiate(bulletPrefab, startPoint.position, Quaternion.identity);
			bullet.transform.forward = dir.normalized;
			Rigidbody rb = bullet.GetComponent<Rigidbody>();

			rb.AddForce(dir * bulletSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
			rb.AddForce(fps_Cam.transform.up * bouncingForce, ForceMode.Impulse);
			bulletLeft--;
			bulletsShot++;

			Invoke("Shoot", timeBetweenShooting);
			yield return null;
		}
		else
			print("I'm looking at nothing!");
	}

	public void OnDrawGizmos()
	{
		Ray ray = fps_Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		Gizmos.color = Color.red;
		Gizmos.DrawRay(ray);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 100f))
		{
			Gizmos.DrawSphere(hit.point, 0.1f);
		}
	}
}
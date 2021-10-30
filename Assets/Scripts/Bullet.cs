using UnityEngine;

public class Bullet : MonoBehaviour
{
	public bool holdDownShooting = true;
	public bool shooting = false;
	public bool readyToShoot = true;
	public bool reloading = false;
	public int bulletLeft;
	public int maxMagazine = 100;
	public float bulletRange = 50f;

	public Camera rpg_Cam;

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

		if (shooting && readyToShoot && !reloading && bulletLeft > 0)
		{
			Shoot();
		}
	}

	public void Shoot()
	{
		Ray ray = rpg_Cam.ViewportPointToRay(new Vector3(0, 0.5f, 0.5f));

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, bulletRange))
		{
			Debug.Log($"{hit.transform.position}");
		}
		else
		{
			Debug.Log($"hit in the air");
		}
	}

	public void OnDrawGizmos()
	{
		Ray ray = rpg_Cam.ViewportPointToRay(new Vector3(0, 0.5f, 0.5f));
		Gizmos.DrawRay(ray);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, bulletRange))
		{
			Gizmos.DrawIcon(hit.transform.position, "center of the screen", true);
		}
	}
}
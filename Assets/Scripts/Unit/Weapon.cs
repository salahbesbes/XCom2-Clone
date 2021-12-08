using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
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
	public float bulletRange = 100;

	[Range(0, 0.5f)]
	public float spread = 1f;

	public float timeBetweenShooting = 0.2f;
	public float timeBetweenShots = 0.06f;
	public bool shutGun = false;
	public int bulletInOneShot = 8;
	public GameObject bulletPrefab;
	public Transform startPoint;
	private NodeGrid grid;
	public Camera fps_Cam;
	public AnyClass player;

	public void Start()
	{
		bulletLeft = maxMagazine;
		grid = FindObjectOfType<NodeGrid>();
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(startPoint.position, fwd, Color.green);
	}

	public void Update()
	{
		Vector3 dir = (player.currentTarget.aimPoint.position - player.aimPoint.position).normalized;
		Debug.DrawRay(startPoint.position, dir * bulletRange, Color.green);
	}

	public void CheckForInput()
	{
		////if (holdDownShooting) shooting = Input.GetMouseButton(1);
		////else shooting = Input.GetMouseButtonDown(1);
		//if (holdDownShooting) shooting = Input.GetKey(KeyCode.R);
		//else shooting = Input.GetKeyDown(KeyCode.R);
		////Debug.Log($"{shooting} {readyToShoot} {!reloading} {bulletLeft > 0}");
		//if (shooting && readyToShoot && !reloading && bulletLeft > 0)
		//{
		//	StartCoroutine(startShooting());
		//}
		//if (Input.GetKeyDown(KeyCode.R))
		//	reloading = true;
		//if (reloading && !shooting && bulletLeft == maxMagazine) Reload();
	}

	public IEnumerator Reload(ReloadAction reload)
	{
		Debug.Log($"start reloading");
		yield return new WaitForSeconds(2f);
		bulletLeft = maxMagazine;
		reloading = false;
		Debug.Log($"finish reloading");
		player.FinishAction(reload);
	}

	public void Shoot(RaycastHit hit)
	{
		// slow don the rate of shooting using delay method
		if (readyToShoot)
		{
			readyToShoot = false;
			StartCoroutine(DelayShooting());
		}
		// hi is always a point in the center of the fps_screen ( always have fixed height
		// and width) we get the node exactly under that point
		Node hitNode = grid.getNodeFromTransformPosition(null, hit.point);
		Vector3 targetPoint = hitNode.coord;
		// this is the direction between the player node to the hit point node
		Vector3 dir = targetPoint - player.currentPos.coord;

		// sp to different direction around the target
		float x = Random.Range(-spread, spread);
		float y = Random.Range(-spread, spread);

		dir = dir + new Vector3(x, y, 0);
		//Debug.Log($"hit point {hit.point}  node.coord {hitNode.coord} dir {dir}");

		GameObject bullet = Instantiate(bulletPrefab, startPoint.position, Quaternion.identity);
		// we orient the bullet to the direction created
		bullet.transform.forward = dir.normalized;

		Rigidbody rb = bullet.GetComponent<Rigidbody>();
		// the bullet create at any point folow the direction (wich have 0 on y at any
		// direction) so the bullet created at the startPoint stays on the same height
		rb.AddForce(dir * bulletSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
		//rb.AddForce(fps_Cam.transform.up * bouncingForce, ForceMode.Impulse);

		bulletLeft--;
		bulletsShot++;
	}

	public IEnumerator startShooting(ShootAction shoot)
	{
		// need to read documentation on the ViewportPointToRay method Vector3(0.5f, 0.5f,
		// 0) the ray is at the center of the camera view. The bottom-left of the camera is
		// (0,0); the top-right is (1,1).

		if (readyToShoot && !reloading && bulletLeft > 0)
		{
			//if (Physics.Raycast(transform.position, fwd, out objectHit, 50))
			//{ }
			//Ray ray = fps_Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			Vector3 fwd = transform.TransformDirection(Vector3.forward);
			Vector3 dir = (player.currentTarget.aimPoint.position - player.aimPoint.position).normalized;
			Debug.DrawRay(startPoint.position, dir * bulletRange, Color.green);
			RaycastHit hit;
			if (Physics.Raycast(startPoint.position, dir, out hit, bulletRange))
			{
				if (shutGun)
				{
					for (int i = 0; i < bulletInOneShot; i++)
					{
						Shoot(hit);
						// delay between each instantiate method
						yield return new WaitForSeconds(timeBetweenShots);
					}
				}
				else
				{
					//todo: Replace this system by the cost Action point
					float halfMagazine = maxMagazine / 2;
					// fire until ( not required )
					while (bulletLeft >= halfMagazine)
					{
						Shoot(hit);
						yield return new WaitForSeconds(timeBetweenShooting);
					}
					//maxMagazine = 0;
				}
			}
			else
			{
				Debug.Log($" out of range!  bullet range is  {bulletRange} ");
				print("I'm looking at nothing!");
			}
		}
		Debug.Log($"finish shotting");
		player.FinishAction(shoot);
	}

	private IEnumerator DelayShooting()
	{
		yield return new WaitForSeconds(timeBetweenShooting);
		readyToShoot = true;
	}

	public void OnDrawGizmos()
	{
		if (grid != null)
		{
			//Ray ray = fps_Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			//RaycastHit hit;
			//Gizmos.color = Color.red;
			//if (Physics.Raycast(ray, out hit))
			//{
			//	Node hitNode = grid.getNodeFromTransformPosition(null, hit.point);
			//	Vector3 targetPoint = hitNode.coord;

			//	//Gizmos.DrawLine(player.actualPos.coord, targetPoint);
			//}
		}
	}
}
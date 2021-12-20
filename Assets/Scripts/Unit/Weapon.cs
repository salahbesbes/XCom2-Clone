using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public WeaponData weaponType;
	public GameObject bulletPrefab;
	public Transform startPoint;
	private NodeGrid grid;
	public Camera fps_Cam;
	public AnyClass player;
	public Projection _projection;
	public float power;

	public void Start()
	{
		//bulletLeft = maxMagazine;
		grid = FindObjectOfType<NodeGrid>();
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(startPoint.position, fwd, Color.green);
	}

	public void Update()
	{
		if (player.currentTarget != null)
		{
			Vector3 dir = (player.currentTarget.aimPoint.position - player.aimPoint.position).normalized;
			Debug.DrawRay(startPoint.position, dir * weaponType.bulletRange, Color.green);
		}

		//_projection.SimulateTrajectory(weaponType.Ammo, startPoint.position, startPoint.forward * weaponType.bouncingForce * Time.fixedDeltaTime);
	}

	public IEnumerator Reload(ReloadAction reload)
	{
		Debug.Log($"start reloading");
		yield return new WaitForSeconds(2f);
		weaponType.bulletLeft = weaponType.maxMagazine;
		weaponType.reloading = false;
		Debug.Log($"finish reloading");
		player.FinishAction(reload);
	}

	public void Shoot(RaycastHit hit)
	{
		// slow don the rate of shooting using delay method
		if (weaponType.readyToShoot)
		{
			weaponType.readyToShoot = false;
			StartCoroutine(DelayShooting());
		}
		// hi is always a point in the center of the fps_screen ( always have fixed height
		// and width) we get the node exactly under that point
		Node hitNode = grid.getNodeFromTransformPosition(null, hit.point);
		Vector3 targetPoint = hitNode.coord;
		// this is the direction between the player node to the hit point node
		Vector3 dir = targetPoint - player.currentPos.coord;

		// sp to different direction around the target
		float x = Random.Range(-weaponType.spread, weaponType.spread);
		float y = Random.Range(-weaponType.spread, weaponType.spread);

		dir = dir + new Vector3(x, y, 0);
		//Debug.Log($"hit point {hit.point}  node.coord {hitNode.coord} dir {dir}");

		GameObject bullet = Instantiate(bulletPrefab, startPoint.position, Quaternion.identity);
		// we orient the bullet to the direction created
		bullet.transform.forward = dir.normalized;

		Rigidbody rb = bullet.GetComponent<Rigidbody>();
		// the bullet create at any point folow the direction (wich have 0 on y at any
		// direction) so the bullet created at the startPoint stays on the same height
		//rb.AddForce(fps_Cam.transform.up * weaponType.bouncingForce, ForceMode.Impulse);
		if (weaponType.bouncingForce > 0)
			rb.AddForce(startPoint.forward * weaponType.bouncingForce * Time.fixedDeltaTime, ForceMode.Impulse);
		else
			rb.AddForce(dir * weaponType.bulletSpeed * Time.fixedDeltaTime, ForceMode.Impulse);

		weaponType.bulletLeft--;
		weaponType.bulletsShot++;
	}

	public IEnumerator startShooting(ShootAction shoot)
	{
		// need to read documentation on the ViewportPointToRay method Vector3(0.5f, 0.5f,
		// 0) the ray is at the center of the camera view. The bottom-left of the camera is
		// (0,0); the top-right is (1,1).

		if (weaponType.readyToShoot && !weaponType.reloading && weaponType.bulletLeft > 0)
		{
			Vector3 dir = (player.currentTarget.aimPoint.position - player.aimPoint.position).normalized;
			Debug.DrawRay(startPoint.position, dir * weaponType.bulletRange, Color.green);
			RaycastHit hit;
			if (Physics.Raycast(startPoint.position, dir, out hit, weaponType.bulletRange))
			{
				weaponType.bulletsShot = 0;
				while (weaponType.bulletsShot <= weaponType.bulletInOneShot)
				{
					Shoot(hit);
					yield return new WaitForSeconds(weaponType.timeBetweenShooting);
				}
			}
			else
			{
				Debug.Log($" out of range!  bullet range is  {weaponType.bulletRange} ");
			}
		}
		Debug.Log($"finish shotting");
		player.FinishAction(shoot);
	}

	private IEnumerator DelayShooting()
	{
		yield return new WaitForSeconds(weaponType.timeBetweenShooting);
		weaponType.readyToShoot = true;
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

	public override string ToString()
	{
		return $"weapon: {this.name}";
	}
}
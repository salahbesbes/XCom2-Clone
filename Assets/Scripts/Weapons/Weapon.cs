using System.Threading.Tasks;
using UnityEngine;

public abstract class Weapon : SelectableItem
{
	//private WeaponData _weaponData;
	//public abstract WeaponData weaponType { get; set; }

	public Transform startPoint;
	public PlayerStateManager player;

	public virtual async Task Reload(ReloadAction reload)
	{ await Task.Yield(); }

	public virtual async Task startShooting(ShootAction shoot)
	{ await Task.Yield(); }






	public virtual void onHover()
	{
	}

	public virtual void onUpdate()
	{
	}



	public Vector3 ShotPercent(float AimPercent)
	{

		int randomValue = Random.Range(0, 100);
		if (randomValue <= AimPercent)
		{
			Debug.Log($"  head Shot ");
			return (player.CurrentTarget.aimPoint.position - startPoint.position).normalized;
		}
		else
		{
			Debug.Log($"missed shot");
			Vector3 newDir = (player.CurrentTarget.aimPoint.position - startPoint.position).normalized + Vector3.up * 0.05f;
			return newDir;
		}


	}
}

public class SelectableItem : MonoBehaviour
{
}
using System.Threading.Tasks;
using UnityEngine;

public abstract class Weapon : Mono
{
	//private WeaponData _weaponData;
	//public abstract WeaponData weaponType { get; set; }

	public Transform startPoint;
	public PlayerStateManager player;

	public virtual async Task Reload(ReloadAction reload)
	{ }

	public virtual async Task startShooting(ShootAction shoot)
	{ }

	private void OnEnable()
	{
		LineRenderer lr = GetComponent<LineRenderer>();
		if (lr != null)
		{
			lr.enabled = true;
		}
	}

	private void OnDisable()
	{
		LineRenderer lr = GetComponent<LineRenderer>();
		if (lr != null)
		{
			lr.enabled = false;
		}
	}

	public virtual float howMuchVisibleTheTArgetIs()
	{
		return 100;
	}


	public virtual void onHover()
	{

	}

	public virtual void onUpdate()
	{

	}



}


public class Mono : MonoBehaviour
{

}
using System.Threading.Tasks;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	//private WeaponData _weaponData;
	//public abstract WeaponData weaponType { get; set; }

	public Transform startPoint;
	public AnyClass player;

	public virtual async Task Reload(ReloadAction reload)
	{ }

	public virtual async Task startShooting(ShootAction shoot)
	{ }

	public virtual void onHover()
	{
	}

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
		return 1;
	}
}
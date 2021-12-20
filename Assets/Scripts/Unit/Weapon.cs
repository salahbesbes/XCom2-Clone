using System.Threading.Tasks;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	public virtual async Task Reload(ReloadAction reload)
	{ }

	public virtual async Task startShooting(ShootAction shoot)
	{ }

	public virtual void onHover()
	{
	}
}
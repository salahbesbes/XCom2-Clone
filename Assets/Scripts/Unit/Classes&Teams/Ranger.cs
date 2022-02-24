public class Ranger : PlayerStateManager, IClassRanger
{
	private ShutGun myWeapon;

	public new void Start()
	{
		myWeapon = weapon as ShutGun;
		base.Start();

	}


	public override void customUpdate()
	{
		weapon.onUpdate();
	}
}
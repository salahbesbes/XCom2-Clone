public class Assault : PlayerStateManager, IClassAssault
{
	private Ak47 myWeapon;

	public new void Start()
	{
		myWeapon = weapon as Ak47;
		base.Start();
	}
	public override void customUpdate()
	{
		weapon.onUpdate();
	}
}

internal interface IClassAssault
{
}

internal interface IClassRanger
{
}

internal interface IClassGrenadier
{
	public void CreateLunchGrenadeAction();

	public void LunchGrenadeCallback(LunchGrenadeAction action, Node destination);
}

public interface IBaseActions
{
	public void CreateNewMoveAction();

	public void CreateNewReloadAction();

	public void CreateNewShootAction();
}
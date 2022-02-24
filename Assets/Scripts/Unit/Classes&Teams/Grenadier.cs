public class Grenadier : PlayerStateManager, IClassGrenadier
{
	private HeavyWeapon myWeapon;

	public new void Start()
	{
		myWeapon = weapon as HeavyWeapon;
		base.Start();
	}


	public override void customUpdate()
	{
		weapon.onUpdate();
	}

	public void CreateLunchGrenadeAction()
	{
		Node potentialDestination = NodeGrid.Instance.getNodeFromMousePosition(secondCam);
		LunchGrenadeAction action = new LunchGrenadeAction(LunchGrenadeCallback, "LunchGrenade", potentialDestination);
		Enqueue(action);
	}

	public void LunchGrenadeCallback(LunchGrenadeAction action, Node dest)
	{
		myWeapon.lunchGrenade(action, dest);
	}
}
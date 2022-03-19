public class HealthModifier : Equipement
{
	public HealthData equipement;

	public override void picked(AnyClass unit)
	{
		unit.stats.addHealthModifier(equipement);
		unit.HealthBar.GetComponent<NewHealthBar>().onEquipementEventTrigger();
		Destroy(gameObject);
	}
}
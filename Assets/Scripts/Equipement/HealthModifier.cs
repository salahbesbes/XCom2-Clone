public class HealthModifier : Equipement
{
	public HealthData equipement;

	public override void picked(Unit unit)
	{
		unit.stats.addHealthModifier(equipement);
		unit.HealthBar.GetComponent<HealthBar>().onEquipementEventTrigger();
		Destroy(gameObject);
	}
}
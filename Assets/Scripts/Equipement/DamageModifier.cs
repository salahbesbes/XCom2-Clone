public class DamageModifier : Equipement
{
	public DamageData equipement;

	public override void picked(Unit unit)
	{
		unit.stats.addDamageModifier(equipement);
		unit.HealthBar.GetComponent<HealthBar>().onEquipementEventTrigger();
		Destroy(gameObject);
	}
}
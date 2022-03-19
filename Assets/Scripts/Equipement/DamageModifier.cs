public class DamageModifier : Equipement
{
	public DamageData equipement;

	public override void picked(AnyClass unit)
	{
		unit.stats.addDamageModifier(equipement);
		unit.HealthBar.GetComponent<NewHealthBar>().onEquipementEventTrigger();
		Destroy(gameObject);
	}
}
public class ArmorModifier : Equipement
{
	public ArmorData equipement;

	public override void picked(Unit unit)
	{
		unit.stats.addArmorModifier(equipement);
		unit.HealthBar.GetComponent<HealthBar>().onEquipementEventTrigger();
		Destroy(gameObject);
	}
}
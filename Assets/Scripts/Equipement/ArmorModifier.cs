public class ArmorModifier : Equipement
{
	public ArmorData equipement;

	public override void picked(AnyClass unit)
	{
		unit.stats.addArmorModifier(equipement);
		unit.HealthBar.GetComponent<NewHealthBar>().onEquipementEventTrigger();
		Destroy(gameObject);
	}
}
using UnityEngine;
public class handleHealthUnitBar : MonoBehaviour
{

	private int healthBeforeShoot;


	public GameObject unitHealth;
	private AnyClass thisUnit;
	private Transform HealthHolder;

	private void Start()
	{
		thisUnit = GetComponentInParent<AnyClass>();
		//tmpHealth = thisUnit.stats.unit.Health;
		HealthHolder = transform.Find("HealthHolder");
		updateHealthBar();
	}





	private void updateHealthBar()
	{
		float holderWidth = HealthHolder.GetComponent<RectTransform>().rect.width;
		healthBeforeShoot = thisUnit.stats.unit.Health;
		float unitwidth = holderWidth / healthBeforeShoot;
		for (int i = 0; i < healthBeforeShoot; i++)
		{
			GameObject unit = Instantiate(unitHealth, HealthHolder);
			unit.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(unitwidth, unit.transform.GetComponent<RectTransform>().sizeDelta.y);
			unit.transform.localScale = new Vector3(unitwidth, unit.transform.localScale.y, unit.transform.localScale.z);
		}
	}

	public void onDamage()
	{
		for (int i = healthBeforeShoot - 1; i >= thisUnit.stats.unit.Health; i--)
		{
			HealthHolder.GetChild(i).GetComponent<Renderer>().material.color = Color.gray;
		}
		healthBeforeShoot = thisUnit.stats.unit.Health;

	}

	public void onHeal()
	{

		for (int i = healthBeforeShoot; i < thisUnit.stats.unit.Health; i++)
		{
			HealthHolder.GetChild(i).GetComponent<Renderer>().material.color = Color.red;
		}
		healthBeforeShoot = thisUnit.stats.unit.Health;

	}
}

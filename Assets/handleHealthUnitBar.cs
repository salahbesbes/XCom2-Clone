using UnityEngine;

public class handleHealthUnitBar : MonoBehaviour
{
	private int healthBeforeShoot;

	private AnyClass thisUnit;
	public GameObject unitHealth;
	private Transform HealthHolder;

	private void Start()
	{
		thisUnit = GetComponentInParent<PlayerStateManager>();
		//tmpHealth = thisUnit.stats.unit.Health;
		HealthHolder = transform.Find("HealthHolder");

		if (thisUnit.team is RedTeam)
		{
			unitHealth.GetComponent<MeshRenderer>().material = Resources.Load("UnitHealth_Enemy_Mat", typeof(Material)) as Material;
		}
		else
		{
			unitHealth.GetComponent<MeshRenderer>().material = Resources.Load("UnitHealth_Player_Mat", typeof(Material)) as Material;
		}

		updateHealthBar();
	}

	private void updateHealthBar()
	{
		float holderWidth = HealthHolder.GetComponent<RectTransform>().rect.width;
		healthBeforeShoot = thisUnit.stats.unit.Health.Value;
		healthBeforeShoot = thisUnit.stats.unit.Health.Value;
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
		Debug.Log($"{transform.parent.name}");
		for (int i = healthBeforeShoot - 1; i >= thisUnit.stats.unit.Health.Value; i--)
		{
			HealthHolder.GetChild(i).GetComponent<Renderer>().material.color = Color.gray;
		}
		healthBeforeShoot = thisUnit.stats.unit.Health.Value;
	}

	public void onHeal()
	{
		for (int i = healthBeforeShoot; i < thisUnit.stats.unit.Health.Value; i++)
		{
			HealthHolder.GetChild(i).GetComponent<Renderer>().material.color = Color.red;
		}
		healthBeforeShoot = thisUnit.stats.unit.Health.Value;
	}
}
using UnityEngine;
public class handleHealthUnitBar : MonoBehaviour
{

	private int healthBeforeShoot;





	private AnyClass thisUnit;
	public GameObject unitHealth;
	private Transform HealthHolder;

	private void Start()
	{
		thisUnit = GetComponentInParent<AnyClass>();
		//tmpHealth = thisUnit.stats.unit.Health;
		HealthHolder = transform.Find("HealthHolder");
		Player test = thisUnit as Player;


		if (test == null)
		{
			unitHealth.GetComponent<MeshRenderer>().material = Resources.Load("UnitHealth_Enemy_Mat", typeof(Material)) as Material;
		}
		else
		{
			unitHealth.GetComponent<MeshRenderer>().material = Resources.Load("UnitHealth_Player_Mat", typeof(Material)) as Material;

		}


		//if (test == null)
		//{
		//	Material redMaterial = Resources.Load("unithealth", typeof(Material)) as Material;
		//	redMaterial.color = Color.red;
		//	unitHealth.GetComponent<MeshRenderer>().material = redMaterial;
		//	Debug.Log($"tried with red material");


		//}
		//else
		//{

		//	Material greenMaterial = Resources.Load("unithealth", typeof(Material)) as Material;
		//	greenMaterial.color = Color.green;
		//	unitHealth.GetComponent<MeshRenderer>().material = greenMaterial;
		//}
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

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	private PlayerStateManager thisUnit;
	private int healthBeforeShoot;
	private int healthafterShoot;
	private Transform HealthHolder;
	private Transform armorHolder;
	public GameObject unitHealth;
	public GameObject unitArmor;
	private int armor = 5;

	private void Start()
	{
		thisUnit = GetComponentInParent<PlayerStateManager>();
		HealthHolder = transform.Find("holder");
		armorHolder = transform.Find("armor");
		healthBeforeShoot = 13;
		armor = 5;
		updateBar(HealthHolder, unitHealth, 13);
		updateBar(armorHolder, unitArmor, 7);
	}

	public void updateBar(Transform HealthHolder, GameObject rawImage, int maxValue)
	{
		float holderWidth = HealthHolder.GetComponent<RectTransform>().rect.width;

		float unitwidth = holderWidth / maxValue;
		Debug.Log($" holderwidth  {holderWidth}  unit width {unitwidth} ");
		Vector3 leftCornerPosition = new Vector3((HealthHolder.position.x - holderWidth / 2) + unitwidth / 2, 0, 0);
		Debug.Log($"{leftCornerPosition}");
		for (int i = 0; i < maxValue; i++)
		{
			GameObject unit = Instantiate(rawImage, HealthHolder, true);
			unit.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(unitwidth, unit.transform.GetComponent<RectTransform>().sizeDelta.y);
			unit.transform.position = new Vector3(leftCornerPosition.x + (i * unitwidth), HealthHolder.position.y, HealthHolder.position.z);
			//unit.transform.localScale = new Vector3(unitwidth, unit.transform.localScale.y, unit.transform.localScale.z);
		}
	}

	public async Task onDamage()
	{
		//for (int i = healthBeforeShoot - 1; i >= thisUnit.stats.unit.Health; i--)
		healthafterShoot = healthBeforeShoot - 3;
		for (int i = healthBeforeShoot - 1; i >= healthafterShoot; i--)
		{
			HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.green;
		}
		for (int i = healthBeforeShoot - 1; i >= healthafterShoot; i--)
		{
			HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.grey;
			await Task.Delay(150);
		}

		healthBeforeShoot = healthafterShoot;
		//healthBeforeShoot = thisUnit.stats.unit.Health;
	}

	public async Task onHeal()
	{
		healthafterShoot = healthBeforeShoot + 3;

		for (int i = healthBeforeShoot; i < healthafterShoot; i++)
		{
			HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.green;
		}
		for (int i = healthBeforeShoot; i < healthafterShoot; i++)
		{
			HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.red;
			await Task.Delay(150);
		}
		healthBeforeShoot = healthafterShoot;
		//healthBeforeShoot = thisUnit.stats.unit.Health;
	}

	private async void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			await onDamage();
			updateArmor();
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			await onHeal();
		}
	}

	public void updateArmor()
	{
		for (int i = 7 - 1; i >= armor; i--)
		{
			armorHolder.GetChild(i).GetComponent<RawImage>().color = Color.gray;
		}
	}
}
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	private Transform HealthHolder;
	private Transform armorHolder;
	public GameObject unitHealth;
	public GameObject unitArmor;

	private AnyClass target;
	private AnyClass previousStats;
	private TextMeshProUGUI healthUI;
	private TextMeshProUGUI armorUI;

	private void Start()
	{
		HealthHolder = transform.Find("healthHolder");
		armorHolder = transform.Find("armorHolder");
		healthUI = transform.Find("health").GetComponent<TextMeshProUGUI>();
		armorUI = transform.Find("armor").GetComponent<TextMeshProUGUI>();
	}

	public void onTargetChange()
	{
		target = GameStateManager.Instance.SelectedUnit.CurrentTarget;

		healthUI.text = $"{target.stats.unit.Health}";
		armorUI.text = $"{target.stats.unit.armor.Value}";

		initBar(HealthHolder, unitHealth, target.stats.unit.maxHealth);
		initBar(armorHolder, unitArmor, 7);
		updateHealthBar(target);
		updateArmorBar(target);
	}

	public void updateHealthBar(AnyClass target, bool player = false)
	{
		if (player == false)
		{
			for (int i = target.stats.unit.Health.Value; i < target.stats.unit.maxHealth; i++)
			{
				HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.grey;
			}
		}
		else
		{
			for (int i = target.stats.unit.maxHealth - 1; i >= target.stats.unit.Health.Value; i--)
			{
				HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.grey;
			}
		}
	}

	public void updateArmorBar(AnyClass target, bool player = false)
	{
		if (target.stats.unit.armor.Value > armorHolder.childCount)
		{
			Debug.Log($"  too much armor :  armor is {target.stats.unit.armor.Value} > max val {armorHolder.childCount} ");
		}

		if (player == false)
		{
			for (int i = armorHolder.childCount - 1; i >= target.stats.unit.armor.Value; i--)
			{
				armorHolder.GetChild(i).GetComponent<RawImage>().color = Color.grey;
			}
		}
		else
		{
			for (int i = target.stats.unit.armor.Value; i < armorHolder.childCount; i++)
			{
				armorHolder.GetChild(i - target.stats.unit.armor.Value).GetComponent<RawImage>().color = Color.grey;
			}
		}
	}

	public void onAllyChange()
	{
		target = GameStateManager.Instance.SelectedUnit;
		healthUI.text = $"{target.stats.unit.Health}";
		armorUI.text = $"{target.stats.unit.armor.Value}";

		initBar(HealthHolder, unitHealth, target.stats.unit.maxHealth, true);
		initBar(armorHolder, unitArmor, 7, true);
		updateHealthBar(target, true);
		updateArmorBar(target, true);
	}

	public void initBar(Transform holder, GameObject rawImage, int maxValue, bool player = false)
	{
		float holderWidth = holder.GetComponent<RectTransform>().rect.width;

		// Todo : explenation on this code (only this way worked for me)
		int childs = holder.childCount;
		for (int i = childs - 1; i >= 0; i--)
		{
			DestroyImmediate(holder.GetChild(i).gameObject);
		}

		float unitwidth = holderWidth / maxValue;
		Vector3 leftCornerPosition = new Vector3((holder.position.x - holderWidth / 2) + unitwidth / 2, 0, 0);
		for (int i = 0; i < maxValue; i++)
		{
			GameObject unit = Instantiate(rawImage, holder, true);
			unit.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(unitwidth, holder.transform.GetComponent<RectTransform>().sizeDelta.y);
			unit.transform.position = new Vector3(leftCornerPosition.x + (i * unitwidth), holder.position.y, holder.position.z);
			//unit.transform.localScale = new Vector3(unitwidth, unit.transform.localScale.y, unit.transform.localScale.z);
		}
	}

	public async void onTargetDamaged()
	{
		target = GameStateManager.Instance.SelectedUnit.CurrentTarget;
		for (int i = target.stats.unit.maxHealth - 1; i >= target.stats.unit.Health.Value; i--)
		{
			HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.green;
		}
		for (int i = target.stats.unit.maxHealth - 1; i >= target.stats.unit.Health.Value; i--)
		{
			HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.grey;
			await Task.Delay(50);
		}
	}

	public async void onPlayerDamaged()
	{
		target = GameStateManager.Instance.SelectedUnit;
		for (int i = target.stats.unit.maxHealth - 1; i >= target.stats.unit.Health.Value; i--)
		{
			HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.green;
		}
		for (int i = target.stats.unit.maxHealth - 1; i >= target.stats.unit.Health.Value; i--)
		{
			HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.grey;
			await Task.Delay(50);
		}
	}

	//public async Task onHeal()
	//{
	//	for (int i = healthBeforeShoot; i < target.stats.unit.Health; i++)
	//	{
	//		HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.green;
	//	}
	//	for (int i = healthBeforeShoot; i < target.stats.unit.Health; i++)
	//	{
	//		HealthHolder.GetChild(i).GetComponent<RawImage>().color = Color.red;
	//		await Task.Delay(150);
	//	}
	//	healthBeforeShoot = target.stats.unit.Health;
	//}

	public void updateArmor()
	{
		Stat armor = target.stats.unit.armor;

		if (armor.Value >= 7)
		{
			Debug.Log($" armor too mush for health Ui ");
		}
		for (int i = 7 - 1; i >= armor.Value; i--)
		{
			armorHolder.GetChild(i).GetComponent<RawImage>().color = Color.gray;
		}
	}
}
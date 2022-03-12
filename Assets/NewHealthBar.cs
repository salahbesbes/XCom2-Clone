using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewHealthBar : MonoBehaviour
{
	private Image backSlider;
	private Image frontSlider;
	private PlayerStateManager thisUnit;
	private TextMeshProUGUI healthText;
	private TextMeshProUGUI armorText;
	private Transform armorBar;

	private void Start()
	{
		backSlider = transform.GetChild(0).Find("back").GetComponent<Image>();
		frontSlider = transform.GetChild(0).Find("front").GetComponent<Image>();
		healthText = transform.Find("Image").Find("health").GetComponent<TextMeshProUGUI>();

		armorBar = transform.GetChild(1).Find("Holder");
		armorText = transform.GetChild(1).Find("Background").Find("ArmorText").GetComponent<TextMeshProUGUI>();

		thisUnit = GetComponentInParent<PlayerStateManager>();
		float health = thisUnit.stats.unit.Health / thisUnit.stats.unit.maxHealth;
		healthText.text = $"{thisUnit.stats.unit.Health}";
		armorText.text = $"{thisUnit.stats.unit.armor.Value}";

		updateArmor();
		backSlider.fillAmount = health;
		frontSlider.fillAmount = health;
	}

	private void updateArmor()
	{
		thisUnit = GetComponentInParent<PlayerStateManager>();
		int armor = thisUnit.stats.unit.armor.Value;
		for (int i = 0, j = 0; i < armorBar.childCount; i++, j++)
		{
			Transform child = armorBar.GetChild(i);
			Image image = child.GetComponent<Image>();
			Color color = image.color;
			if (j < armor)
			{

				color.a = 1f;
				image.color = color;
			}
			else
			{
				color.a = 0f;
				image.color = color;

			}
		}
	}

	public async void onDamage()
	{
		thisUnit = GetComponentInParent<PlayerStateManager>();
		float health = (float)thisUnit.stats.unit.Health / (float)thisUnit.stats.unit.maxHealth;
		healthText.text = $"{thisUnit.stats.unit.Health}";
		updateArmor();
		await updateFrontImage(0.75f, health);
		await updateBackImage(0.5f, health);
	}

	private async Task updateBackImage(float lerpDuration = 0.5f, float newVal = 1)
	{
		float timeElapsed = 0;
		float currentVal = backSlider.fillAmount;
		while (timeElapsed < lerpDuration)
		{
			backSlider.fillAmount = Mathf.Lerp(currentVal, newVal, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;
			await Task.Yield();
		}
	}

	private async Task updateFrontImage(float lerpDuration = 0.5f, float newVal = 1)
	{
		float timeElapsed = 0;
		float currentVal = frontSlider.fillAmount;
		while (timeElapsed < lerpDuration)
		{
			frontSlider.fillAmount = Mathf.Lerp(currentVal, newVal, timeElapsed / lerpDuration);
			timeElapsed += Time.deltaTime;
			await Task.Yield();
		}
	}
}
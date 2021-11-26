using UnityEngine;

public class handleHealthUnitBar : MonoBehaviour
{
	private int health = 20;

	public int Health
	{ get => health; set { health = Mathf.Clamp(value, 0, MaxHealth); } }

	public int MaxHealth = 20;
	public GameObject unitHealth;

	private void Start()
	{
		Health = MaxHealth;
		updateHealthBar();
	}

	private void updateHealthBar()
	{
		float holderWidth = transform.GetComponent<RectTransform>().rect.width;
		float unitwidth = holderWidth / Health;

		for (int i = 0; i < Health; i++)
		{
			GameObject unit = Instantiate(unitHealth, transform);
			unit.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(unitwidth, unit.transform.GetComponent<RectTransform>().sizeDelta.y);
			unit.transform.localScale = new Vector3(unitwidth, unit.transform.localScale.y, unit.transform.localScale.z);
		}
	}

	public void onDamage(int damageValue)
	{
		Health -= damageValue;
		for (int i = Health; i < Health + damageValue; i++)
		{
			transform.GetChild(i).GetComponent<Renderer>().material.color = Color.gray;
		}
	}

	public void onHeal(int healVAlue)
	{
		Health += healVAlue;
		for (int i = Health - 1; i >= Health - healVAlue; i--)
		{
			transform.GetChild(i).GetComponent<Renderer>().material.color = Color.red;
		}
	}
}
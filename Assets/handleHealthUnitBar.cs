using UnityEngine;

public class handleHealthUnitBar : MonoBehaviour
{
	public int health = 20;
	public GameObject unitHealth;

	private void Start()
	{
		float holderWidth = transform.GetComponent<RectTransform>().rect.width;
		float unitwidth = holderWidth / health;

		for (int i = 0; i < health; i++)
		{
			GameObject unit = Instantiate(unitHealth, transform);
			unit.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(unitwidth, unit.transform.GetComponent<RectTransform>().sizeDelta.y);
			unit.transform.localScale = new Vector3(unitwidth, unit.transform.localScale.y, unit.transform.localScale.z);
		}
	}

	// Update is called once per frame
	private void Update()
	{
	}
}
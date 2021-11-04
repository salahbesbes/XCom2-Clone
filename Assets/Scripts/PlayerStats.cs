using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
	public float Health;
	public float MaxHealth = 20;
	public float MoveRange = 10;
	public float MoveVision = 20;
	public float ActionPoint = 10;
	public Transform holder;
	public GameObject Textprefab;

	public void Start()
	{
		List<object> localprops = new List<object>() { Health, MoveRange, MoveVision, ActionPoint };

		foreach (var item in localprops)
		{
			GameObject text = Instantiate(Textprefab);
			text.GetComponent<Text>().text = $"{nameof(item)}: {item}";
			text.transform.SetParent(holder);
		}
	}
}
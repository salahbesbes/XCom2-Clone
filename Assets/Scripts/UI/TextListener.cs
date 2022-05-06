using TMPro;
using UnityEngine;

public class TextListener : MonoBehaviour
{
	TextMeshProUGUI TextComponent;

	private void Start()
	{
		TextComponent = GetComponent<TextMeshProUGUI>();
	}

	public void UpdateText(string message)
	{
		TextComponent.text = $"{message}";

	}

}

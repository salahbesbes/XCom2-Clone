using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextListener : MonoBehaviour
{
	TextMeshProUGUI TextComponent;

	private void Start()
	{
		TextComponent = GetComponent<TextMeshProUGUI>();
	}

	public void UpdateText(string message)
	{
		transform.parent.GetComponent<Image>().enabled = true;
		TextComponent.text = $"{message}";
		eraseText();

	}


	private async void eraseText()
	{
		await Task.Delay(2000);
		TextComponent.text = $"";
		transform.parent.GetComponent<Image>().enabled = false;


	}

}

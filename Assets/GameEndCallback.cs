using TMPro;
using UnityEngine;

public class GameEndCallback : MonoBehaviour
{
	private Transform textUI;
	private TextMeshProUGUI endMessage;

	private void Start()
	{
		textUI = transform.GetChild(0).GetChild(0);
		endMessage = textUI.GetComponent<TextMeshProUGUI>();
	}

	public void showEndMessage()
	{
		Debug.Log($"sow message");
		transform.gameObject.SetActive(true);
		endMessage.text = "Some Team Win";
	}
}
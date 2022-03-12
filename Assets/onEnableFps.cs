using UnityEngine;

public class onEnableFps : MonoBehaviour
{
	private void OnEnable()
	{
		if (GameStateManager.Instance?.players == null || GameStateManager.Instance?.enemies == null)
			return;
	}
}
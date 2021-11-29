using UnityEngine;
using UnityEngine.UI;

namespace gameEventNameSpace

{
	public class StateListner : BaseGameEventListner<BaseState<GameStateManager>, BaseStateEvent, UnityStateEvent>
	{
		public Text UiText;

		public void updateUiText(BaseState<GameStateManager> newState)
		{
			if (UiText == null || UiText?.text == null)
			{
				Debug.Log($"please select Text UI ");
				return;
			}

			UiText.text = $"{newState.name} ";
		}
	}
}
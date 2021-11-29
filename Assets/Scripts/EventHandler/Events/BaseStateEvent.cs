using UnityEngine;

namespace gameEventNameSpace
{
	[CreateAssetMenu(fileName = "new BaseState Event ", menuName = "Game Event / BaseState Event")]
	public class BaseStateEvent : BaseGameEvent<BaseState<GameStateManager>>
	{
		//public void Raise(BaseState<GameStateManager> newState) ;
	}
}
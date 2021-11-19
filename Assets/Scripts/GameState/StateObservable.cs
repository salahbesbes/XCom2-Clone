using UnityEngine.UI;

public class StateObservable : ObserverAbstraction<GameStateManager, BaseState<GameStateManager>>
{
	public override void OnSubjectEventChanges(BaseState<GameStateManager> state)
	{
		GetComponent<Text>().text = $"state : {state}";
	}
}
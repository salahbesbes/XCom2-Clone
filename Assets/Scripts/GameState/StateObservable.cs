using UnityEngine.UI;

public class StateObservable : ObserverAbstraction<GameStateManager, GameBaseState>
{
	public override void OnSubjectEventChanges(GameBaseState state)
	{
		GetComponent<Text>().text = $"state : {state}";
	}
}
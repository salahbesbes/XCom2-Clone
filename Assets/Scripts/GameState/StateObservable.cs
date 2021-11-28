using UnityEngine;

[System.Serializable]
public class StateObservable : MonoBehaviour
{
	public void SubjectEventChanges(BaseState<GameStateManager> state)
	{
		//GetComponent<Text>().text = $"state : {state}";
	}

	public void test()
	{
	}
}
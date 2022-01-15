using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "ActionData", order = 51)]
[System.Serializable]
public class ActionData : ScriptableObject
{
	public string cost;

	public Sprite icon;

	//public AnimationType animationType;

	public string SwordName
	{
		get
		{
			return cost;
		}
	}

	public Sprite Icon
	{
		get
		{
			return icon;
		}
	}

	public PlayerAction Actionevent;
}

public enum AnimationType
{
	run,
	idel,
	walk,
	jump,
	shoot,
	aim,
	die,
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Action", menuName = "ActionData", order = 51)]
[System.Serializable]
public class ActionData : ScriptableObject
{
	[SerializeField]
	private string cost;

	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private RuntimeAnimatorController animationController;

	public string SwordName
	{
		get
		{
			return cost;
		}
	}

	public RuntimeAnimatorController AnimationController
	{
		get
		{
			return animationController;
		}
		set
		{
			animationController = value;
		}
	}

	public Sprite Icon
	{
		get
		{
			return icon;
		}
	}

	public PlayerEvent Actionevent;
}
using UnityEngine;

public class SecondCam : MonoBehaviour
{
	public AnyClass unit;

	private AnyClass currentTarget;

	private void Start()
	{
		switchTrarget();
	}

	private void Update()
	{
		switchTrarget();
	}

	private void switchTrarget()
	{
		currentTarget = unit.currentTarget;
		if (currentTarget != null)
		{
			transform.SetParent(unit.currentTarget.transform);
			transform.LookAt(currentTarget.transform);
		}
	}
}
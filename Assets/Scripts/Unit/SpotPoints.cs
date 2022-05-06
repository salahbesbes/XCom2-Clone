using System.Collections.Generic;
using UnityEngine;

public class SpotPoints : MonoBehaviour
{
	private void OnEnable()
	{
		Unit thisPlayer = transform.parent.parent.GetComponent<Unit>();
		thisPlayer.sportPoints = sportPoint;
	}

	public List<Transform> sportPoint = new List<Transform>();
}
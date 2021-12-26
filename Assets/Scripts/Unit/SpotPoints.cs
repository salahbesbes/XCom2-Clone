using System.Collections.Generic;
using UnityEngine;

public class SpotPoints : MonoBehaviour
{
	private void OnEnable()
	{
		AnyClass thisPlayer = transform.parent.parent.GetComponent<AnyClass>();
		thisPlayer.sportPoints = sportPoint;
	}

	public List<Transform> sportPoint = new List<Transform>();
}
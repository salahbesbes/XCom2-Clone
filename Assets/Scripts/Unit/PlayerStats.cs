using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log($"trigger");
	}

	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log($"collission");
	}
}
using UnityEngine;

public class Ammo : MonoBehaviour
{
	public GameObject fireEffect;

	public void SpawnDecalEffect(Collision hit, GameObject prefab, Quaternion? dirPass = null)
	{
		Quaternion dir = dirPass ?? hit.transform.rotation;
		// spawn the Effect in the direction of the Collider loking
		GameObject spawnedDecal = Instantiate(prefab, hit.contacts[0].point, dir);
		//spawnedDecal.transform.SetParent(hit.collider.transform);
	}
}
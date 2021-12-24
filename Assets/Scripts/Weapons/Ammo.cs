using UnityEngine;

public class Ammo : MonoBehaviour
{
	public ParticleSystem fireEffect;

	//public ParticleSystem hittingEffect;
	public GameObject metalHitEffect;
	public GameObject waterLeakEffect;
	public GameObject sandHitEffect;
	public GameObject stoneHitEffect;
	public GameObject woodHitEffect;
	public GameObject fleshHitEffect;
	public GameObject charachterHitEffect;
	public GameObject waterLeakExtinguishEffect;

	public void OnCollisionEnter(Collision collision)
	{
		//Debug.Log($"hit {collision.transform.parent.parent.name}");
		//Vector3 dir = collision.contacts[0].point - transform.forward;
		//Instantiate(hittingEffect, collision.contacts[0].point, collision.transform.rotation, collision.transform);

		if (collision.collider.sharedMaterial != null)
		{
			string materialName = collision.collider.sharedMaterial.name;

			switch (materialName)
			{
				case "Metal":
					SpawnDecal(collision, metalHitEffect);
					break;

				case "Sand":
					SpawnDecal(collision, sandHitEffect);
					break;

				case "Stone":
					SpawnDecal(collision, stoneHitEffect);
					break;

				case "WaterFilled":
					SpawnDecal(collision, waterLeakEffect);
					SpawnDecal(collision, metalHitEffect);
					break;

				case "Wood":
					SpawnDecal(collision, woodHitEffect);
					break;

				case "Meat":
					SpawnDecal(collision, fleshHitEffect);
					break;

				case "Character":
					SpawnDecal(collision, fleshHitEffect);
					break;

				case "WaterFilledExtinguish":
					SpawnDecal(collision, waterLeakExtinguishEffect);
					SpawnDecal(collision, metalHitEffect);
					break;

				default:
					Debug.Log($"cant fin the collider material");
					break;
			}
		}
	}

	public void SpawnDecal(Collision hit, GameObject prefab)
	{
		// spawn the Effect in the direction of the Collider loking
		GameObject spawnedDecal = Instantiate(prefab, hit.contacts[0].point, hit.transform.rotation);
		//spawnedDecal.transform.SetParent(hit.collider.transform);
	}
}
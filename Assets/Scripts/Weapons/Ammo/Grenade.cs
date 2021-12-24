using System.Threading.Tasks;
using UnityEngine;

public class Grenade : Ammo
{
	[SerializeField] private Rigidbody _rb;
	public GameObject ExplodeEffect;

	[Range(0, 5)]
	public int ExplosionIn;

	public void Init(Vector3 velocity, bool isGhost)
	{
		_rb.velocity = velocity;
	}

	public void OnCollisionEnter(Collision col)
	{
		ExplodeIn(col, ExplosionIn);
	}

	private async Task ExplodeIn(Collision col, int time)
	{
		_rb.isKinematic = true;
		await Task.Delay(1000 * time);
		GameObject obj = Instantiate(ExplodeEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
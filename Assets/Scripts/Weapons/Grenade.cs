using UnityEngine;

public class Grenade : Ammo
{
	[Range(0, 5)]
	public float ExpodeIn = 1;
	[SerializeField] private Rigidbody _rb;

	public async void OnCollisionEnter(Collision col)
	{
		_rb.isKinematic = true;
		await System.Threading.Tasks.Task.Delay((int)(ExpodeIn * 1000));
		SpawnDecal(col, fireEffect);
	}
}
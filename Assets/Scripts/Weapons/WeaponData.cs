using UnityEngine;

public class WeaponData : ScriptableObject
{
	public bool holdDownShooting;
	public bool shooting;
	public float bulletSpeed;
	public float bouncingForce;
	public bool readyToShoot;
	public bool reloading;
	public int bulletLeft;
	public int bulletsShot;
	public int maxMagazine;
	public float bulletRange;

	[Range(0, 0.5f)]
	public float spread;

	public float timeBetweenShooting;
	public float timeBetweenShots;
	public bool shutGun;
	public int bulletInOneShot;
	public Ammo Ammo;
}
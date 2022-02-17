using UnityEngine;

public class WeaponData : Item
{
	public bool holdDownShooting = false;
	public bool shooting = false;
	public bool readyToShoot = true;
	public bool reloading = false;

	[Range(9.8f, 1000)]
	public float ammoSpeed = 9.81f;

	public float bouncingForce;
	public int bulletLeft;
	public int bulletsShot = 0;
	public int maxMagazine;
	public float bulletRange = 100;

	[Range(0, 0.5f)]
	public float spread;

	public float timeBetweenShooting;
	public float timeBetweenShots;
	public bool shutGun;
	public int bulletInOneShot = 1;
	public WeaponType type;
}

public enum WeaponType
{
	pistol,
	automatic,
	grenadeluncher,
	shotGun
}
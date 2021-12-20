using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon / Shotgun")]
public class ShotgunWeapon : WeaponData
{
	private void Awake()
	{
		shooting = false;
		ammoSpeed = 100f;
		bouncingForce = 10f;
		readyToShoot = true;
		reloading = false;
		maxMagazine = 1000;
		bulletRange = 20;
		timeBetweenShooting = 0.2f;
		shutGun = false;
		timeBetweenShots = 0.06f;
		bulletInOneShot = 8;
		bulletLeft = maxMagazine;
		holdDownShooting = true;
		type = WeaponType.automatic;
	}

	private void OnEnable()
	{
		bulletLeft = maxMagazine;
	}
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon / AutomaticWeapon")]
public class AutomaticWeapon : WeaponData
{
	private void Awake()
	{
		holdDownShooting = true;

		shooting = false;
		bulletSpeed = 100f;
		bouncingForce = 0f;
		readyToShoot = true;
		reloading = false;
		maxMagazine = 1000;
		bulletRange = 100;
		timeBetweenShooting = 0.2f;
		timeBetweenShots = 0.06f;
		shutGun = false;
		bulletInOneShot = 8;
		bulletLeft = maxMagazine;
	}

	private void OnEnable()
	{
		bulletLeft = maxMagazine;
	}
}
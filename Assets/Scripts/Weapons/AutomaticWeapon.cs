using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon / AutomaticWeapon")]
public class AutomaticWeapon : WeaponData
{
	private void Awake()
	{
		holdDownShooting = true;

		shooting = false;
		ammoSpeed = 600;
		bouncingForce = 0f;
		readyToShoot = true;
		reloading = false;
		maxMagazine = 1000;
		bulletRange = 100;
		timeBetweenShooting = 0.1f;
		timeBetweenShots = 1f;
		bulletInOneShot = 8;
		bulletLeft = maxMagazine;
		type = WeaponType.automatic;
	}

	private void OnEnable()
	{
		bulletLeft = maxMagazine;
	}
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon / Grenade Luncher")]
public class GrenadeLuncher : WeaponData
{
	private void Awake()
	{
		ammoSpeed = 100f;
		bouncingForce = 0;
		maxMagazine = 5;
		bulletRange = 20;
		timeBetweenShooting = 0.2f;
		timeBetweenShots = 0.06f;
		bulletInOneShot = 1;
		bulletLeft = maxMagazine;
		type = WeaponType.grenadeluncher;
	}

	private void OnEnable()
	{
		bulletLeft = maxMagazine;
	}
}
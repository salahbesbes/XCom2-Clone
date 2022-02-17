using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon / Shotgun")]
public class ShotgunWeapon : WeaponData
{
	public AutoAmmo ammo;

	private void Awake()
	{
		shooting = false;
		ammoSpeed = 500;
		bouncingForce = 0f;
		readyToShoot = true;
		reloading = false;
		maxMagazine = 1000;
		bulletRange = 50;
		timeBetweenShooting = 0f;
		shutGun = false;
		timeBetweenShots = 1f;
		bulletInOneShot = 8;
		bulletLeft = maxMagazine;
		holdDownShooting = true;
		type = WeaponType.shotGun;
	}

	private void OnEnable()
	{
		bulletLeft = maxMagazine;
	}

	public override void Use()
	{

		ShutGun EquipedWeapon = (ShutGun)prefab;

		AnyClass selectedUnit = GameStateManager.Instance.SelectedUnit;
		Quaternion prevWeaponRotation = selectedUnit.weapon.transform.rotation;
		Destroy(selectedUnit.weapon.gameObject);

		Instantiate(prefab, selectedUnit.weapon.transform.position, prevWeaponRotation, selectedUnit.hand);
		EquipedWeapon.player = selectedUnit;
		selectedUnit.weapon = EquipedWeapon;
	}
}
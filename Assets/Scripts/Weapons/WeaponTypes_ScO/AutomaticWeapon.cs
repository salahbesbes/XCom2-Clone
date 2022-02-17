using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon / AutomaticWeapon")]
public class AutomaticWeapon : WeaponData
{
	public AutoAmmo ammo;

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

	public override void Use()
	{

		Ak47 EquipedWeapon = (Ak47)prefab;

		AnyClass selectedUnit = GameStateManager.Instance.SelectedUnit;
		Quaternion prevWeaponRotation = selectedUnit.weapon.transform.rotation;
		Destroy(selectedUnit.weapon.gameObject);

		Instantiate(prefab, selectedUnit.weapon.transform.position, prevWeaponRotation, selectedUnit.hand);
		EquipedWeapon.player = selectedUnit;
		selectedUnit.weapon = EquipedWeapon;
	}
}
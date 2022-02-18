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
		PlayerStateManager selectedUnit = GameStateManager.Instance.SelectedUnit;
		Quaternion prevWeaponRotation = selectedUnit.weapon.transform.rotation;
		Destroy(selectedUnit.weapon.gameObject);

		Ak47 EquipedWeapon = Instantiate(prefab, selectedUnit.weapon.transform.position, prevWeaponRotation, selectedUnit.hand) as Ak47;
		EquipedWeapon.player = selectedUnit;
		selectedUnit.weapon = EquipedWeapon;
	}
}
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon / Grenade Luncher")]
public class GrenadeLuncher : WeaponData
{
	public Grenade ammo;

	private void Awake()
	{
		ammoSpeed = 100f;
		bouncingForce = 5;
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


	public override void Use()
	{

		HeavyWeapon EquipedWeapon = (HeavyWeapon)prefab;

		AnyClass selectedUnit = GameStateManager.Instance.SelectedUnit;
		Quaternion prevWeaponRotation = selectedUnit.weapon.transform.rotation;
		Destroy(selectedUnit.weapon.gameObject);

		Instantiate(prefab, selectedUnit.weapon.transform.position, prevWeaponRotation, selectedUnit.hand);
		EquipedWeapon.player = selectedUnit.inventory.unit;
		selectedUnit.weapon = EquipedWeapon;
	}
}
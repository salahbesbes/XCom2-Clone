using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Grenade : Ammo
{
	[SerializeField] private Rigidbody _rb;
	public GameObject ExplodeEffect;
	private List<Node> NodeInRange = new List<Node>();
	[Range(0, 5)]
	public int explosionIn;
	[Range(0, 5)]
	public int explotionRange;
	[HideInInspector]
	public UnitStats unitStats;

	public void Init(Vector3 velocity, bool isGhost)
	{
		_rb.velocity = velocity;
	}

	public void OnCollisionEnter(Collision col)
	{
		ExplodeIn(col, explosionIn);
	}


	void HitTargetInrange(int range)
	{
		Node GrenadeNode = NodeGrid.Instance.getNodeFromTransformPosition(transform);
		if (GrenadeNode.coord.y > 0.5f) return;
		int halfRange = range / 2;

		for (int i = GrenadeNode.x - halfRange; i < GrenadeNode.x + halfRange; i++)
		{
			for (int j = GrenadeNode.y - halfRange; j < GrenadeNode.y + halfRange; j++)
			{
				Node neighbour = NodeGrid.Instance.getNode(i, j);
				if (neighbour == null) continue;
				NodeInRange.Add(neighbour);
			}
		}




	}

	private void LateUpdate()
	{
		foreach (Node item in NodeInRange)
		{
			item.tile.obj.GetComponent<Renderer>().material.color = Color.yellow;
			Debug.DrawLine(item.coord, item.coord + Vector3.up, Color.green);
		}
	}


	private async Task ExplodeIn(Collision col, int time)
	{
		_rb.isKinematic = true;

		HitTargetInrange(explotionRange);
		await Task.Delay(1000 * time);
		GameObject obj = Instantiate(ExplodeEffect, transform.position, Quaternion.identity);




		GameStateManager.Instance.clearPreviousSelectedUnitFromAllWeaponEvent(GameStateManager.Instance.SelectedUnit.CurrentTarget);

		foreach (Node item in NodeInRange)
		{
			RaycastHit hit;
			if (Physics.Raycast(item.coord, Vector3.up, out hit, 1))
			{
				if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Enemy" || LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
				{
					AnyClass target = hit.transform.parent.parent.GetComponent<AnyClass>();
					GameStateManager.Instance.MakeOnlySelectedUnitListingToWeaponEvent(target, unitStats.onWeaponFinishShooting);
					unitStats.onWeaponFinishShooting.Raise(unitStats);
					GameStateManager.Instance.clearPreviousSelectedUnitFromAllWeaponEvent(target);


				}
			}
		}
		GameStateManager.Instance.MakeOnlySelectedUnitListingToWeaponEvent(GameStateManager.Instance.SelectedUnit.CurrentTarget, unitStats.onWeaponFinishShooting);

		Destroy(gameObject);
	}
}
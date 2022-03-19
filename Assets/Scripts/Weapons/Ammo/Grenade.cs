using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Grenade : Ammo
{
	[SerializeField] private Rigidbody _rb;
	public GameObject ExplodeEffect;
	private List<Node> NodeInRange = new List<Node>();
	private GrenadierStats grenadier;

	[Range(0, 5)]
	public int explosionIn;

	[Range(0, 5)]
	public int explotionRange;

	[HideInInspector]
	public void Init(Vector3 velocity, bool isGhost)
	{
		_rb.velocity = velocity;
	}

	private void Start()
	{
		grenadier = GameStateManager.Instance.SelectedUnit.GetComponent<GrenadierStats>();
	}

	public async void OnCollisionEnter(Collision col)
	{
		await ExplodeIn(col, explosionIn);
	}

	private void HitTargetInrange(int range)
	{
		Node GrenadeNode = NodeGrid.Instance.getNodeFromTransformPosition(transform);
		if (GrenadeNode.coord.y > 0.5f) return;
		int halfRange = range / 2;
		for (int i = GrenadeNode.x - halfRange; i <= GrenadeNode.x + halfRange; i++)
		{
			// if lalfRange is even we ignore the last row
			if (halfRange % 2 == 0 && i == GrenadeNode.x + halfRange)
				continue;
			for (int j = GrenadeNode.y - halfRange; j <= GrenadeNode.y + halfRange; j++)
			{
				// if lalfRange is even we ignore the last col
				if (halfRange % 2 == 0 && j == GrenadeNode.y + halfRange)
					continue;
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
		Instantiate(ExplodeEffect, transform.position, Quaternion.identity);

		foreach (Node item in NodeInRange)
		{
			RaycastHit hit;
			if (Physics.Raycast(item.coord, Vector3.up, out hit, 1))
			{
				if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Enemy" || LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
				{
					AnyClass target = hit.transform.parent.parent.GetComponent<AnyClass>();
					Debug.Log($"  detect  {target} ");
					GameStateManager.Instance.MakeOnlySelectedUnitListingGrenadeExplosionEvent(target, grenadier.grenadeLancherEvent);
					grenadier.grenadeLancherEvent.Raise(grenadier.unit);
					GameStateManager.Instance.clearPreviousSelectedUnitFromAllGrenadeExplosionEvent(target);
				}
			}
		}
		Destroy(gameObject);
	}
}
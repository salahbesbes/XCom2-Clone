using System.Threading.Tasks;
using UnityEngine;

public class ModalTriggerer : MonoBehaviour
{
	public async Task OnTriggerEnter(Collider other)
	{
		AnyClass thisUnit = transform.parent.parent.GetComponent<AnyClass>();
		if (LayerMask.LayerToName(other.gameObject.layer) == "LowObstacle")
		{
			thisUnit.PlayAnimation(AnimationType.jump);
			thisUnit.speed = 1;
			await Task.Delay(500);

			thisUnit.PlayAnimation(AnimationType.run);
			thisUnit.speed = 5;
		}
		if (LayerMask.LayerToName(other.gameObject.layer) == "Pickable")
		{
			Equipement obj = other.GetComponent<Equipement>();

			if (obj == null) return;

			obj.picked(thisUnit);
		}
	}

	private void Start()
	{
		AnyClass thisUnit = transform.parent.parent.GetComponent<AnyClass>();

		SkinnedMeshRenderer[] meshRendrers = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (var mesh in meshRendrers)
		{
			foreach (Material material in mesh.materials)
			{
				thisUnit.modelMaterials.Add(material);
			}
		}
	}
}
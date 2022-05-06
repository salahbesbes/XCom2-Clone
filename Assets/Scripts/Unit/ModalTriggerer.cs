using System.Threading.Tasks;
using UnityEngine;

public class ModalTriggerer : MonoBehaviour
{
	public async Task OnTriggerEnter(Collider other)
	{
		Unit thisUnit = transform.parent.parent.GetComponent<Unit>();
		if (other.CompareTag("JumpObject"))
		{
			thisUnit.PlayAnimation(AnimationType.jump);
			thisUnit.speed = 1;
			await Task.Delay(500);

			thisUnit.PlayAnimation(AnimationType.run);
			thisUnit.speed = 5;
		}
		if (other.CompareTag("Pickable"))
		{
			Equipement obj = other.GetComponent<Equipement>();

			if (obj == null) return;

			obj.picked(thisUnit);
		}
	}

	private void Start()
	{
		Unit thisUnit = transform.parent.parent.GetComponent<Unit>();

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
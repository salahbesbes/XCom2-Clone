using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
	public Node node;
	public Cover leftCover;
	public Cover rightCover;
	public Cover forwardCover;
	public Cover backCover;
	public List<Cover> listOfActiveCover;
	public bool mouseOnTile = false;
	private Transform parent;
	[HideInInspector]
	public float size = 1;
	public float scale = 1;
	[HideInInspector]
	public float offset = 2f;
	public Collider colliderOnTop;

	public GameObject getPrefabOnTopOfTheNode(Node node)
	{
		RaycastHit hit;


		if (Physics.Raycast(node.coord, Vector3.up, out hit))
		{

			//Debug.Log($"{hit.collider.name}");
			hit.collider.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.blue);
		}
		return hit.transform.gameObject;
	}

	private void Awake()
	{
		parent = transform;
		size = NodeGrid.Instance.nodeSize;
		scale = NodeGrid.Instance.scale;
		transform.localScale = new Vector3(size, size, size);
		listOfActiveCover = new List<Cover>();

	}
	public Collider getPrefabOnTopOfTheTile()
	{

		string[] collidableLayers = { "Unwalkable", "Unit" };
		int layerToCheck = LayerMask.GetMask(collidableLayers);
		float GroundTileheight = node.groundTile.transform.position.y;
		Collider[] objs = Physics.OverlapSphere(node.coord + Vector3.up * GroundTileheight, NodeGrid.Instance.nodeSize / 2, layerToCheck);
		if (objs.Length != 0)
		{

			Collider unit = objs.FirstOrDefault(el => el.CompareTag("Unit"));
			if (unit) return unit;
			else
			{
				Collider highObstacle = objs.FirstOrDefault(el => el.CompareTag("HighObstacle"));
				if (highObstacle) return highObstacle;
				else
				{
					return objs[0].transform.GetComponent<Collider>();
				}
			}


			//for (int i = 0; i < objs.Length; i++)
			//{

			//}
			//// TODO: if 2 GameObject share same tile this line can cause bugs
		}

		return null;
	}
	public void createRightCover()
	{
		Vector3 origin = node.coord + Vector3.up * (size / 2) * scale;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.right * size;
		quad.transform.rotation = Quaternion.Euler(0, 90, 0);
		quad.transform.localScale = new Vector3(size, size, 1) * scale;
		quad.transform.SetParent(parent);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.red;
		rightCover = new Cover(quad);
		listOfActiveCover.Add(rightCover);
		//Debug.Log($"create right Cover");
	}

	public void createLeftCover()
	{
		Vector3 origin = node.coord + Vector3.up * (size / 2) * scale;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.left * size;
		quad.transform.rotation = Quaternion.Euler(0, -90, 0);
		quad.transform.localScale = new Vector3(size, size, 1) * scale;
		quad.transform.SetParent(parent);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.green;
		leftCover = new Cover(quad);
		listOfActiveCover.Add(leftCover);
		//Debug.Log($"create left Cover");
	}

	public void createForwardCover()
	{
		Vector3 origin = node.coord + Vector3.up * (size / 2) * scale;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.forward * size;
		quad.transform.rotation = Quaternion.Euler(0, 0, 0);
		quad.transform.localScale = new Vector3(size, size, 1) * scale;
		quad.transform.SetParent(parent);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.gray;
		forwardCover = new Cover(quad);
		listOfActiveCover.Add(forwardCover);
		//Debug.Log($"create forward Cover");
	}

	public void createBackCover()
	{
		Vector3 origin = node.coord + Vector3.up * (size / 2) * scale;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.back * size;
		quad.transform.rotation = Quaternion.Euler(180, 0, 0);
		quad.transform.localScale = new Vector3(size, size, 1) * scale;
		quad.transform.SetParent(parent);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.yellow;
		backCover = new Cover(quad);
		listOfActiveCover.Add(backCover);
		//Debug.Log($"create Back Cover");
	}

	public void hightLight(Color newColor)
	{
		GetComponent<Renderer>().material.color = newColor;
	}

	public void destroyAllActiveCover()
	{
		if (listOfActiveCover.Count == 0) return;
		foreach (Cover cover in listOfActiveCover)
		{
			GameObject.Destroy(cover.coverObj);
		}
		//Debug.Log($"destroy");
		listOfActiveCover.Clear();
	}
}

public class Cover
{
	private float _value = 0;
	public GameObject coverObj;

	public float Value
	{ get { return _value; } set { _value = value; } }

	public Cover(GameObject cover, float coverValue = 45f)
	{
		coverObj = cover;
		Value = coverValue;
	}

	public override string ToString()
	{
		return $"cover exist";
	}
}




public enum Direction
{
	right,
	left,
	top,
	buttom,
	topLeft,
	topright,
	front,
	flanked
}
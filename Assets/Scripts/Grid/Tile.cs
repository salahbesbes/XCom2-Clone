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
	public bool mouseAlreadyOnTile = false;
	private Transform parent;
	[HideInInspector]
	public float size = 1;
	public float scale = 1;
	[HideInInspector]
	public float offset = 2f;
	public Collider colliderOnTop;
	public Material defaultMaterial;
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
		transform.localScale = new Vector3(size, 0.1f, size);
		listOfActiveCover = new List<Cover>();
		defaultMaterial = NodeGrid.Instance.tile_mat;


	}
	public Collider getPrefabOnTopOfTheTile()
	{

		string[] collidableLayers = { "Environment", "Charachter" };

		int layerToCheck = LayerMask.GetMask(collidableLayers);
		float GroundTileheight = node.tile.transform.position.y;
		Collider[] objs = Physics.OverlapSphere(node.coord + Vector3.up * GroundTileheight, NodeGrid.Instance.nodeSize / 2, layerToCheck);

		node.isUnwalkable = false;
		node.isObstacle = false;
		if (objs.Length == 0) return null;

		Collider unit = objs.FirstOrDefault(el => el.CompareTag("Unit"));
		if (unit)
		{
			node.isUnwalkable = true;
			node.isObstacle = false;
			return unit;
		}
		else
		{
			Collider highObstacle = objs.FirstOrDefault(el => el.CompareTag("HighObstacle"));
			if (highObstacle)
			{
				node.isUnwalkable = true;
				node.isObstacle = true;
				return highObstacle;
			}
			else
			{
				Collider lowObstacle = objs.FirstOrDefault(el => el.CompareTag("LowObstacle"));
				if (lowObstacle)
				{
					node.isUnwalkable = true;
					node.isObstacle = true;
					return lowObstacle;
				}
				else
				{
					Collider jumpObj = objs.FirstOrDefault(el => el.CompareTag("JumpObject"));
					if (jumpObj)
					{

						node.isUnwalkable = false;
						node.isObstacle = false;
						return jumpObj;
					}
					else
					{
						Collider obj = objs.FirstOrDefault();

						node.isUnwalkable = false;
						node.isObstacle = false;
						return obj;
					}
				}

			}
		}
	}
	public void createRightCover(CoverType typeOfCover)
	{
		rightCover = new Cover(CoverDirection.right, typeOfCover, node);
		rightCover.showSprite(size, scale);
		listOfActiveCover.Add(rightCover);
		//Debug.Log($"create right Cover");
	}

	public void createLeftCover(CoverType typeOfCover)
	{
		leftCover = new Cover(CoverDirection.left, typeOfCover, node);
		leftCover.showSprite(size, scale);
		listOfActiveCover.Add(leftCover);
		//Debug.Log($"create left Cover");
	}

	public void createForwardCover(CoverType typeOfCover)
	{
		forwardCover = new Cover(CoverDirection.front, typeOfCover, node);
		forwardCover.showSprite(size, scale);

		listOfActiveCover.Add(forwardCover);
		//Debug.Log($"create forward Cover");
	}

	public void createBackCover(CoverType typeOfCover)
	{
		backCover = new Cover(CoverDirection.back, typeOfCover, node);

		//Vector3 origin = node.coord + Vector3.up * (size / 2) * scale;
		//quad.transform.position = origin + Vector3.back * size;
		//quad.transform.rotation = Quaternion.Euler(180, 0, 0);
		//quad.transform.localScale = new Vector3(size, size, 1) * scale;
		//quad.transform.SetParent(parent);
		backCover.showSprite(size, scale);

		listOfActiveCover.Add(backCover);
		//Debug.Log($"create Back Cover");
	}

	public void hightLight(Color newColor)
	{
		if (defaultMaterial != NodeGrid.Instance.tile_mat)
		{
			hightLight();
			return;
		}
		GetComponent<Renderer>().material.color = newColor;
	}
	public void hightLight(Material newMaterial = null)
	{
		if (newMaterial != null)
		{
			GetComponent<Renderer>().material = newMaterial;
			return;
		}
		else if (defaultMaterial == NodeGrid.Instance.tile_mat)
		{
			return;
		}
		else
		{
			GetComponent<Renderer>().material = defaultMaterial == NodeGrid.Instance.larva_mat ? NodeGrid.Instance.highlightedLarva_mat : NodeGrid.Instance.highlightedAcid_mat;
		}
	}
	public void resetTextureAndColor()
	{
		if (colliderOnTop == null)
		{
			GetComponent<MeshRenderer>().material.color = node.color;
		}
		else
		{
			GetComponent<Renderer>().material = defaultMaterial;
		}
	}

	public void destroyAllActiveCover()
	{
		if (listOfActiveCover.Count == 0) return;
		foreach (Cover cover in listOfActiveCover)
		{
			GameObject.Destroy(cover.prefab);
		}
		//Debug.Log($"destroy");
		listOfActiveCover.Clear();
	}
}


//public class NewCover
//{
//	private float _value = 0;
//	public GameObject coverObj;

//	public float Value
//	{ get { return _value; } set { _value = value; } }

//	public NewCover(GameObject cover, float coverValue = 45f)
//	{
//		coverObj = cover;
//		Value = coverValue;
//	}

//	public override string ToString()
//	{
//		return $"cover exist";
//	}
//}




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
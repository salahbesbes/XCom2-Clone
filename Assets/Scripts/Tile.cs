using System.Collections.Generic;
using UnityEngine;

public class Tile
{
	public Transform obj;
	public Node node;
	public Cover leftCover;
	public Cover rightCover;
	public Cover forwardCover;
	public Cover backCover;
	public List<Cover> listOfActiveCover;
	public bool mouseOnTile = false;
	private Transform parent;
	private float size = 1;

	public float offset = 2f;

	public Tile(Node node, Transform parent, List<Tile> listTiles)
	{
		// create Quad
		size = NodeGrid.Instance.nodeSize / 2;
		this.parent = parent;
		this.node = node;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		obj = quad.transform;
		quad.transform.position = node.coord;
		quad.transform.rotation = Quaternion.Euler(90, 0, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, size, 1);
		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		listTiles.Add(this);
		node.tile = this;
		listOfActiveCover = new List<Cover>();
	}

	public void createRightCover()
	{
		Vector3 origin = obj.transform.position + Vector3.up * size;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.right * size;
		quad.transform.rotation = Quaternion.Euler(0, 90, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, 2 * size, 1);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.red;
		rightCover = new Cover(quad);
		listOfActiveCover.Add(rightCover);
		Debug.Log($"create right Cover");
	}

	public void createLeftCover()
	{
		Vector3 origin = obj.transform.position + Vector3.up * size;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.left * size;
		quad.transform.rotation = Quaternion.Euler(0, -90, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, 2 * size, 1);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.green;
		leftCover = new Cover(quad);
		listOfActiveCover.Add(leftCover);
		Debug.Log($"create left Cover");
	}

	public void createForwardCover()
	{
		Vector3 origin = obj.transform.position + Vector3.up * size;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.forward * size;
		quad.transform.rotation = Quaternion.Euler(0, 0, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, 2 * size, 1);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.gray;
		forwardCover = new Cover(quad);
		listOfActiveCover.Add(forwardCover);
		Debug.Log($"create forward Cover");
	}

	public void createBackCover()
	{
		Vector3 origin = obj.transform.position + Vector3.up * size;
		GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
		quad.transform.position = origin + Vector3.back * size / 2;
		quad.transform.rotation = Quaternion.Euler(180, 0, 0);
		quad.transform.SetParent(parent);
		quad.transform.localScale = new Vector3(size, 2 * size, 1);

		quad.GetComponent<Renderer>().material = (Material)Resources.Load("tile", typeof(Material));
		quad.GetComponent<Renderer>().material.color = Color.yellow;
		backCover = new Cover(quad);
		listOfActiveCover.Add(backCover);
		Debug.Log($"create Back Cover");
	}

	public void destroyAllActiveCover()
	{
		if (listOfActiveCover.Count == 0) return;
		foreach (Cover cover in listOfActiveCover)
		{
			GameObject.Destroy(cover.coverObj);
		}
		Debug.Log($"destroy");
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
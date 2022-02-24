using ExtensionMethods;
using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class CostAttribute : Attribute
{
	public int cost { get; }

	public CostAttribute(int cost)
	{
		this.cost = cost;
	}
}

public enum CoverType
{
	[Cost(cost: 10)]
	low,

	[Cost(cost: 20)]
	high,
}

public enum CoverDirection
{
	[Cost(cost: 10)]
	front,

	[Cost(cost: 5)]
	left,

	[Cost(cost: 5)]
	right,
}

public enum FlunckDirection
{
	front,
	left,
	right,
	None
}

internal class NewCover
{
	public CoverDirection direction;
	public CoverType type;
	public Collider obj;
	public int Value;
	public Node node;

	public NewCover(CoverDirection direction, CoverType type, Node node)
	{
		this.direction = direction;
		this.type = type;
		obj = node.tile.colliderOnTop;
		this.node = node;
		// sum of the direction cost + the type cost
		Value = direction.getCoverValue<CoverDirection>() + type.getCoverValue<CoverType>();
	}

	public override string ToString()
	{
		return $"Cover({direction}, {type}) => Value = {Value} at {node}";
	}
}

internal class MovingTest : Mono
{
	private CoverType coverType = CoverType.low;
	private CoverDirection coverdirection = CoverDirection.front;

	private void Start()
	{
		//Debug.Log($" cover Type is {coverType} [{coverType.getCoverValue<CoverType>()}]");
		//Debug.Log($" cover Direction is {coverdirection} [{coverdirection.getCoverValue<CoverDirection>()}]");
	}
}
public class Enemy : PlayerStateManager
{


	//private bool checkPointIfSameLineOrColumAsTarget(Vector3 target, Vector3 pointNode)
	//{
	//	if (pointNode != null)
	//	{
	//		if (target.x == pointNode.x || target.z == pointNode.z)
	//		{
	//			return true;
	//		}
	//	}
	//	return false;
	//}

	//public void checkFlank(Node target)
	//{
	//	if (currentPos == null) return;

	//	Transform points = transform.Find("Points");
	//	Vector3 selectedPointCood = Vector3.zero;
	//	Vector3 selectedPoint;
	//	if (target != null && points != null)
	//	{
	//		Dictionary<Vector3, float> ordredDictByMagnitude = new Dictionary<Vector3, float>();

	//		for (int i = 0; i < points.childCount; i++)
	//		{
	//			Transform point = points.GetChild(i);
	//			float mag = (target.coord - point.position).magnitude;
	//			ordredDictByMagnitude.Add(point.position, mag);
	//		}

	//		ordredDictByMagnitude = ordredDictByMagnitude.OrderBy((item) => item.Value)
	//								.ToDictionary(t => t.Key, t => t.Value);

	//		// default node is the nearest one to the target (first one in the dict)
	//		Vector3 defaultPoint = ordredDictByMagnitude.First().Key;
	//		selectedPoint = defaultPoint;

	//		bool foundPotentialPositionToFlank = false;
	//		foreach (var item in ordredDictByMagnitude)
	//		{
	//			Vector3 point = item.Key;
	//			if (checkPointIfSameLineOrColumAsTarget(target.coord, point))
	//			{
	//				foundPotentialPositionToFlank = true;
	//				// update selected Point Coord
	//				selectedPoint = point;
	//				break;
	//			}
	//		}
	//		if (foundPotentialPositionToFlank)
	//		{
	//			if (defaultPoint == selectedPoint)
	//			{
	//				selectedPoint = ordredDictByMagnitude.ElementAt(1).Key;
	//			}
	//			CheckForTargetWithRayCast(selectedPoint, target.coord);
	//		}
	//		else
	//		{
	//			CheckForTargetWithRayCast(defaultPoint, target.coord);
	//		}
	//	}
	//}






}

//public class Charachter : MonoBehaviour
//{
//}
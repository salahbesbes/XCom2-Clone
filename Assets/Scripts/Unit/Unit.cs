using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Unit : MonoBehaviour
{
	//public ActionType[] actions;
	private float _rotateBy = 0;

	public float rotateBy
	{
		get => _rotateBy; set
		{
			_rotateBy = value;
		}
	}

	protected List<Node> path;

	public Queue<ActionBase> queueOfActions;

	protected Vector3[] turnPoints;

	[HideInInspector]
	protected NodeGrid grid;

	[SerializeField]
	public Node currentPos;

	protected AnyClass _currentTarger;

	//public AnyClass _currentTarger
	//{
	//	get => _currentTarger;
	//	set
	//	{
	//		_currentTarger = value;

	//	}
	//}

	public Node destination;
	public bool processing = false;

	//public Weapon weapon;
	public Transform partToRotate;
	public Transform model;
	protected Animator animator;
	public float speed = 5f;

	public void MoveActionCallback(MoveAction actionInstance, Node start, Node end)
	{
		PlayAnimation(AnimationType.run);
		move(actionInstance, turnPoints);
	}

	//private void Start()
	//{
	//	grid = NodeGrid.Instance;
	//}

	public void PlayAnimation(AnimationType anim)
	{
		animator = model.GetComponent<Animator>();
		foreach (AnimatorControllerParameter item in animator.parameters)
		{
			if (item.type is AnimatorControllerParameterType.Bool)
			{
				animator.SetBool(item.name, false);
			}
		}
		PlayerStateManager thisPlayer = (PlayerStateManager)this;
		thisPlayer.currentActionAnimation = anim;
		string CorrespondNameOfTheAnimation = Enum.GetName(typeof(AnimationType), anim);
		animator.SetBool(CorrespondNameOfTheAnimation, true);
	}

	public void PlayIdelAnimation()
	{
		PlayAnimation(AnimationType.idel);
	}

	public async Task originalRotation(Transform partToRotate, Vector3 dir, float timeToSpentTurning = 2)
	{
		float speed = 3;
		float timeElapsed = 0, lerpDuration = timeToSpentTurning;

		if (partToRotate == null) return;
		Quaternion startRotation = partToRotate.rotation;

		//Quaternion targetRotation = player.transform.rotation * Quaternion.Euler(dir);
		Quaternion targetRotation = Quaternion.LookRotation(dir);

		while (timeElapsed < lerpDuration)
		{
			Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
				    targetRotation,
				     timeElapsed / lerpDuration
				    )
				    .eulerAngles;
			//partToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
			timeElapsed += (speed * Time.deltaTime);
			//Debug.Log($"rotating");
			await Task.Yield();
			partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
			//partToRotate.Rotate(Vector3.up, rotation.y);
		}
		rotateBy = Quaternion.Angle(startRotation, this.partToRotate.rotation);
	}

	public async Task rotateTowardDirection(Transform partToRotate, Vector3 dir, float timeToSpentTurning = 2)
	{
		float speed = 3;
		float timeElapsed = 0, lerpDuration = timeToSpentTurning;

		if (partToRotate == null) return;

		try
		{
			Quaternion startRotation = partToRotate.rotation;

			Quaternion targetRotation = Quaternion.LookRotation(dir);

			while (timeElapsed < lerpDuration)
			{
				Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
					    targetRotation,
					     timeElapsed / lerpDuration
					    )
					    .eulerAngles;
				//partToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / lerpDuration);
				timeElapsed += (speed * Time.deltaTime);
				await Task.Yield();
				partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
			}
		}
		catch (Exception ex)
		{
			Debug.Log($"exception {ex.Message}");
		}
	}

	public void turnTheModel(Vector3 dir)
	{
		// handle rotation on axe Y
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		// smooth the rotation of the turrent
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
				lookRotation,
				Time.deltaTime * 2
				)
				.eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	public async Task move(MoveAction moveInstance, Vector3[] turnPoints)
	{
		if (turnPoints.Length > 0)
		{
			for (int i = 0; i < turnPoints.Length; i++)
			{
				turnPoints[i].y = transform.position.y;
			}
			//grid.path = path;
			//grid.turnPoints = turnPoints;
			Vector3 currentPoint = turnPoints[0];
			int index = 0;
			// this while loop simulate the update methode
			while (true)
			{
				if (transform.position == currentPoint)
				{
					index++;
					if (index >= turnPoints.Length)
					{
						//PathRequestManager.Instance.finishedProcessingPath();

						break;
					}
					// im moving the model not the part to rotate
					rotateTowardDirection(model, destination.coord - partToRotate.transform.position, 0.5f);
					currentPoint = turnPoints[index];
				}

				transform.position = Vector3.MoveTowards(transform.position, currentPoint, speed * Time.deltaTime);

				// this yield return null waits until the next frame reached ( dont
				// exit the methode )
				await Task.Yield();
			}
		}

		//Debug.Log($"finish moving");
		FinishAction(moveInstance);
		//onActionFinish();
		await Task.Yield();
	}

	public void LockOnTarger()
	{
		if (currentPos == null || destination == null) return;

		if (_currentTarger == null || currentPos.coord != destination.coord)
		{// handle rotation on axe Y
			Vector3 dir = destination.coord - currentPos.coord;
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			// smooth the rotation of the turrent
			Vector3 rotation = Quaternion.Lerp(partToRotate.rotation,
					lookRotation,
					Time.deltaTime * 5f)
					.eulerAngles;
			partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
			return;
		}
		if (destination == null || (currentPos.coord == destination.coord))
		{
			Vector3 dir = _currentTarger.aimPoint.position - transform.position;
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * 5f).eulerAngles;
			partToRotate.rotation = Quaternion.Euler(0, rotation.y, 0);

			return;
		}
	}

	public async void FinishAction(ActionBase action)
	{
		//todo: reset the grid

		PlayIdelAnimation();
		PlayerStateManager player = (PlayerStateManager)this;
		// triget event

		switch (action)
		{
			case ShootAction:
				player.stats.onWeaponFinishShooting.Raise(player.stats.unit);
				break;

			case LunchGrenadeAction:
				Debug.Log($"  Grenade Lunched ..!!!!!!! explosion in 1 Sec");
				//player.stats.onWeaponFinishShooting.Raise(player.stats.unit);
				break;

			case MoveAction:
				//rotateTowardDirection(_currentTarger.partToRotate, transform.position - _currentTarger.aimPoint.position);
				Vector3 ori = new Vector3(_currentTarger.partToRotate.transform.position.x, 0.5f, _currentTarger.partToRotate.transform.position.z);

				RaycastHit hit;
				if (Physics.Raycast(ori, _currentTarger.partToRotate.forward * 2, out hit, Vector3.forward.magnitude * 2))
				{
					//Debug.Log($"target have some obstacle => {hit.collider.name}");
				}
				break;

			default:
				Debug.Log($" action  {action} NOT FOUND");
				break;
		}
		player.SwitchState(player.idelState);

		await rotateTowardDirection(partToRotate, _currentTarger.aimPoint.position - partToRotate.position, 1f);
		// when moving i rotating the model not the part to rotate, so when reach
		// destination i have to rotate the model last time
		await rotateTowardDirection(model, _currentTarger.aimPoint.position - partToRotate.position, 0.5f);
		player.testDelay();
		processing = false;

		// update the cost
		//GetComponent<PlayerStats>().ActionPoint -= action.cost;

		ExecuteActionInQueue();
	}

	public void updateNeighbourCover()
	{
		Transform points = partToRotate.Find("points");
		Utils utils = points.GetComponent<Utils>();
		Node front;

		//Vector3 backCoord = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
		front = NodeGrid.Instance.getNodeFromTransformPosition(null, points.transform.localPosition);
		front.tile.hightLight(Color.yellow);

		//utils.updateCoversNode();

		//float[] floatvalues = new float[4];
		//Dictionary<float, Transform> dict = new Dictionary<float, Transform>();
		//for (int i = 0; i < 4; i++)
		//{
		//	float distance = Vector3.Distance(points.GetChild(i).position, _currentTarger.transform.position);
		//	floatvalues[i] = distance;
		//	dict.Add(distance, points.GetChild(i));
		//}

		//float minDist = Mathf.Min(floatvalues);
		//front = NodeGrid.Instance.getNodeFromTransformPosition(dict[minDist]);
		//front.tile.obj.GetComponent<Renderer>().material.color = Color.yellow;

		//Vector3 backCoord = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
		//back = NodeGrid.Instance.getNodeFromTransformPosition(null, backCoord);
		//back.tile.obj.GetComponent<Renderer>().material.color = Color.white;

		//Vector3 rightCoord = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
		//right = NodeGrid.Instance.getNodeFromTransformPosition(null, rightCoord);
		//right.tile.obj.GetComponent<Renderer>().material.color = Color.gray;

		//Vector3 leftCoord = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
		//left = NodeGrid.Instance.getNodeFromTransformPosition(null, leftCoord);
		//left.tile.obj.GetComponent<Renderer>().material.color = Color.red;
		//Debug.Log($"front {front} back {back}");
	}

	public void Enqueue(ActionBase action)
	{
		queueOfActions.Enqueue(action);
		ExecuteActionInQueue();
	}

	public void ExecuteActionInQueue()
	{
		if (processing == false && queueOfActions.Count > 0)
		{
			processing = true;
			ActionBase action = queueOfActions.Dequeue();
			action.TryExecuteAction();
		}
	}

	public override string ToString()
	{
		return $" {transform.name}";
	}
}
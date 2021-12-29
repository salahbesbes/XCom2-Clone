using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Unit : MonoBehaviour
{
	//public ActionType[] actions;
	protected List<Node> path;

	public Queue<ActionBase> queueOfActions;

	protected Vector3[] turnPoints;

	[HideInInspector]
	public NodeGrid grid;

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
	public Weapon weapon;
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
		foreach (AnimatorControllerParameter item in animator.parameters)
		{
			if (item.type is AnimatorControllerParameterType.Bool)
			{
				animator.SetBool(item.name, false);
			}
		}
		string CorrespondNameOfTheAnimation = Enum.GetName(typeof(AnimationType), anim);

		animator.SetBool(CorrespondNameOfTheAnimation, true);
	}

	public void PlayIdelAnimation()
	{
		foreach (AnimatorControllerParameter item in animator.parameters)
		{
			if (item.type is AnimatorControllerParameterType.Bool)
			{
				animator.SetBool(item.name, false);
			}
		}
		string CorrespondNameOfTheAnimation = Enum.GetName(typeof(AnimationType), AnimationType.idel);
		animator.SetBool(CorrespondNameOfTheAnimation, true);
	}

	public async Task rotateTowardDirection(Transform partToRotate, Vector3 dir, float timeToSpentTurning = 2)
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

		// smooth the rotation of the turrent

		//partToRotate.rotation = targetRotation;
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

	public void FinishAction(ActionBase action)
	{
		//todo: reset the grid

		PlayIdelAnimation();
		PlayerStateManager player = (PlayerStateManager)this;
		// triget event
		if (action is ShootAction)
		{
			UnitStats stats = GetComponent<Stats>().unit;
			stats.onWeaponFinishShooting.Raise(stats);
		}
		if (action is MoveAction)
		{
			rotateTowardDirection(_currentTarger.partToRotate, transform.position - _currentTarger.aimPoint.position);
			Vector3 ori = new Vector3(_currentTarger.partToRotate.transform.position.x, 0.5f, _currentTarger.partToRotate.transform.position.z);

			RaycastHit hit;
			if (Physics.Raycast(ori, _currentTarger.partToRotate.forward * 2, out hit, Vector3.forward.magnitude * 2))
			{
				//Debug.Log($"target have some obstacle => {hit.collider.name}");
			}
		}
		// switch state
		player.SwitchState(player.idelState);
		Debug.Log($"{player.name} current state : {player.State.name}");

		rotateTowardDirection(partToRotate, _currentTarger.aimPoint.position - partToRotate.position);
		rotateTowardDirection(model, _currentTarger.aimPoint.position - partToRotate.position);
		processing = false;
		// update the cost
		//GetComponent<PlayerStats>().ActionPoint -= action.cost;

		ExecuteActionInQueue();
	}

	public void ReloadActionCallBack(ReloadAction reload)
	{
		weapon.Reload(reload);
	}

	public void ShootActionCallBack(ShootAction soot)
	{
		weapon.startShooting(soot);
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
		return $" {GetType().Name} {transform.name} selected";
	}
}
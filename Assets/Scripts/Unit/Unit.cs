using System;
using System.Collections;
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
	public AnyClass currentTarget;
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
		StartCoroutine(move(actionInstance, turnPoints));
	}

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

	public async void rotateTowardDirection(Transform partToRotate, Vector3 dir, float timeToSpentTurning = 2)
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
			Debug.Log($"rotating");
			await Task.Yield();
			partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
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

	public IEnumerator move(MoveAction moveInstance, Vector3[] turnPoints)
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
				yield return null;
			}
		}

		//Debug.Log($"finish moving");
		FinishAction(moveInstance);
		//onActionFinish();
		yield return null;
	}

	public void LockOnTarger()
	{
		if (currentPos == null || destination == null) return;

		if (currentTarget == null || currentPos.coord != destination.coord)
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
			Vector3 dir = currentTarget.aimPoint.position - transform.position;
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
		Player player = (Player)this;
		// triget event
		if (action is ShootAction)
		{
			UnitStats stats = GetComponent<SalahStatsTest>().unit;
			stats.eventToListnTo.Raise(stats);
		}
		// switch state
		player.SwitchState(player.idelState);
		rotateTowardDirection(partToRotate, currentTarget.aimPoint.position - partToRotate.position);
		processing = false;
		// update the cost
		//GetComponent<PlayerStats>().ActionPoint -= action.cost;

		ExecuteActionInQueue();
	}

	public void ReloadActionCallBack(ReloadAction reload)
	{
		StartCoroutine(weapon.Reload(reload));
	}

	public void ShootActionCallBack(ShootAction soot)
	{
		//PlayAnimation(AnimationType.jump);
		StartCoroutine(weapon.startShooting(soot));
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
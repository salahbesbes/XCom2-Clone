using System;
using System.Collections;
using System.Collections.Generic;
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

	private void turnTheModel(Vector3 dir)
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
				turnPoints[i].y = 0.5f;
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

					//partToRotate.RotateAround(partToRotate.position, Vector3.up ,  )
					currentPoint = turnPoints[index];
				}

				turnTheModel(currentPoint - partToRotate.position);

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

	public void FinishAction(ActionBase action)
	{
		//todo: reset the grid

		PlayIdelAnimation();
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
		PlayAnimation(AnimationType.jump);
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
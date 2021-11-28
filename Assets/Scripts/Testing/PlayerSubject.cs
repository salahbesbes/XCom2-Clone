using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerSubject : MonoBehaviour
{
	[SerializeField]
	public Dictionary<string, int> testdict = new Dictionary<string, int>();

	public List<ActionData> actions = new List<ActionData>();
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			ActionData move = actions.FirstOrDefault((el) => el is MovementAction);

			move?.Actionevent?.Raise();
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			ActionData shoot = actions.FirstOrDefault((el) => el is ShootingAction);

			shoot?.Actionevent?.Raise();
		}
	}

	public void startShooting()
	{
		animator.runtimeAnimatorController = actions[0].AnimationController;
		StartCoroutine(Shoot());
	}

	public void startMoving()
	{
		animator.runtimeAnimatorController = actions[1].AnimationController;

		StartCoroutine(Move());
	}

	public IEnumerator Shoot()
	{
		Debug.Log($"start shooting");
		yield return new WaitForSeconds(2f);
		Debug.Log($"finish shooting");
		animator.runtimeAnimatorController = null;
	}

	public IEnumerator Move()
	{
		Debug.Log($"start Moving");
		yield return new WaitForSeconds(2f);
		Debug.Log($"finish Moving");
		animator.runtimeAnimatorController = null;
	}
}
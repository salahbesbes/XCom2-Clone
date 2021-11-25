using UnityEngine;

public class basicAnimation : MonoBehaviour
{
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetBool("idel", false);
			animator.SetBool("jump", true);
			animator.SetBool("run", true);
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			animator.SetBool("idel", false);
			animator.SetBool("jump", false);
			animator.SetBool("run", true);
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			animator.SetBool("idel", true);
			animator.SetBool("jump", false);
			animator.SetBool("run", false);
		}
	}
}
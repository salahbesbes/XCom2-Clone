using UnityEngine;

public class Dead : BaseState<PlayerStateManager>
{
	public override void EnterState(PlayerStateManager player)
	{
		//player.model.transform.GetComponent<CapsuleCollider>().height = 0.5f;
		//Debug.Log($" {player} IS DEAD");

		//string CorrespondNameOfTheAnimation = Enum.GetName(typeof(AnimationType), player.currentActionAnimation);

		//player.model.GetComponent<Animator>().SetBool(CorrespondNameOfTheAnimation, false);
		//player.model.GetComponent<Animator>().SetBool("die", true);
		player.PlayAnimation(AnimationType.die);
		player.model.GetComponent<CapsuleCollider>().height = 0.05f;
		player.stopGlowing();
		player.HealthBar.gameObject.SetActive(false);
		player.notifyGameManagerEvent.Raise(player);
		player.IsDead();
		GameStateManager.Instance.SelectedUnit.SelectNextTarget();

	}

	public override void ExitState(PlayerStateManager player)
	{
	}

	public override void Update(PlayerStateManager player)
	{
		//if (Input.anyKeyDown)
		//{
		//	player.SwitchState(player.idelState);
		//}
	}
}
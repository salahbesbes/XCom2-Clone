using gameEventNameSpace;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameManagerListner : MonoBehaviour
{
	public VoidEvent GameEndedEvent;
	public StringEvent notifyCanvas;

	private void clearGameManagerFromPreviousSelectedUnit()
	{
		PlayerEventListener[] listners = gameObject.GetComponents<PlayerEventListener>();
		foreach (PlayerEventListener listner in listners)
		{
			Destroy(listner);
		}
	}

	public void MakeGAmeMAnagerListingToNewSelectedUnit(Unit unit)
	{
		clearGameManagerFromPreviousSelectedUnit();
		// the Subject (Trigger) is the GameManager and the listner is Current Selected Unit

		foreach (ActionData action in unit.actions)
		{
			PlayerAction playerEvent = action.Actionevent;
			PlayerEventListener e = gameObject.AddComponent<PlayerEventListener>();
			e.GameEvent = playerEvent;
			e.response = new UnityEvent();
			e.register();
			if (unit == null) return;

			if (playerEvent is ShootActionEvent)
			{
				e.response.AddListener(unit.CreateNewShootAction);
			}
			if (playerEvent is LunchGrenadeActionEvent)
			{
				e.response.AddListener(unit.GetComponent<Grenadier>().CreateLunchGrenadeAction);
			}
		}
	}

	public void clearPreviousSelectedUnitFromAllVoidEvents(Unit unit)
	{
		if (unit == null) return;
		VoidListner[] listners = unit.listners.GetComponents<VoidListner>();
		if (listners == null) return;
		foreach (VoidListner listner in listners)
		{
			Destroy(listner);
		}
	}

	public void MakeOnlySelectedUnitListingToEventArgument(Unit unit, VoidEvent voidEvent)
	{
		// the Subject (Trigger) is the GameManager and the listner is Current
		// Selected.currentTarget Or the Subject current Selected Unit listner is Current
		// Selected.currentTarget

		if (voidEvent == null || unit == null) Debug.Log($" unit OR void event is null");

		if (unit == null || voidEvent == null) return;
		VoidListner e = unit.listners.AddComponent<VoidListner>();
		e.GameEvent = voidEvent;
		e.UnityEventResponse = new UnityVoidEvent();

		e.Register();
		e.UnityEventResponse.AddListener((EventArgument) =>
		{
			//EventArgument is what ever argument is passed when we trugger(raise the
			//Event ) in this case its Void(no argument)
			if (voidEvent is UnitSwitchAllyEvent)
			{
				unit.listners.GetComponent<CallBackOnListen>().onPlayerChangeEventTrigger();
			}
			else if (voidEvent is UnitSwitchTargetEvent)
			{
				unit.listners.GetComponent<CallBackOnListen>().onTargetChangeEventTrigger();
			}
			else if (voidEvent is StatsChangeEvent)
			{
				unit.HealthBar.GetComponent<HealthBar>().onEquipementEventTrigger();
			}
		});
	}

	public void clearPreviousSelectedUnitFromAllWeaponEvent(Unit unit)
	{
		if (unit == null) return;
		WeaponListner[] listners = unit.listners.GetComponents<WeaponListner>();
		foreach (WeaponListner listner in listners)
		{
			Destroy(listner);
		}
	}

	public void MakeOnlySelectedUnitListingToWeaponEvent(Unit unit, WeaponEvent weaponEvent)
	{
		// the Subject (Trigger) is the current Selected Unit and the listner is Current
		// Selected.currentTarget
		if (unit == null || weaponEvent == null)
		{
			Debug.Log($"  unit OR weapon event is null"); return;
		}
		WeaponListner e = unit.listners.AddComponent<WeaponListner>();
		e.GameEvent = weaponEvent;
		e.UnityEventResponse = new UnityWeaponEvent();
		e.UnityEventResponse.AddListener((EventArgument) =>
		{
			// EventArgument is what ever argument is passed when we trugger (raise the
			// Event ) in this case its Weapon
			unit.listners.GetComponent<UnitCallBack>().onWeaponShootEventTrigger(EventArgument);
		});

		e.Register();
	}

	public void clearPreviousSelectedUnitFromAlEquipementEvent(Unit unit)
	{
		if (unit == null) return;
		EquipementListner[] listners = unit.listners.GetComponents<EquipementListner>();
		//if (listners == null) return;
		foreach (EquipementListner listner in listners)
		{
			Destroy(listner);
		}
	}

	public void MakeOnlySelectedUnitListingToEquipeEvent(Unit unit, EquipementEvent equipeEvent)
	{
		// the Subject (Trigger) is the Equipement GAme Object in the scene and the listner
		// is the Current Selected Unit
		if (equipeEvent == null) Debug.Log($"weapon event is null");
		if (unit == null || equipeEvent == null) return;
		EquipementListner e = unit.listners.AddComponent<EquipementListner>();
		e.GameEvent = equipeEvent;
		e.UnityEventResponse = new UnityEquipementEvent();
		e.UnityEventResponse.AddListener((EventArgument) =>
		{
			// EventArgument is what ever argument is passed when we trugger (raise the
			// Event ) in this case its Weapon
			//unit.listners.GetComponent<UnitCallBack>().onEquipeEventTrigger(EventArgument);
		});

		e.Register();
	}

	public void MakeOnlySelectedUnitListingGrenadeExplosionEvent(Unit unit, GrenadeExplosion grenadeExplotionEvent)
	{
		// the Subject (Trigger) is the Equipement GAme Object in the scene and the listner
		// is the Current Selected Unit

		if (unit == null || grenadeExplotionEvent == null)
		{
			Debug.Log($"unit / weapon event is null"); return;
		}
		GrenadeExplosionListner e = unit.listners.AddComponent<GrenadeExplosionListner>();
		e.GameEvent = grenadeExplotionEvent;
		e.UnityEventResponse = new UnityGrenadeExplosionEvent();
		e.UnityEventResponse.AddListener((EventArgument) =>
		{
			// EventArgument is what ever argument is passed when we trugger (raise the
			// Event ) in this case its Weapon
			unit.listners.GetComponent<UnitCallBack>().onGrenadeExplodes(EventArgument);
		});

		e.Register();
	}

	public void clearPreviousSelectedUnitFromAllGrenadeExplosionEvent(Unit unit)
	{
		if (unit == null) return;
		GrenadeExplosionListner[] listners = unit.listners.GetComponents<GrenadeExplosionListner>();
		//if (listners == nuWeaponListnerll) return;
		foreach (GrenadeExplosionListner listner in listners)
		{
			Destroy(listner);
		}
	}

	public void MakeOnlySelectedUnitListingToBoolEvent(Unit unit, BoolEvent boolEvent)
	{
		// the Subject (Trigger) is the Equipement GAme Object in the scene and the listner
		// is the Current Selected Unit

		if (unit == null || boolEvent == null)
		{
			Debug.Log($"unit / bool event is null"); return;
		}
		BoolListner e = unit.listners.AddComponent<BoolListner>();
		e.GameEvent = boolEvent;
		e.UnityEventResponse = new UnityBoolEvent();
		e.UnityEventResponse.AddListener((EventArgument) =>
		{
			// EventArgument is what ever argument is passed when we trugger (raise the
			// Event ) in this case its Weapon
			unit.listners.GetComponent<UnitCallBack>().onUnitFlunked(EventArgument);
		});

		e.Register();
	}

	public void clearPreviousSelectedUnitFromAllBoolEvent(Unit unit)
	{
		if (unit == null) return;
		BoolListner[] listners = unit.listners.GetComponents<BoolListner>();
		//if (listners == nuWeaponListnerll) return;
		foreach (BoolListner listner in listners)
		{
			Destroy(listner);
		}
	}

	public void PlayerDied(PlayerStateManager player)
	{
		if (didGameEnd() == false)
		{
			checkIfselectedUnitDied(player);
		}
	}

	private void checkIfselectedUnitDied(PlayerStateManager unit)
	{
		GameStateManager manager = GameStateManager.Instance;

		if (manager.SelectedUnit == unit)
		{
			if (manager.State is PlayerTurn)
			{
				manager.SelectedUnit = manager.players.FirstOrDefault();
			}
			else if (manager.State is EnemyTurn)
			{
				manager.SelectedUnit = manager.enemies.FirstOrDefault();
			}
		}
	}

	public async Task delayEndEvent(int time)
	{
		await Task.Delay(time);
		GameEndedEvent.Raise();


	}
	private bool didGameEnd()
	{
		GameStateManager manager = GameStateManager.Instance;

		if (manager.enemies.Where(unit => unit.State is Dead).Count() == manager.enemies.Count || manager.enemies.Count == 0)
		{
			Debug.LogError("PLAYER WINS CONGRADUATION");
			delayEndEvent(1500);
			return true;
		}
		if (manager.players.Where(unit => unit.State is Dead).Count() == manager.players.Count || manager.players.Count == 0)
		{
			Debug.LogError("ENEMIES WINS CONGRADUATION");
			delayEndEvent(1500);
			return true;
		}
		return false;
	}
}
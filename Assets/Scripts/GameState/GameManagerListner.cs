using gameEventNameSpace;
using UnityEngine;
using UnityEngine.Events;

public class GameManagerListner : MonoBehaviour
{
	private void clearGameManagerFromPreviousSelectedUnit()
	{
		PlayerEventListener[] listners = gameObject.GetComponents<PlayerEventListener>();
		foreach (PlayerEventListener listner in listners)
		{
			Destroy(listner);
		}
	}

	public void MakeGAmeMAnagerListingToNewSelectedUnit(AnyClass unit)
	{
		clearGameManagerFromPreviousSelectedUnit();
		foreach (ActionData action in unit.actions)
		{
			PlayerAction playerEvent = action.Actionevent;
			PlayerEventListener e = gameObject.AddComponent<PlayerEventListener>();
			e.GameEvent = playerEvent;
			e.response = new UnityEvent();
			e.register();
			if (unit == null) return;
			if (playerEvent is MovementActionEvent)
			{
				e.response.AddListener(unit.CreateNewMoveAction);
			}
			if (playerEvent is ShootActionEvent)
			{
				e.response.AddListener(unit.CreateNewShootAction);
			}
		}
	}

	public void clearPreviousSelectedUnitFromAllVoidEvents(AnyClass unit)
	{
		if (unit == null) return;
		VoidListner[] listners = unit.listners.GetComponents<VoidListner>();
		foreach (VoidListner listner in listners)
		{
			Destroy(listner);
		}
	}

	public void MakeOnlySelectedUnitListingToEventArgument(AnyClass unit, VoidEvent voidEvent)
	{
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
		});
	}

	public void clearPreviousSelectedUnitFromAllWeaponEvent(AnyClass unit)
	{
		if (unit == null) return;
		WeaponListner[] listners = unit.listners.GetComponents<WeaponListner>();
		foreach (WeaponListner listner in listners)
		{
			Destroy(listner);
		}
	}

	public void MakeOnlySelectedUnitListingToWeaponEvent(AnyClass unit, WeaponEvent weaponEvent)
	{
		if (unit == null || weaponEvent == null) return;
		WeaponListner e = unit.listners.AddComponent<WeaponListner>();
		e.GameEvent = weaponEvent;
		e.UnityEventResponse = new UnityWeaponEvent();
		e.UnityEventResponse.AddListener((EventArgument) =>
		{
			// EventArgument is what ever argument is passed when we trugger (raise the
			// Event ) in this case its Weapon
			unit.listners.GetComponent<CallBackOnListen>().onWeaponShootEventTrigger(EventArgument);
		});

		e.Register();
	}
}
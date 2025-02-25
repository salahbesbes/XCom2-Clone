﻿using UnityEngine.Events;

namespace gameEventNameSpace
{
	[System.Serializable]
	public class UnityVoidEvent : UnityEvent<Void>
	{
	}

	[System.Serializable]
	public class UnityIntEvent : UnityEvent<int>
	{
	}

	[System.Serializable]
	public class UnityStateEvent : UnityEvent<BaseState<GameStateManager>>
	{
	}

	[System.Serializable]
	public class UnityWeaponEvent : UnityEvent<UnitStats>
	{
	}

	[System.Serializable]
	public class UnityGrenadeExplosionEvent : UnityEvent<UnitStats>
	{
	}

	[System.Serializable]
	public class UnityEquipementEvent : UnityEvent<EquipementData>
	{
	}

	[System.Serializable]
	public class UnityNotifyGameManagerEvent : UnityEvent<PlayerStateManager>
	{
	}

	[System.Serializable]
	public class UnityInventoryEvent : UnityEvent<Inventory>
	{
	}

	[System.Serializable]
	public class UnityBoolEvent : UnityEvent<bool>
	{
	}
	[System.Serializable]
	public class UnityStringEvent : UnityEvent<string>
	{
	}
}
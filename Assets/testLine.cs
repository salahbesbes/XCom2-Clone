using System;
using UnityEngine;

public static class Predict
{
	public static float Distance(float speed, float drag, float time)
	{
		var characteristicTime = 1 / drag;
		return (float)(speed * characteristicTime * (1 - Math.Pow(Math.E, -time / characteristicTime)));
	}

	public static float Speed(float speed, float distance, float drag)
	{
		var characteristicTime = 1 / drag;
		var time = Time(speed, distance, drag);
		return (float)(speed * Math.Pow(Math.E, -time / characteristicTime));
	}

	public static float Time(float speed, float distance, float drag)
	{
		var characteristicTime = 1 / drag;
		return (float)(characteristicTime * Math.Log(characteristicTime * speed / (characteristicTime * speed - distance)));
	}

	public static Vector3 Position(Vector3 velocity, float drag, float time)
	{
		var characteristicTime = 1 / drag;
		return velocity * characteristicTime * (float)(1 - Math.Pow(Math.E, -time / characteristicTime));
	}

	public static Vector3 Position(Vector3 velocity, float drag, float time, Vector3 force, float mass)
	{
		var terminalVelocity = force / drag / mass;
		var characteristicTime = 1 / drag;
		var compound = (float)Math.Pow(Math.E, -time / characteristicTime);
		return terminalVelocity * time + velocity * characteristicTime * (1 - compound) + terminalVelocity * characteristicTime * (compound - 1);
	}

	public static Vector3 Force(Vector3 velocity, float drag, float time, Vector3 target, float mass)
	{
		var compound = (float)Math.Pow(Math.E, drag * time);
		return drag * mass * (drag * target * compound - velocity * compound + velocity) / (compound * (drag * time - 1) + 1);
	}

	public static Vector3 Velocity(Vector3 force, float drag, float time, Vector3 target, float mass)
	{
		var compound = (float)Math.Pow(Math.E, drag * time);
		return (drag * drag * mass * target * compound + force * (compound * (1 - drag * time) - 1)) / (drag * mass * (compound - 1));
	}
}
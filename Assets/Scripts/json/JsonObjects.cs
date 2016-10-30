using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

[System.Serializable]
public class PlayerSpringJoint2D
{
	public float distance;
	public float dampingRatio;
	public float frequency;
}

[System.Serializable]
public class PlayerHookShot
{
	public int speed;
}

[System.Serializable]
public class PlayerSpeedLimit
{
	public int speed;	
}

public static class JsonObjects
{
	public static PlayerSpringJoint2D playerSpringJoint2D;
	public static PlayerHookShot playerHookShot;
	public static PlayerSpeedLimit playerSpeedLimit;
	
	static JsonObjects()
	{
		playerSpringJoint2D = JsonIO.LoadJson<PlayerSpringJoint2D>();
		playerHookShot = JsonIO.LoadJson<PlayerHookShot>();
		playerSpeedLimit = JsonIO.LoadJson<PlayerSpeedLimit>();
	}
}

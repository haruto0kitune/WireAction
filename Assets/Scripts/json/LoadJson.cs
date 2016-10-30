using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class LoadJson : MonoBehaviour 
{
    void Awake ()
    {

    }

    void Start () 
    {
	
    }
	
	public void LoadJsonObjects()
	{
		JsonObjects.playerSpringJoint2D = JsonIO.LoadJson<PlayerSpringJoint2D>();
		Debug.Log("distance: " + JsonObjects.playerSpringJoint2D.distance);
		Debug.Log("Damping Ratio: " + JsonObjects.playerSpringJoint2D.dampingRatio);
		Debug.Log("frequency: " + JsonObjects.playerSpringJoint2D.frequency);
		JsonObjects.playerHookShot = JsonIO.LoadJson<PlayerHookShot>();
		Debug.Log("speed: " + JsonObjects.playerHookShot.speed);
	}
}

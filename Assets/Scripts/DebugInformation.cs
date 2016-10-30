using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class DebugInformation : MonoBehaviour 
{
	[SerializeField]
	Text playerSpringJoint2DDistance;
	[SerializeField]
	Text playerSpringJoint2DDampingRatio;
	[SerializeField]
	Text playerSpringJoint2DFrequency;
	[SerializeField]
	Text playerHookShotSpeed;
	[SerializeField]
	Text playerSpeedLimit;
	
    void Awake ()
    {
	    
    }

	void Start () 
	{
		this.ObserveEveryValueChanged(x => JsonObjects.playerSpringJoint2D.distance).SubscribeToText(playerSpringJoint2DDistance);
	    this.ObserveEveryValueChanged(x => JsonObjects.playerSpringJoint2D.dampingRatio).SubscribeToText(playerSpringJoint2DDampingRatio);
	    this.ObserveEveryValueChanged(x => JsonObjects.playerSpringJoint2D.frequency).SubscribeToText(playerSpringJoint2DFrequency);
		this.ObserveEveryValueChanged(x => JsonObjects.playerHookShot.speed).SubscribeToText(playerHookShotSpeed);
		this.ObserveEveryValueChanged(x => JsonObjects.playerSpeedLimit.speed).SubscribeToText(playerSpeedLimit);
    }
}

using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerConfig : MonoBehaviour 
{
	[SerializeField]
	GameObject player_shot;
	[SerializeField]
	GameObject hook;
	
    void Awake ()
    {

    }

    void Start () 
	{
		// Load Json every one minute
	    this.UpdateAsObservable()
		    .ThrottleFirstFrame(60)
		    .Subscribe(x => 
		    {
			    JsonObjects.playerSpringJoint2D = JsonIO.LoadJson<PlayerSpringJoint2D>();
			    JsonObjects.playerHookShot = JsonIO.LoadJson<PlayerHookShot>();
			    JsonObjects.playerSpeedLimit = JsonIO.LoadJson<PlayerSpeedLimit>();
		    });
		
		this.ObserveEveryValueChanged(x => JsonObjects.playerSpringJoint2D)
			.Subscribe(x => 
			{
				hook.GetComponent<SpringJoint2D>().distance = x.distance;
				hook.GetComponent<SpringJoint2D>().dampingRatio = x.dampingRatio;
				hook.GetComponent<SpringJoint2D>().frequency= x.frequency;
			});
		
		this.ObserveEveryValueChanged(x => JsonObjects.playerHookShot)
			.Subscribe(x => player_shot.GetComponent<HookShot>().speed = x.speed);
		
		this.ObserveEveryValueChanged(x => JsonObjects.playerSpeedLimit)
			.Subscribe(x => GetComponent<SpeedLimit>().speed = x.speed);
    }
}

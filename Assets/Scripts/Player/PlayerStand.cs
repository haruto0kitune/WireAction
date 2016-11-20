using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerStand : MonoBehaviour 
{
	[SerializeField]
	GameObject player;
	ObservableStateMachineTrigger observableStateMachineTrigger;
	
    void Awake ()
    {
	    observableStateMachineTrigger = player.GetComponent<Animator>().GetBehaviour<ObservableStateMachineTrigger>();
    }

    void Start () 
	{
		// player_stand -> player_shot
	    observableStateMachineTrigger
		    .OnStateUpdateAsObservable()
		    .Where(x => x.StateInfo.IsName("player_stand"))
		    .Where(x => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		    .Subscribe(_ => 
		    {
		    	_.Animator.SetBool("isStanding", false);
		    	_.Animator.SetBool("isShooting", true);
		    });
    }
}

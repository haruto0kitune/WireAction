using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerFalling : MonoBehaviour 
{
	[SerializeField]
	GameObject player;
	ObservableStateMachineTrigger observableStateMachineTrigger;
	[SerializeField]
	LayerMask layerMask;
	
    void Awake ()
    {
	    observableStateMachineTrigger = player.GetComponent<Animator>().GetBehaviour<ObservableStateMachineTrigger>();
    }

    void Start () 
	{
		observableStateMachineTrigger
			.OnStateUpdateAsObservable()
			.Where(x => x.StateInfo.IsName("player_fall"))
			.Where(x => Physics2D.Linecast(player.transform.position, new Vector2(player.transform.position.x, player.transform.position.y - 0.2f), layerMask))
			.Subscribe(_ => 
			{
				_.Animator.SetBool("isFalling", false);
				_.Animator.SetBool("isStanding", true);
			});
    }
}

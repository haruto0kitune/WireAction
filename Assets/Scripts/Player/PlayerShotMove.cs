using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerShotMove : MonoBehaviour 
{
	[SerializeField]
	GameObject player;
	Rigidbody2D playerRigidbody2d;
	ObservableStateMachineTrigger observableStateMachineTrigger;
	[SerializeField]
	GameObject hook;
	
	bool isShotMoveUpper;
	
    void Awake ()
    {
	    playerRigidbody2d = player.GetComponent<Rigidbody2D>();
	    observableStateMachineTrigger = player.GetComponent<Animator>().GetBehaviour<ObservableStateMachineTrigger>();
    }

    void Start () 
	{
		observableStateMachineTrigger
			.OnStateEnterAsObservable()
			.Where(x => x.StateInfo.IsName("player_shot_move_upper"))
			.Subscribe(_ => isShotMoveUpper = true);
		
		observableStateMachineTrigger
			.OnStateUpdateAsObservable()
			.Where(x => x.StateInfo.IsName("player_shot_move_upper"))
			.Where(x => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			.Subscribe(_ => 
			{
				_.Animator.SetBool("isShotMoveUpper", false);
				_.Animator.SetBool("isFalling", true);
				isShotMoveUpper = false;
			});
		
		//this.FixedUpdateAsObservable()
		//    .Where(x => isShotMoveUpper)
		//    .Subscribe(_ => 
		//    {
		//    	var vector = hook.transform.position - player.transform.position;
		//    	playerRigidbody2d.AddForce(vector.normalized * 15);
		//    	Debug.DrawLine(player.transform.position, vector.normalized * 30, Color.red);
		//    });
    }
}

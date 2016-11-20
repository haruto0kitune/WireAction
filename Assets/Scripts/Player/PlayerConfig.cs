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
    JsonObjects JsonObjects;
	
    void Awake ()
    {
    }

    IEnumerator Start () 
	{
        JsonObjects = GameObject.Find("JsonObjects").GetComponent<JsonObjects>();
        yield return new WaitUntil(() => JsonObjects.hasFinishedCopy);

        this.ObserveEveryValueChanged(x => JsonObjects.dictionary)
            .Subscribe(x =>
            {
                //PlayerSpringJoint2D
                hook.GetComponent<SpringJoint2D>().distance = JsonObjects.dictionary["PlayerSpringJoint2D"].GetField("distance").n;
                hook.GetComponent<SpringJoint2D>().dampingRatio = JsonObjects.dictionary["PlayerSpringJoint2D"].GetField("dampingRatio").n;
                hook.GetComponent<SpringJoint2D>().frequency = JsonObjects.dictionary["PlayerSpringJoint2D"].GetField("frequency").n;

                // PlayerHookShot
                player_shot.GetComponent<HookShot>().speed = (int)JsonObjects.dictionary["PlayerHookShot"].GetField("speed").n;

                // PlayerSpeedLimit
                GetComponent<SpeedLimit>().speed = (int)JsonObjects.dictionary["PlayerSpeedLimit"].GetField("speed").n;

                //PlayerRigidbody2D
                GetComponent<Rigidbody2D>().gravityScale = JsonObjects.dictionary["PlayerRigidbody2D"].GetField("gravityScale").n;
                GetComponent<Rigidbody2D>().isKinematic = JsonObjects.dictionary["PlayerRigidbody2D"].GetField("isKinematic").b;
            });
    }
}

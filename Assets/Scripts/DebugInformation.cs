using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
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
	[SerializeField]
	Text playerRigidbody2DGravityScale;
    JsonObjects JsonObjects;

	IEnumerator Start () 
	{
        JsonObjects = GameObject.Find("JsonObjects").GetComponent<JsonObjects>();
        yield return new WaitUntil(() => JsonObjects.hasFinishedCopy);

        this.ObserveEveryValueChanged(x => JsonObjects.dictionary)
            .Subscribe(_ =>
            {
                // PlayerSpringJoint2D
                playerSpringJoint2DDistance.text = _["PlayerSpringJoint2D"].GetField("distance").n.ToString();
                playerSpringJoint2DDampingRatio.text = _["PlayerSpringJoint2D"].GetField("dampingRatio").n.ToString();
                playerSpringJoint2DFrequency.text = _["PlayerSpringJoint2D"].GetField("frequency").n.ToString();

                // PlayerHookShot
                playerHookShotSpeed.text = _["PlayerHookShot"].GetField("speed").n.ToString();

                // PlayerSpeedLimit
                playerSpeedLimit.text = _["PlayerSpeedLimit"].GetField("speed").n.ToString();

                // PlayerRigidbody2D
                playerRigidbody2DGravityScale.text = _["PlayerRigidbody2D"].GetField("gravityScale").n.ToString();
            });
    }
}

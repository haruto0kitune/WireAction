using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class SaveJsonOnStartup : MonoBehaviour 
{
    void Awake ()
    {
	    StartCoroutine(JsonIO.SaveJson("PlayerSpringJoint2D.json"));
	    StartCoroutine(JsonIO.SaveJson("PlayerHookShot.json"));
	    StartCoroutine(JsonIO.SaveJson("PlayerSpeedLimit.json"));
    }

    void Start () 
    {
    }
}

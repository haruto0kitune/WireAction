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
        GameObject.Find("JsonObjects").GetComponent<JsonObjects>().dictionary = JsonIO.LoadAllJson();
	}
}

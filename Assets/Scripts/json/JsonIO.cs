using UnityEngine;
using System.Collections;
using System.IO;
using UniRx;
using UniRx.Triggers;

public static class JsonIO
{
	public static IEnumerator SaveJson(string filename)
	{
		string fromPath = Application.streamingAssetsPath + "/" + filename;
		WWW www;
		
		if(Application.platform == RuntimePlatform.WindowsEditor)
		{
			www = new WWW("file:///" + fromPath);
			yield return www;
		}
		else
		{
			www = new WWW(fromPath);
			yield return www;
		}
		
		string toPath = Application.persistentDataPath + "/" + filename;
		File.WriteAllBytes(toPath, www.bytes);
	}
	
	public static T LoadJson<T>()
	{
		var path = Application.persistentDataPath + "/" + typeof(T).Name + ".json";
		var json = File.ReadAllText(path);
		return JsonUtility.FromJson<T>(json);
	}
}

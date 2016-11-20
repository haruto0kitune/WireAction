using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using UniRx;
using UniRx.Triggers;

public static class JsonIO
{
	/// <summary>
	/// Copy streamingAssets to persistentData
	/// </summary>
	/// <param name="filename">filename of jsonfile in StreamingAssets</param>
	/// <returns></returns>
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

    public static IEnumerator CopyJsonInStreamingAssetsToPersistentData()
    {
        foreach (var i in Directory.GetFiles(Application.streamingAssetsPath, "*.json"))
        {
            string fromPath = Application.streamingAssetsPath + "/" + Path.GetFileName(i);
            WWW www;

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                www = new WWW("file:///" + fromPath);
                yield return www;
            }
            else
            {
                www = new WWW(fromPath);
                yield return www;
            }

            string toPath = Application.persistentDataPath + "/" + Path.GetFileName(i);
            File.WriteAllBytes(toPath, www.bytes);
        }
    }

    /// <summary>
    /// Load json using JsonUtility
    /// </summary>
    /// <param name="filename">filename of jsonfile in StreamingAssets</param>
    /// <returns></returns>	
    public static T LoadJson<T>()
	{
		var path = Application.persistentDataPath + "/" + typeof(T).Name + ".json";
		var json = File.ReadAllText(path);
		return JsonUtility.FromJson<T>(json);
	}

    /// <summary>
    /// Load json using JSONObject
    /// </summary>
    /// <param name="filename">filename of jsonfile in StreamingAssets</param>
    /// <returns></returns>	
    public static JSONObject LoadJson(string filename)
    {
        var path = Application.persistentDataPath + "/" + filename;
        var json = File.ReadAllText(path);
        return new JSONObject(json);
    }

    public static Dictionary<string, JSONObject> LoadAllJson()
    {
        var dictionary = new Dictionary<string, JSONObject>();

        foreach(var i in Directory.GetFiles(Application.persistentDataPath, "*.json"))
        {
            var json = File.ReadAllText(i);
            dictionary[Path.GetFileNameWithoutExtension(i)] = new JSONObject(json);
        }

        return dictionary;
    } 
}

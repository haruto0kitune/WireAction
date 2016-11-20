using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UniRx.Triggers;

public class JsonObjects : MonoBehaviour
{
    public Dictionary<string, JSONObject> dictionary;
    public bool hasFinishedCopy;

    IEnumerator Start()
    {
        // Wait CopyJsonInStreamingAssetsToPersistentData
        var coroutine = StartCoroutine(JsonIO.CopyJsonInStreamingAssetsToPersistentData());
        yield return coroutine;
        hasFinishedCopy = true;

        // Load json as Dictionary<string, JSONObject>
        dictionary = JsonIO.LoadAllJson();

        // When file was overwritten, Load json
        var watcher = new FileSystemWatcher(Application.persistentDataPath) { Filter = "*.json", NotifyFilter = NotifyFilters.LastWrite, EnableRaisingEvents = true };
        watcher.ChangedAsObservable().ObserveOnMainThread().Subscribe(_ => dictionary = JsonIO.LoadAllJson());
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PrefabLoader : MonoBehaviour
{
    public string _prefabName;
    public UnityEvent _eventOnLoad;
    public UnityEvent _eventOnDestroy;

    private static GameObject _loadedInstance;
    private static PrefabLoader _prefabLoader;
    private static int _lastLoadedId = -1;

    private void Start()
    {
        _loadedInstance = null;
        _prefabLoader = null;
        _lastLoadedId = -1;
    }

    public void LoadPrefab(int id)
    {
        if(_lastLoadedId != id)
        {
            if(_loadedInstance != null)
            {
                _prefabLoader._eventOnDestroy.Invoke();
                Destroy(_loadedInstance);
                Resources.UnloadUnusedAssets();
            }
            Load(id);
        }
    }

    public void Load(int id)
    {
        _prefabLoader = this;
        _lastLoadedId = id;
        _loadedInstance = Instantiate(Resources.Load(_prefabName), transform.parent) as GameObject;
        _eventOnLoad.Invoke();
    }

    public static void ClearPrefab()
    {
        _lastLoadedId = -1;
        if (_loadedInstance != null)
        {
            Destroy(_loadedInstance);
            Resources.UnloadUnusedAssets();
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class TaggedPrefab
{
    public string name;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "projData", menuName = "UI/PackageSettings")]
public class EUIPackageSettings : ScriptableObject
{
    public List<TaggedPrefab> prefabs;
    private Dictionary<string, GameObject> prefabList;

    private void OnValidate()
    {
        prefabList = new Dictionary<string, GameObject>();
        foreach (TaggedPrefab a in prefabs)
        {
            if (a == null || prefabList.ContainsKey(a.name)) { continue; }
            prefabList.Add(a.name, a.prefab);
        }
    }
}

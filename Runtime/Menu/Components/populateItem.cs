using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public class populateItems : MonoBehaviour
{
    public GameObject layoutGroup; // where to place generated items
    // Start is called before the first frame update
    void Start()
    {
        GUIutil.clearChildren(layoutGroup.transform, "none", true);
        generateItems();
    }

    public virtual void generateItems()
    {

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        PrefabStage editingPrefab = PrefabStageUtility.GetCurrentPrefabStage();
        if (editingPrefab != null || PrefabUtility.IsPartOfAnyPrefab(this)) { return; }
        foreach (Transform child in transform)
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (child != null)
                {
                    UnityEditor.Undo.DestroyObjectImmediate(child.gameObject);
                }
            };
        }
        generateItems();
    }
#endif
}

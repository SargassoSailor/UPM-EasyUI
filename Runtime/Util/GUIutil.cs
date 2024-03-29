﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ObjectToDictionaryHelper
{
    public static IDictionary<string, object> ToDictionary(this object source)
    {
        return source.ToDictionary<object>();
    }

    /*public static Dictionary<string,object> ToFullDictionary(this object source)
    {
        Dictionary<string, object> dict = (Dictionary<string, object>)ToDictionary(source);//not fully converted. 
        //dict["Keys"]
        //dict["Values"]

        Dictionary<string,object> fulldict = new Dictionary<string, object>();
        foreach()

    }*/

    public static IDictionary<string, T> ToDictionary<T>(this object source)
    {
        if (source == null)
            ThrowExceptionWhenSourceArgumentIsNull();

        var dictionary = new Dictionary<string, T>();
        foreach (PropertyDescriptor property in System.ComponentModel.TypeDescriptor.GetProperties(source))
            AddPropertyToDictionary<T>(property, source, dictionary);
        return dictionary;
    }

    private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
    {
        object value = property.GetValue(source);
        if (IsOfType<T>(value))
            dictionary.Add(property.Name, (T)value);
    }

    private static bool IsOfType<T>(object value)
    {
        return value is T;
    }

    private static void ThrowExceptionWhenSourceArgumentIsNull()
    {
        throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
    }
}

public static class GUIutil
{
    public static Transform RecursiveFindChild(Transform parent, string childName)
    {
        Transform child = null;
        for (int i = 0; i < parent.childCount; i++)
        {
            child = parent.GetChild(i);
            if (child.name == childName)
            {
                break;
            }
            else
            {
                child = RecursiveFindChild(child, childName);
                if (child != null)
                {
                    break;
                }
            }
        }

        return child;
    }

    public static void matchObjSizeWithParent(GameObject child, GameObject parent)
    {
        RectTransform rectTransform = child.GetComponent<RectTransform>();

        rectTransform.position = parent.GetComponent<RectTransform>().position;
        rectTransform.anchorMin = new Vector2(0, 0);

        rectTransform.anchorMax = new Vector2(1, 1);

        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }



    public static void SetTimer(Timer t, float delay, ElapsedEventHandler callbackFunc)
    {
        if (t != null)
        {
            t.Stop();
        }

        // Create a timer with a two second interval.
        t = new System.Timers.Timer(delay);//in ms
                                           // Hook up the Elapsed event for the timer. 
        t.Elapsed += callbackFunc;
        t.AutoReset = true;
        t.Enabled = true;
    }

    public static void clearChildren(Transform t, string exception = "none", bool immediate = false)
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (Transform child in t)
        {
            if (child.name != exception)
            {
                objs.Add(child.gameObject);
            }

        }
        foreach (GameObject c in objs)
        {
            if (immediate)
            {
                Object.DestroyImmediate(c);
            }
            else
            {
                Object.Destroy(c);
            }
        }
    }

    public static void changeChildPrefab(Transform t, GameObject newPrefab)
    {
        clearChildren(t);
        UnityEngine.Object.Instantiate(newPrefab, t);
    }
}

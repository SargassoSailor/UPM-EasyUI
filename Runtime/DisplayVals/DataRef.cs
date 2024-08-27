using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EUI
{
    [System.Serializable]
    public class DataRef
    {
        //support non-monobehaviour classes too
        public GameObject objectToTrack;

        public Component selComp;

        public string selVar ="";

        public string selSubVar = "";
        [HideInInspector]
        public Type compType;

        //used by custom inspector

        public int compIdx = 0;

        public int varIdx = 0;

        public int subVarIdx;

        public DataRef() { }

        public void SetTrackedObj(GameObject obj)
        {
            objectToTrack = obj;
            if (selComp)
            {
                selComp = obj.GetComponent(compType);
            }
        }

        public static bool isProperty(string name)
        {
            string varType = name.Substring(0, 2);
            string varName = name.Substring(2);
            string val = "";
            if (varType == "p-")
            {
                return true;
            }
            else if (varType == "f-")
            {
                return false;
            }
            return false;
        }

        public string GetValue()
        {
            if (selComp != null && selVar != "")
            {
                string varType = selVar.Substring(0, 2);
                string varName = selVar.Substring(2);
                string val = "null";
                object obj = null;
                if (varType == "p-")
                {
                    //varName = selComp.GetType().GetProperty(varName).Name;
                    PropertyInfo p = selComp.GetType().GetProperty(varName);
                    if(p != null)
                    {
                        obj = selComp.GetType().GetProperty(varName).GetValue(selComp);
                    }
                }
                else if (varType == "f-")
                {
                    //varName = selComp.GetType().GetField(varName).Name;
                    FieldInfo f = selComp.GetType().GetField(varName);
                    if(f!=null)
                    {
                        obj = selComp.GetType().GetField(varName).GetValue(selComp);
                    }
                }

                if (selSubVar != "" && selSubVar != "None")
                {
                    string subVarType = selSubVar.Substring(0, 2);
                    string subVarName = selSubVar.Substring(2);
                    varName += $".{subVarName}";

                    try
                    {
                        object subObj = null;
                        if (subVarType == "p-")
                        {
                            //varName = selComp.GetType().GetProperty(varName).Name;
                            subObj = obj.GetType().GetProperty(subVarName).GetValue(obj);
                        }
                        else if (subVarType == "f-")
                        {
                            //varName = selComp.GetType().GetField(varName).Name;
                            subObj = obj.GetType().GetField(subVarName).GetValue(obj);
                        }
                        if (subObj != null) { val = subObj.ToString(); }
                    }
                    catch(Exception e) 
                    {
                        Debug.LogError($"Could not print {obj}.{subVarName}");
                        return "";
                    }

                }
                else
                {
                    if (obj != null) { val = obj.ToString(); }
                }
                return val;
            }
            return "";
        }
    }
}


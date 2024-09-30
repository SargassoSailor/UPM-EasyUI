using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EUI
{
    [System.Serializable]
    public class DataRegisterRef
    {
        public string registryListName = "";
        public string registryEntryName = "";

        public int registryListIdx = 0;
        public int registryEntryIdx = 0;

        public string ValueName { get { return DataRegister.GetTypeOptions(type)[registryEntryIdx]; } }

        [SerializeField]
        public string type;
        public DataRegisterRef(string regListName = "")
        {
            this.registryListName = regListName;
        }

        public DataRegisterRef(Type boundType)
        {
            this.type = boundType.ToString();
        }

        public void SetTrackedObj(GameObject obj)
        {

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
            return "";
        }
    }
}



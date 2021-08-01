using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Packages.ChrisAsh.EUI.Editor
{
    
    public static class PackageSetup
    {
        [InitializeOnLoadMethod]
        public static void DoSetup()
        {
            ProjectData data = ProjectSettings.GetData();
            if(data==null)
            {
                bool canCopy = AssetDatabase.CopyAsset("Packages/com.chrisash.eui/DefaultProjectFile/projectData.asset", "Assets/Resources/ProjectSettings1.asset");
                if (!canCopy) 
                {
                    data = new ProjectData();
                    AssetDatabase.CreateAsset(data, "Assets/Resources/ProjectSettings1.asset"); 
                }
            }
        }
    }
}

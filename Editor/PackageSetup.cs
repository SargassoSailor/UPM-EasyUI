using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;

namespace Packages.ChrisAsh.EUI.Editor
{
    
    public static class PackageSetup
    {
        public const string k_SettingsPath = "Assets/Resources/";
        public const string k_SettingsName = "EUIProject.asset";
        [InitializeOnLoadMethod]
        public static void DoSetup()
        {
            var settings = AssetDatabase.LoadAssetAtPath<ProjectData>(k_SettingsPath+k_SettingsName);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<ProjectData>();

                if (!Directory.Exists(k_SettingsPath))
                {
                    Directory.CreateDirectory(k_SettingsPath);
                }

                AssetDatabase.CreateAsset(settings, k_SettingsPath + k_SettingsName);
                AssetDatabase.SaveAssets();
            }
            ProjectSettings.data = settings;
            ProjectSettings.data.versionNum = GetGitVersion();
        }

        internal static SerializedObject GetSerializedSettings()
        {
            if(ProjectSettings.data == null) { DoSetup(); }
            return new SerializedObject(ProjectSettings.data);
        }

        public static string GetGitVersion()
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.IndexOf("/Assets"));
            path += "/.git";
            string buildNum = "0";
            if (Directory.Exists(path))
            {
                buildNum = ExecuteCommand("git rev-list HEAD --count");
            }
            
            return buildNum;
        }

        public static string ExecuteCommand(string Command)
        {
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
            ProcessInfo.RedirectStandardOutput = true;

            Process = Process.Start(ProcessInfo);
            string output = Process.StandardOutput.ReadToEnd();
            Process.WaitForExit();
            return output.Split('\n')[0];
        }
    }
}

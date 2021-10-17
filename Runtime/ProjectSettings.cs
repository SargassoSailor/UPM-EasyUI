using UnityEngine;

public static class ProjectSettings
{
    public static ProjectData data;
    public static AudioSource audioPlayer;
    public static ProjectData Data { get { return GetData(); } }

    public static ProjectData GetData()
    {
        if (data != null) { return data; }
        else
        {
            ProjectData[] dataSettings = Resources.LoadAll<ProjectData>("");
            if (dataSettings.Length > 0) { data = dataSettings[0]; Debug.Log("Loaded ProjectData:" + data.ToString()); }
            else { Debug.LogError("Cannot find ProjectData"); }
            return data;
        }
    }

    public static string returnStr(string valName)
    {
        if (data == null) { return ""; }

        string val = (string)data.GetType().GetField(valName)?.GetValue(data);
        return val;
    }
}

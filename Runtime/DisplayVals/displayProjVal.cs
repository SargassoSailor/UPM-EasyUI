using UnityEngine;
using UnityEngine.UI;
/// <summary>
// Display a projectValue in a gui element.
/// </summary>


public class displayProjVal : valDisplayBase
{
    public override string GetVal(string var)
    {
       return ProjectSettings.returnStr(var);
    }
}

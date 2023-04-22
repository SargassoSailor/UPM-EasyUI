using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class displayGMval : valDisplayBase
{
    public override string GetVal(string var)
    {
        return BaseGM.returnDBStrVal(var);
    }
}

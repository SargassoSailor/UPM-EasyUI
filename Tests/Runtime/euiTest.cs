using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class euiTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void euiProjectDataExists()
    {
        ProjectData d ProjectSettings.GetData();

        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator Packages_com.chrisash.eui_Tests_Tests_NewTestScriptWithEnumeratorPasses()
    {
        
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

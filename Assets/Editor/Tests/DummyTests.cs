using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EditModeTest
{
	[Test]
	public void SimplePassingTest()
	{
		// Use the Assert class to test conditions.
		Assert.IsTrue(true);
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator NewPlayModeTestWithEnumeratorPasses() 
	{
		Assert.IsTrue(true);
		yield return null;
		Assert.IsTrue(true);
	}
}

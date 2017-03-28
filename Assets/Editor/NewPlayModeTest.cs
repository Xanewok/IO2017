﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewPlayModeTest {

	[Test]
	public void NewPlayModeTestSimplePasses() {
		Assert.IsTrue(true);
		// Use the Assert class to test conditions.
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator NewPlayModeTestWithEnumeratorPasses() {
		Assert.IsTrue(false);
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}

	[UnityTest]
	public void FailingUnityTest() {
		Assert.IsTrue(false);
	}
}

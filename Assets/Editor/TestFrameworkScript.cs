using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestFrameworkScript : MonoBehaviour
{
	[Test]
	public void SimpleAddition()
	{
		Assert.That(2 + 2 == 4);
	}

	[Test]
	public void FailingTest()
	{
		Assert.That(2 + 2 == 5);
	}
}

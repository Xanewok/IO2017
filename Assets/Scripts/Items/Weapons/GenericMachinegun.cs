using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMachinegun : GenericShotty {

	/**
	 * Lazy!!!
	 */
	public override void onUse()
	{
		base.onUse();
		if (actualMagazine > 0 || actualMagazine == -1)
		{
			animator.SetTrigger("Shoot");
		}
		else
		{
			animator.SetTrigger("Reload");
		}
	}
}

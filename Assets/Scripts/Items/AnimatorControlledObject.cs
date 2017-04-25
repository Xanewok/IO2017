using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Baseclass for objects that are mainly controlled by animator (like most weapons)
/// </summary>
public class AnimatorControlledObject : SimpleItem
{
    /// <summary>
    /// Object animator
    /// </summary>
    public Animator animator;

    protected override void onStart()
    {
        base.onStart();
        if (animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
        }
    }

    public override void onEquip(int hand)
    {
        base.onEquip(hand);
        animator.SetTrigger("Equip");
    }

    public override void onUnEquip()
    {
        base.onUnEquip();
        animator.SetTrigger("UnEquip");
    }

    public override void onUseStart()
    {
        base.onUseStart();
        animator.SetTrigger("AttackStart");
    }

    public override void onUseEnd()
    {
        base.onUseEnd();
        animator.SetTrigger("AttackEnd");
    }

}

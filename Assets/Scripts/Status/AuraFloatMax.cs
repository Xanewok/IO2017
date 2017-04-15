using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraFloatMax : AuraContainer<float> {

    protected Dictionary<int, float> values = new Dictionary<int, float>();
    int lastNum = 0;

    //Not the best, but it may be enough.
    public override int addAuraInternal(float value)
    {
        values.Add(lastNum, value);
        return lastNum++;
    }

    public override void delAuraInternal(int token)
    {
        values.Remove(token);
    }

    public override float getValue()
    {
        Dictionary<int, float>.Enumerator enumerator = values.GetEnumerator();
        float maxVal = 0f;
        while(enumerator.MoveNext())
        {
            maxVal = Mathf.Max(maxVal, enumerator.Current.Value);
        }
        return maxVal;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputMapper
{
    private static Dictionary<int, int> mapping = new Dictionary<int, int> { { 0, 0 }, { 1, 1 } };

    public static int PlayerNumToControllerLayoutIndex(int playerNum)
    {
        return mapping[playerNum];
    }

    public static void SetControllerLayoutIndex(int playerNum, int layoutIndex)
    {
        mapping[playerNum] = layoutIndex;
    }
}

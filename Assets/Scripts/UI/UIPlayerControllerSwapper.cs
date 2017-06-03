using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerControllerSwapper : MonoBehaviour
{
    public Text player1LayoutName;
    public Text player2LayoutName;

    private int player1LayoutIndex;
    private int player2LayoutIndex;

    void Start()
    {
        player1LayoutIndex = InputMapper.PlayerNumToControllerLayoutIndex(0);
        player2LayoutIndex = InputMapper.PlayerNumToControllerLayoutIndex(1);

        UpdateLayoutNames();
    }

    public void SwapControllers()
    {
        Swap(ref player1LayoutIndex, ref player2LayoutIndex);

        InputMapper.SetControllerLayoutIndex(0, player1LayoutIndex);
        InputMapper.SetControllerLayoutIndex(1, player2LayoutIndex);

        UpdateLayoutNames();
    }

    private void UpdateLayoutNames()
    {
        player1LayoutName.text = InputMapper.GetControllerLayoutName(player1LayoutIndex);
        player2LayoutName.text = InputMapper.GetControllerLayoutName(player2LayoutIndex);
    }

    private void Swap(ref int a, ref int b)
    {
        int tmp = a;
        a = b;
        b = tmp;
    }
}

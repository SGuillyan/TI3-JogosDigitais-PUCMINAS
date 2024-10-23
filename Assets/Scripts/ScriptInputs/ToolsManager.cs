using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ToolsManager : MonoBehaviour
{
    public enum Tools
    {
        None,
        Plow,
        Flatten,
        Harvest
    }

    public static Tools activeTool = 0;
    private static Toggle[] toolList;

    public static bool isToolBoxOpen = false;
    private static Animator animator;



    private void Start()
    {
        toolList = new Toggle[transform.childCount];
        for (int i = 0; i < toolList.Length; i++)
        {
            toolList[i] = transform.GetChild(i).GetComponent<Toggle>();
        }

        animator = GetComponent<Animator>();
    }



    public static void ChangeTool(Toggle ignore)
    {
        if (activeTool != Tools.None)
        {
            for (int i = 0; i < toolList.Length; i++)
            {
                if (toolList[i].isOn && toolList[i] != ignore)
                {
                    toolList[i].isOn = false;
                }
            }
        }
    }

    public static void SetActiveTool(Tools tool)
    {
        activeTool = tool;
    }

    public static void CloseToolBox()
    {
        animator.Play("Close_ToolBox");
    }

    public static void SetIsToolBoxOpen(bool value)
    {
        isToolBoxOpen = value;
    }
}

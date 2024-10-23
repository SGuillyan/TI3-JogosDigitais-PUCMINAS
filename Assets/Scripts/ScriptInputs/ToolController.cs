using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToolController : MonoBehaviour
{
    Toggle tool;

    [SerializeField] ToolsManager.Tools type;

    private void Start()
    {
        tool = GetComponent<Toggle>();
    }

    public void Action()
    {
        if (ToolsManager.isToolBoxOpen)
        {
            ToolsManager.SetIsToolBoxOpen(false);

            if (tool.isOn)
            {
                ToolsManager.ChangeTool(tool);
                ToolsManager.SetActiveTool(type);
            }
            else
            {
                ToolsManager.SetActiveTool(ToolsManager.Tools.None);
            }

            ToolsManager.CloseToolBox();
        }
    }
}

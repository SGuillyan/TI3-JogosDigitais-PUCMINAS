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
            if (tool.isOn)
            {
                // Desativa os outros toggles e ativa a ferramenta correspondente
                ToolsManager.ChangeTool(this);

                // Se a ferramenta não for Plant ou Fertilize, ativa a ferramenta correspondente
                if (type != ToolsManager.Tools.Plant && type != ToolsManager.Tools.Fertilize)
                {
                    ToolsManager.SetActiveTool(type);
                }
                else
                {
                    // Para Plant e Fertilize, apenas ativa a ferramenta sem alterar o toggle
                    ToolsManager.SetActiveTool(type);
                }
            }
            else
            {
                // Se o toggle foi desmarcado, desativa a ferramenta correspondente
                ToolsManager.SetActiveTool(ToolsManager.Tools.None);
            }
        }
    }
}

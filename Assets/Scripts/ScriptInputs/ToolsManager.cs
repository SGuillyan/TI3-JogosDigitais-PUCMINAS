using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsManager : MonoBehaviour
{
    public enum Tools
    {
        None,
        Plow,
        Harvest,
        Info,
        Plant,
        Fertilize,
        Flatten,
        CutDown,
        Water,
    }

    [SerializeField] public static Tools activeTool = Tools.None;
    private static Toggle[] toolList;

    public static bool isToolBoxOpen = false;
    private static Animator animator;

    private void Start()
    {
        // Pega todos os toggles na caixa de ferramentas
        toolList = new Toggle[transform.childCount];
        for (int i = 0; i < toolList.Length; i++)
        {
            toolList[i] = transform.GetChild(i).GetComponent<Toggle>();
        }

        animator = GetComponent<Animator>();
    }

    public void Update(){
        // Debug.Log(activeTool);
    }

    // Método para desmarcar todos os toggles, exceto o que foi passado
    public static void ChangeTool(ToolController currentToolController)
    {
        for (int i = 0; i < toolList.Length; i++)
        {
            // Se o Toggle não for o atual (que está sendo ativado), desmarque
            if (toolList[i].isOn && toolList[i] != currentToolController.GetComponent<Toggle>())
            {
                toolList[i].isOn = false;
            }
        }
    }

    // Método para ativar ou desativar a ferramenta
    public static void SetActiveTool(Tools tool)
    {
        // Se a ferramenta ativa for diferente da ferramenta que está sendo ativada/desativada
        if (activeTool != tool)
        {
            activeTool = tool;
            Debug.Log("Ferramenta Ativada: " + activeTool);
        }
    }

    // Desativa todos os toggles
    public static void DeactivateAllTools()
    {
        for (int i = 0; i < toolList.Length; i++)
        {
            toolList[i].isOn = false;
        }
    }

    public static void ToolBoxAnim()
    {
        if (isToolBoxOpen)
        {
            animator.Play("Close_ToolBox");
            isToolBoxOpen = false;
        }
        else
        {
            animator.Play("Open_ToolBox");
            isToolBoxOpen = true;
        }
    }
}

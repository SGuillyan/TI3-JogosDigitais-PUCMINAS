using System;
using UnityEngine;
using UnityEngine.UI;

public class ToolBoxIcone : MonoBehaviour
{
    [Header("Access")]
    [SerializeField] private Image image;

    [Header("Icons")]
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Sprite plowIcon;
    [SerializeField] private Sprite flattenIcon;
    [SerializeField] private Sprite harvestIcon;
    [SerializeField] private Sprite cutDownIcon;
    [SerializeField] private Sprite infoIcon;

    private void Update()
    {
        switch (ToolsManager.activeTool)
        {
            case ToolsManager.Tools.None:
                image.sprite = defaultIcon;
                break;
            case ToolsManager.Tools.Plow:
                image.sprite = plowIcon;
                break;
            case ToolsManager.Tools.Flatten:
                image.sprite = flattenIcon;
                break;
            case ToolsManager.Tools.Harvest:
                image.sprite = harvestIcon;
                break;
            case ToolsManager.Tools.CutDown:
                image.sprite = cutDownIcon;
                break;
            case ToolsManager.Tools.Info:
                image.sprite = infoIcon;
                break;
        }
    }
}

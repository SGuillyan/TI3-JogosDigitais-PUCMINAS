using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] TileSelector tileSelector;
    [SerializeField] CameraController cameraController;

    // Update is called once per frame
    void Update()
    {
        tileSelector.SelectTile();
        
        // Analytics
        if (Input.touchCount > 0)
        {
            if (AnalyticsManager.GetInativeTime() > 1f)
            {
                AnalyticsSystem.AddAnalyticInativeTime_Seconds(this.name, "Inative time", AnalyticsManager.GetInativeTime());
                AnalyticsSystem.AddAnalyticInativeTime_Formated(this.name, "Inative time", AnalyticsManager.GetInativeTime());
            }
        }
        else
        {
            AnalyticsManager.AddInativeTime();
        }
    }

    private void FixedUpdate()
    {
        if (ToolsManager.activeTool == ToolsManager.Tools.None && !MenuManager.openedMenu)
        {
            cameraController.CameraInput();
        }
    }
}

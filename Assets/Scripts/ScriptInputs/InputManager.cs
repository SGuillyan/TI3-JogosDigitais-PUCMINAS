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
        Debug.Log(ToolsManager.activeTool);
    }

    private void FixedUpdate()
    {
        if (ToolsManager.activeTool == ToolsManager.Tools.None)
        {
            cameraController.CameraInput();
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstaclesMapGenerator))]
public class ShowingMouseWorldCoords : Editor
{
    void OnSceneGUI(SceneView sceneView)
    {
        Vector2 mousePosition = Event.current.mousePosition;
        mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y;
        Vector2 worldMousePosition = sceneView.camera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.y *= -1;
        Debug.Log($"World position: {worldMousePosition}");
    }

    void OnScene(SceneView sceneView)
    {
        Vector2 mousePosition = Event.current.mousePosition;
        mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y;
        Vector2 worldMousePosition = sceneView.camera.ScreenToWorldPoint(mousePosition);
        worldMousePosition.y *= -1;
        Debug.Log($"World position: {worldMousePosition}");
    }
}

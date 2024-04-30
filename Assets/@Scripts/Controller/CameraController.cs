using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

    private void Start()
    {
        Rect safeArea = Screen.safeArea; 

        Camera mainCamera = Camera.main;
        Rect cameraViewport = mainCamera.rect;

        float widthRatio = safeArea.width / Screen.width;
        float heightRatio = safeArea.height / Screen.height;
        float offsetX = safeArea.x / Screen.width;
        float offsetY = safeArea.y / Screen.height;

        cameraViewport.x += offsetX;
        cameraViewport.y += offsetY;
        cameraViewport.width = widthRatio;
        cameraViewport.height = heightRatio;

        mainCamera.rect = cameraViewport;

        mainCamera.clearFlags = CameraClearFlags.SolidColor;
    }

    private void LateUpdate()
    {
        if (Player == null)
            return;

        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10); 
    }
}

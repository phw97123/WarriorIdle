using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController Player { get { return Managers.ObjectManager?.Player; } }

    private void LateUpdate()
    {
        if (Player == null)
            return;
        if (GameObject.FindGameObjectWithTag("DungeonObject") != null)
        {
            transform.position = new Vector3(0, 0, -10); 
        }
        else
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector3      position;
    [HideInInspector]
    public Quaternion   rotation;
    [HideInInspector]
    public string       animation;

    public PlayerData(string animation, Vector3 position, Quaternion rotation)
    {
        this.animation = animation;
        this.position = position;
        this.rotation = rotation;
    }
}

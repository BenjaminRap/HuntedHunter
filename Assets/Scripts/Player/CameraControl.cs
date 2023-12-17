using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private float            maxXOffset;
    [SerializeField]
    private float            maxYOffset;
    [SerializeField]
    private float            moveSpeedY;
    [SerializeField]
    private float            moveSpeedX;
    private GameObject       player;
    private float            XOffset;
    private float            YOffset;

    void Start()
    {
        XOffset = transform.localPosition.x;
        YOffset = transform.localPosition.y;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x + XOffset, player.transform.position.y + YOffset, transform.position.z);
    }

    public void    MoveX(float distance)
    {
        float newXOffset;

        newXOffset = XOffset + distance * moveSpeedX * Time.deltaTime;
        if (newXOffset < maxXOffset && newXOffset > -maxXOffset)
        {
            XOffset = newXOffset;
            transform.localPosition = new Vector3(XOffset, transform.localPosition.y, transform.localPosition.z);
        }
    }

    public void    MoveY(float distance)
    {
        float newYOffset;

        newYOffset = YOffset + distance * moveSpeedY * Time.deltaTime;
        if (newYOffset < maxYOffset && newYOffset > -maxYOffset)
        {
            YOffset = newYOffset;
            transform.localPosition = new Vector3(transform.localPosition.x, newYOffset, transform.localPosition.z);
        }
    }
}

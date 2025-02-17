using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLevel : Level
{
    private float       wallSpeed;
    [SerializeField]
    private GameObject  wall;
    [SerializeField]
    private float       maxSpeed;
    [SerializeField]
    private float       speedAcceleration;

    protected override void SecondStart()
    {
        wallSpeed = 0;
    }

    protected override void WinAction()
    {

    }

    protected override void LoseAction()
    {

    }

    protected override void WakeUpAction()
    {

    }

    protected override void SecondUpdate()
    {
        if (!start)
            return ;
        if (wallSpeed < maxSpeed)
            wallSpeed += speedAcceleration * Time.deltaTime;
        wall.transform.position += Vector3.right * wallSpeed * Time.deltaTime;
    }
}

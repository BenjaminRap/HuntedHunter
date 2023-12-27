using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Level       level;
    private GameObject  player;

    void    Start()
    {
        level = transform.root.gameObject.GetComponent<Level>();
        player = level.GetPlayer();
    }

    void    OnTriggerEnter2D(Collider2D col)
    {
        if (level.HasStarted() && col.gameObject == player)
        {
            Debug.Log("Wall too fast");
            StartCoroutine(level.Lose());
        }
    }
}

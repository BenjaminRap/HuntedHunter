using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Level       level;
    private GameObject  player;

    void    Start()
    {
        level = transform.root.GetComponent<Level>();
        player = level.GetPlayer();
    }

    void    OnTriggerEnter2D(Collider2D col)
    {
        if (level.HasStarted() && col.gameObject == player)
            StartCoroutine(level.Win());
    }
}

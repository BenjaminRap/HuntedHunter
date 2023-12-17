using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private Level       level;
    private GameObject  player;
    [SerializeField]
    private float       winDistance;

    void    Start()
    {
        level = GameObject.FindGameObjectsWithTag("Levels")[0].GetComponent<Level>();
        player = level.GetPlayer();
    }

    void Update()
    {
        if (!level.HasStarted())
            return ;
        if (Vector3.Magnitude(transform.position - player.transform.position) < winDistance)
            StartCoroutine(level.Win());
    }
}

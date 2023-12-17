using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField]
    private List<PlayerData>    datas;
    [SerializeField]
    private bool                active;
    [SerializeField]
    private float               delay;
    private Coroutine           move;
    private int                 index;
    private Level               level;

    void    Start()
    {
        level = GameObject.FindGameObjectsWithTag("Levels")[0].GetComponent<Level>();
        index = 0;
        active = false;
    }

    public void Init(List<PlayerData> datas, float delay)
    {
        this.datas = datas;
        this.delay = delay;
    }

    public void StartRun()
    {
        active = true;
    }

    void    Update()
    {
        if (!active)
            return ;
        if (index >= datas.Count - 1)
        {
            StartCoroutine(level.Lose());
            return ;
        }
        if (move == null)
            move = StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float   delta;

        delta = 0;
        while (delta < delay)
        {
            transform.position = Vector3.Lerp(datas[index].position, datas[index + 1].position, delta / delay);
            transform.rotation = Quaternion.Lerp(datas[index].rotation, datas[index + 1].rotation, delta / delay);
            delta += Time.deltaTime;
            yield return null;
        }
        move = null;
        index++;
    }

    public void MoveToIndex(int index)
    {
        if (index >= datas.Count)
            return ;
        transform.position = datas[index].position;
        transform.rotation = datas[index].rotation;
        this.index = index;
    }
}

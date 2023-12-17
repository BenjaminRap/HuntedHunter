using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordMotion : MonoBehaviour
{
    [SerializeField]
    private float               delay;
    private float               delta;
    private bool                recording;
    private List<PlayerData>    datas;

    public void StartRecording()
    {
        datas = new List<PlayerData>();
        recording = true;
        StartCoroutine(Record());
    }

    public void EndRecording()
    {
        recording = false;
    }

    IEnumerator Record()
    {
        while (recording)
        {
            datas.Add(new PlayerData("", transform.position, transform.rotation));
            yield return (new WaitForSeconds(delay));
        }
    }

    public List<PlayerData> GetDatas()
    {
        return (datas);
    }

    public float    GetDelay()
    {
        return (delay);
    }
}

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
    private Rigidbody2D         rb;
    private PlayerPhysics       player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerPhysics>();
        recording = false;
    }

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
        string    anim;


        while (recording)
        {
            if (player.isGrounded)
            {
                if (rb.velocity.y >= 0)
                    anim = "Jump";
                else
                    anim = "Fall";
            }
            else
            {
                if (Vector3.Magnitude(rb.velocity) == 0)
                    anim = "Idle";
                else
                    anim = "Run";
            }
            datas.Add(new PlayerData(anim, transform.position, transform.rotation));
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

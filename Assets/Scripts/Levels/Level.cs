using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Level : MonoBehaviour
{
    protected GameObject    player;
    [SerializeField]
    private Vector3         startPosition;
    private RawImage        blackScene;
    [SerializeField]
    private string          currentLevel;
    [SerializeField]
    private string          nextLevel;
    [SerializeField]
    private float           time;
    [SerializeField]
    private float           wakeUpTime;
    protected bool          start;
    private InputManager    input;
    private GameObject      currentLevel_object;
    private GameObject      nextLevel_object;
    private GameObject      shadow_prefab;
    private RecordMotion    record;
    private Shadow          shadow;
    private int             space_pressed;

    protected abstract void SecondStart();

    protected abstract void WinAction();

    protected abstract void LoseAction();

    protected abstract void WakeUpAction();

    void    Awake()
    {
        player = GameObject.Find("Player");
    }

    void    Start()
    {
        start = false;
        space_pressed = 0;
        input = player.GetComponent<InputManager>();
        blackScene = GameObject.Find("BlackScene").GetComponent<RawImage>();
        record = player.GetComponent<RecordMotion>();
        player.transform.position = startPosition;
        player.transform.rotation = Quaternion.identity;
        input.SleepingInput();
        currentLevel_object = Resources.Load("Levels/" + currentLevel) as GameObject;
        nextLevel_object = Resources.Load("Levels/" + nextLevel) as GameObject;
        shadow_prefab = Resources.Load("Entities/Shadow") as GameObject;
        StartCoroutine(Fade(-1, 0, time));
        shadow = SpawnShadow();
        SecondStart();
    }

    public void Space()
    {
        if (shadow == null)
        {
            WakeUp();
            return ;
        }
        space_pressed++;
        shadow.MoveToIndex(space_pressed);
        if (space_pressed * record.GetDelay() > wakeUpTime)
            WakeUp();
    }

    public void WakeUp()
    {
        WakeUpAction();
        record.StartRecording();
        input.OnFootInput();
        start = true;
        if (shadow != null)
            shadow.StartRun();
    }

    public GameObject   GetPlayer()
    {
        return (player);
    }

    public IEnumerator  Lose()
    {
        LoseAction();
        record.EndRecording();
        record.GetDatas().Clear();
        start = false;
        StartCoroutine(Fade(1, 255, time));
        yield return (new WaitForSeconds(time));
        Instantiate(currentLevel_object, Vector3.zero, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public IEnumerator  Win()
    {
        WinAction();
        record.EndRecording();
        start = false;
        StartCoroutine(Fade(1, 255, time));
        yield return (new WaitForSeconds(time));
        Instantiate(nextLevel_object, Vector3.zero, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private IEnumerator Fade(int sign, byte stop, float time)
    {
        float   alpha;

        alpha = 255 - stop;
        while (blackScene.color.a != stop)
        {
            alpha += sign * 255 / time * Time.deltaTime;
            blackScene.color = Color.SetAlpha(blackScene.color, Mathf.FloorToInt(alpha));
            yield return null;
        }
    }

    private Shadow  SpawnShadow()
    {
        RecordMotion        record;
        List<PlayerData>    datas;
        GameObject          shadow_instance;

        shadow_instance = null;
        record = player.GetComponent<RecordMotion>();
        datas = record.GetDatas();
        if (datas != null && datas.Count != 0)
        {
            shadow_instance = Instantiate(shadow_prefab, datas[0].position, datas[0].rotation);
            shadow_instance.GetComponent<Shadow>().Init(datas, record.GetDelay());
        }
        else
            return (null);
        return (shadow_instance.GetComponent<Shadow>());
    }

    public bool HasStarted()
    {
        return (start);
    }
}

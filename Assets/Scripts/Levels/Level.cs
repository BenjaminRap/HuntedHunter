using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Level : MonoBehaviour
{
    protected GameObject        player;
    [SerializeField]
    private Vector3             startPosition;
    private RawImage            blackScene;
    [SerializeField]
    private string              currentLevel;
    [SerializeField]
    private string              nextLevel;
    [SerializeField]
    private float               time;
    [SerializeField]
    private float               wakeUpTime;
    [SerializeField]
    private float               minShadowStart;
    [SerializeField]
    private float               minHeight;
    protected bool              start;
    private InputManager        input;
    private GameObject          currentLevel_object;
    private GameObject          nextLevel_object;
    private GameObject          shadow_prefab;
    private GameObject          hand;
    private RecordMotion        record;
    private Shadow              shadow;
    private int                 space_pressed;
    private List<PlayerData>    datas;
    private Animator            anim;

    protected abstract void SecondStart();

    protected abstract void SecondUpdate();

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
        hand = GameObject.Find("Hand");
        hand.SetActive(false);
        record = player.GetComponent<RecordMotion>();
        anim = player.GetComponent<Animator>();
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

    void Update()
    {
        if (player.transform.position.y < minHeight)
        {
            Debug.Log("fell into the void");
            StartCoroutine(Lose());
        }
        SecondUpdate();
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
        if (space_pressed * record.GetDelay() > wakeUpTime && Vector3.Distance(shadow.transform.position, player.transform.position) > minShadowStart)
            WakeUp();
    }

    public void WakeUp()
    {
        start = true;
        anim.SetTrigger("WakeUp");
        WakeUpAction();
        record.StartRecording();
        input.OnFootInput();
        if (shadow != null)
            shadow.StartRun();
    }

    private IEnumerator HandCatch()
    {
        float   time;

        time = 0f;
        hand.SetActive(true);
        while (time < 1.5f)
        {
            hand.transform.position = player.transform.position + new Vector3(15f, 15f, 0f);
            time += Time.deltaTime;
            yield return null;
        }
        hand.SetActive(false);
    }

    public GameObject   GetPlayer()
    {
        return (player);
    }

    public IEnumerator  Lose()
    {
        GameObject  newLevel;

        Debug.Log("lost");
        start = false;
        LoseAction();
        anim.SetTrigger("Lose");
        record.EndRecording();
        record.GetDatas().Clear();
        StartCoroutine(Fade(1, 255, time));
        yield return (new WaitForSeconds(time));
        hand.SetActive(true);
        newLevel = Instantiate(currentLevel_object, Vector3.zero, Quaternion.identity);
        newLevel.GetComponent<Level>().SetDatas(datas);
        if (shadow != null)
            Destroy(shadow.transform.gameObject);
        Destroy(this.gameObject);
    }

    public IEnumerator  Win()
    {
        GameObject  newLevel;

        Debug.Log("Win");
        start = false;
        WinAction();
        StartCoroutine(HandCatch());
        yield return (new WaitForSeconds(0.7f));
        anim.SetTrigger("Win");
        yield return (new WaitForSeconds(1.5f));
        record.EndRecording();
        datas = record.GetDatas();
        StartCoroutine(Fade(1, 255, time));
        yield return (new WaitForSeconds(time));
        hand.SetActive(true);
        newLevel = Instantiate(nextLevel_object, Vector3.zero, Quaternion.identity);
        newLevel.GetComponent<Level>().SetDatas(datas);
        if (shadow != null)
            Destroy(shadow.transform.gameObject);
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
        GameObject          shadow_instance;

        shadow_instance = null;
        record = player.GetComponent<RecordMotion>();
        if (datas != null && datas.Count != 0)
        {
            shadow_instance = Instantiate(shadow_prefab, datas[0].position, datas[0].rotation);
            shadow_instance.GetComponent<Shadow>().Init(datas, record.GetDelay());
            shadow_instance.transform.parent = transform;
            GameObject.Find("Canvas").GetComponent<LocateShadow>().Locate(this, shadow_instance, player);
        }
        else
            return (null);
        return (shadow_instance.GetComponent<Shadow>());
    }

    public bool HasStarted()
    {
        return (start);
    }

    public float GetFadeTime()
    {
        return (time);
    }

    public void SetDatas(List<PlayerData> datas)
    {
        this.datas = datas;
    }
}

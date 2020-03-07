using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    private const int END_SCORE = 3000;

    public GameObject joystick;
    public GameObject gamePanel;
    public GameObject destination;
    public Button pause;
    public GameObject spawn;

    private bool isEndCheck = false;

    public Animator endingAni;
    public AudioSource endingAudio;


    void Start()
    {
        isEndCheck = false;
        endingAudio = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (!isEndCheck && GameManager.instance.totalScore >= END_SCORE)
        {
            EndGame();
            isEndCheck = true;
        }
    }


    private void EndGame()
    {
        joystick.GetComponent<Image>().raycastTarget = false;
        pause.enabled = false;
        spawn.SetActive(false);
        Player.instance.isReversed = false;
        Player.instance.ShieldEnd();
        Player.instance.seeingEnding = true;

        GameObject[] targetObj = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in targetObj)
        {
            if (obj.activeInHierarchy)
            {
                if (obj.CompareTag("Spear"))
                {
                    obj.SetActive(false);
                }
                else if (obj.CompareTag("Indicator"))
                {
                    obj.SetActive(false);
                }
                else if (obj.CompareTag("Enemy"))
                {
                    TurtleFollow turtleFollow = obj.GetComponent<TurtleFollow>();
                    TurtleLinear turtleLinear = obj.GetComponent<TurtleLinear>();
                    TurtleBlinder turtleBlinder = obj.GetComponent<TurtleBlinder>();
                    TurtleBoom turtleBoom = obj.GetComponent<TurtleBoom>();
                    TurtleConfuser turtleConfuser = obj.GetComponent<TurtleConfuser>();
                    TurtleSpear turtleSpear = obj.GetComponent<TurtleSpear>();
                    TurtleSuddenAttack turtleSuddenAttack = obj.GetComponent<TurtleSuddenAttack>();
                    TurtleSummoner turtleSummoner = obj.GetComponent<TurtleSummoner>();
                    TurtleSummons turtleSummons = obj.GetComponent<TurtleSummons>();

                    if (turtleFollow != null) turtleFollow.OnDead(false);
                    else if (turtleLinear != null) turtleLinear.OnDead(false);
                    else if (turtleBlinder != null) turtleBlinder.OnDead(false);
                    else if (turtleBoom != null) turtleBoom.OnDead(false);
                    else if (turtleConfuser != null) turtleConfuser.OnDead(false);
                    else if (turtleSpear != null) turtleSpear.OnDead(false);
                    else if (turtleSuddenAttack != null) turtleSuddenAttack.OnDead(false);
                    else if (turtleSummoner != null) turtleSummoner.OnDead(false);
                    else if (turtleSummons != null) turtleSummons.OnDead(false);
                }
                else if (obj.CompareTag("Dragon"))
                {
                    Dragon dragon = obj.GetComponent<Dragon>();
                    if (dragon != null) dragon.OnDead();
                }
                else if (obj.CompareTag("Swirl"))
                {
                    obj.SetActive(false);
                }
                else if (obj.CompareTag("Item"))
                {
                    Item item = obj.GetComponent<Item>();
                    if (item != null) item.EndingDestroyItem();
                }
                else if (obj.CompareTag("Soul"))
                {
                    Soul soul = obj.GetComponent<Soul>();
                    if (soul != null) soul.EndingDestroySoul();
                }
            }
        }
        StartCoroutine(UISide());
        endingAni.SetTrigger("Ending");
    }


    public void IsAliveFalse()
    {
        Player.instance.isAlive = false;
        Player.instance.KeepOnTrail();
    }


    public void EndingAudioPlay()
    {
        endingAudio.Play();
    }


    public void PlayerEnding()
    {
        GameManager.instance.PlayerDied();
    }


    IEnumerator UISide()
    {
        while (true)
        {
            joystick.transform.localPosition += new Vector3(0, -3f);
            gamePanel.transform.localPosition += new Vector3(0, 1.5f);
            destination.transform.localPosition += new Vector3(0, -1.5f);

            if(joystick.transform.localPosition.y < -370)
            {
                joystick.gameObject.SetActive(false);
                gamePanel.gameObject.SetActive(false);
                destination.gameObject.SetActive(false);
                Player.instance.TurnAngle(Vector3.up);
                break;
            }
            yield return null;
        }
    }
}

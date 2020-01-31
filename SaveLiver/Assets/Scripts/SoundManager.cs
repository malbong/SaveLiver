using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; set; }

    public AudioClip menuBGM;

    public AudioClip playBGM;

    private AudioSource audioSource;
    private bool transScene = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName == "Play Scene")
        {
            if (GameManager.instance.isPause || !Player.instance.isAlive)
            {
                audioSource.volume = 0;
            }
            else
            {
                audioSource.volume = 0.7f;
            }

            if (audioSource.clip == menuBGM)
            {
                audioSource.clip = playBGM;
                audioSource.volume = 0.7f;
                audioSource.Play();
            }
        }
        else if (sceneName == "Menu Scene")
        {
            if (audioSource.clip == playBGM)
            {
                audioSource.clip = menuBGM;
                audioSource.volume = 1f;
                audioSource.Play();
            }
        }
    }


    public void ButtonClick()
    {
        transform.Find("ButtonClick").GetComponent<AudioSource>().Play();
    }

}

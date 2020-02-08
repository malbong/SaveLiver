using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    public Canvas logoCanvas;
    public Text hosanText;
    public Image logoImage;
    public GameObject soundObject;


    void Start()
    {
        if (!PlayerInformation.IsSawLogo)
        {
            logoCanvas.gameObject.SetActive(true);
            StartCoroutine(LogoEnd());
        }
        else
        {
            SoundManager.instance.menuAudio.Play();
        }
    }


    IEnumerator LogoEnd()
    {
        yield return new WaitForSeconds(4f);
        PlayerInformation.IsSawLogo = true;

        SoundManager.instance.menuAudio.Play();
        logoCanvas.gameObject.SetActive(false);
    }
}

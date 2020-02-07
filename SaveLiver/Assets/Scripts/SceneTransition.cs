using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private bool isRunning = false;
    public GameObject LoadingPanel;


    public void LoadPlayScene()
    {
        StartCoroutine(LoadScene());
    }


    public bool isSceneTransitionRunning()
    {
        return isRunning;
    }


    IEnumerator LoadScene()
    {
        isRunning = true;

        LoadingPanel.SetActive(true);
        yield return new WaitForSeconds(3f);

        isRunning = false;

        SceneManager.LoadScene(1);
    }
}

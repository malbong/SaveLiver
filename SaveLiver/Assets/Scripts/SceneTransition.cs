using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transition;
    private bool isRunning = false;


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

        transition.SetTrigger("Transition");
        yield return new WaitForSeconds(3f);

        isRunning = false;

        SceneManager.LoadScene(1);
    }
}

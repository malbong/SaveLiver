using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transition;


    public void LoadPlayScene()
    {
        StartCoroutine(LoadScene());
    }


    IEnumerator LoadScene()
    {
        transition.SetTrigger("Transition");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(1);
    }
}

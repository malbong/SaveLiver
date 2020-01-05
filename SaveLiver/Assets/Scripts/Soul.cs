using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public void CreateSoul(Vector3 createPosition)
    {
        float random = Random.Range(0f, 1f);
        if (random < 0.6f) // 60%
        {
            GameObject obj = ObjectPooler.instance.GetSoul();
            obj.transform.position = createPosition;
            obj.SetActive(true);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag != "Player") return;

        GetComponent<AudioSource>().Play();

        WaitForSetActiveFalse();
    }



    private IEnumerator WaitForSetActiveFalse()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }
}

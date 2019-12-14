using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigEnemy : MonoBehaviour
{
    public Sprite sprite;


    void Awake()
    {
        CreateRig();
    }
    

    /********************************************
     * @함수명 : CreateRig()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : void
     * @설명 : 자식 오브젝트가 부모 Rig을 만들고 sprite를 입힘
     */
    private void CreateRig()
    {
        GameObject rigEnemy = new GameObject("Rig Enemy");
        transform.SetParent(rigEnemy.transform);
        SpriteRenderer enemySpriteRenderer = rigEnemy.AddComponent<SpriteRenderer>();
        enemySpriteRenderer.sprite = sprite;
    }
}

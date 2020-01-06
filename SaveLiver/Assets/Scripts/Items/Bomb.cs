using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item, IItem
{
    public float itemDuration = 2f;
    private float bombItemTime = 0f;
    private bool hasItem = false;

    public Sprite bombSprite;
    public Animator anim;

    public GameObject boomEffect;

    private Rigidbody2D parent;

    void Start()
    {
        parent = GetComponentInParent<Rigidbody2D>();
        StartCoroutine(TimeCheckAndDestroy());
    }


    /**************************************
    * @함수명: OnEnable()
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 오브젝트 풀러로 인해 폭탄이 재사용될 때 실행
    *        꺼주었던 기능들을 다시 활성화해줌
    */
    private void OnEnable()
    {
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 255f;
        spriteRenderer.color = color;

        GetComponentInParent<Collider2D>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    void Update()
    {
        if (GameManager.instance.isPause) return;
        ItemDurationAndDestroy();
    }


    /**************************************
    * @함수명: ItemDurationAndDestroy()
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 폭탄 심지 타는 애니메이션 시간(itemDuration)이 끝난다면
    *        폭발효과 생성, sprite 초기화
    */
    private void ItemDurationAndDestroy()
    {
        if (Time.time - bombItemTime >= itemDuration && hasItem)
        {
            hasItem = false;
            Instantiate(boomEffect, transform.position, Quaternion.identity); // 폭발효과
            GetComponent<SpriteRenderer>().sprite = bombSprite;
            parent.gameObject.SetActive(false);
        }
    }


    /**************************************
    * @함수명: Use
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 폭탄 아이템과 충돌 시 발동
    *        Coliider를 꺼줌
    *        폭탄에 불을 붙히는 애니메이션 실행
    */
    public void Use()
    {
        ItemManager.instance.AudioPlay();
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        anim.SetBool("Fire", true);
        GetComponentInParent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        bombItemTime = Time.time;
        hasItem = true;
    }


    /**************************************
    * @ Shield와 동일
    */
    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(ItemManager.instance.itemLifeTime);
        if (!hasItem)
        {
            SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
            while (true)
            {
                Color color = spriteRenderer.color;
                color.a -= 0.05f;
                spriteRenderer.color = color;
                yield return new WaitForSeconds(0.05f);
                if (spriteRenderer.color.a <= 0.1f) break;
            }
            parent.gameObject.SetActive(false);
        }
    }
}

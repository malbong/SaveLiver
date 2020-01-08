using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public float speed = 10.0f;
    public float lifeTime = 10.0f;
    public int score = 50;
    private bool isNotMove = false;
    //private bool isHitOnPlayer = false;
    private Rigidbody2D enemyRigid;
    private Renderer dragonRenderer;
    private Renderer dragonTrackRenderer;
    public Animator DeadAnim;
    public Soul soul;

    private void Start()
    {
        dragonRenderer = GetComponent<Renderer>();
        dragonTrackRenderer = transform.GetChild(5).GetComponent<Renderer>();
        // GetChild(5) : Dragon Track
        enemyRigid = GetComponent<Rigidbody2D>();

        StartCoroutine(TimeCheckAndDestroy());
        Destroy(dragonTrackRenderer.gameObject, 1.5f);
    }


    private void FixedUpdate()
    {
        if (GameManager.instance.isPause) return;
        Move();
    }


    /**************************************
    * @함수명: Move
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 죽는 모션이 아니라면, 앞으로 전진.
    */
    public void Move()
    {
        if(!isNotMove) enemyRigid.velocity = -transform.right * speed;
    }


    /**************************************
    * @함수명: OnTriggerEnter2D
    * @작성자: zeli
    * @입력: other
    * @출력: void
    * @설명: Player와 만나면 Player 죽음
    *        BoomEffect와 만나면 OnDead 실행
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Player.instance.isFevered) OnDead();
            Player.instance.TakeDamage(true);
        }
        else if(other.CompareTag("BoomEffect"))
        {
            OnDead();
        }
    }


    /**************************************
    * @함수명: OnDead
    * @작성자: zeli
    * @입력: void
    * @출력: void
    * @설명: 제자리 정지
    *        Dead Animation 실행
    *        페이드 아웃 코루틴 호출
    */
    private void OnDead()
    {
        enemyRigid.velocity = Vector2.zero;
        isNotMove = true;

        DeadAnim.SetTrigger("Dead");
        soul.CreateSoul(transform.position + new Vector3(0, 0, 0), 0.5f);
        soul.CreateSoul(transform.position + new Vector3(1, 0, 0), 0.5f);
        soul.CreateSoul(transform.position + new Vector3(1.5f, 0, 0), 0.5f);
        soul.CreateSoul(transform.position + new Vector3(2, 0, 0), 0.5f);
        soul.CreateSoul(transform.position + new Vector3(-1, 0, 0), 0.5f);
        soul.CreateSoul(transform.position + new Vector3(-1.5f, 0, 0), 0.5f);
        soul.CreateSoul(transform.position + new Vector3(-2f, 0, 0), 0.5f);
        StartCoroutine(FadeOutAndDead());
    }


    /**************************************
    * @함수명: FadeOutAndDead
    * @작성자: zeli
    * @입력: void
    * @출력: IEnumerator
    * @설명: OnDead에서 부름
    *        0.01씩 알파값 감소 후 삭제
    */
    IEnumerator FadeOutAndDead()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        for (int i = 0; i < 100; i++)
        {
            color.a -= 0.01f;
            dragonRenderer.material.color = color;
            yield return new WaitForSeconds(0.01f);
        }
        gameObject.SetActive(false);
    }


    /**************************************
    * @함수명: TimeCheckAndDestroy()
    * @작성자: zeli
    * @입력: void
    * @출력: IEnumerator
    * @설명: lifeTime이 지난 후 비활성화
    */
    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}

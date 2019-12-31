using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float lifeTime = 10.0f;
    public Sprite sprite;
    public ParticleSystem onDeadParticle;
    public ParticleSystem onDeadParticleGetLiver;
    protected bool isAlive = true;


    private void Awake()
    {
        CreateRig();
    }


    private void Start()
    {
        isAlive = true;
    }


    protected void FixedUpdate()
    {
        if (Time.timeScale == 0) return;

        if (isAlive)
        {
            Move();
        }
        else
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }


    private void Update()
    {
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }


    /********************************************
     * @함수명 : OnDead()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : void
     * @설명 : 제한시간이 초과하거나 Trigger로 인해 죽을 때 실행시킬 함수
     *         Animation 혹은 Particle을 실행
     *         자식에서 오버라이딩 필요
     */
    public abstract void OnDead(bool getLiver = false);
    
    public abstract void Move();
    

    /********************************************
     * @함수명 : Hit()
     * @작성자 : Malbong
     * @입력 : void
     * @출력 : void
     * @설명 : OnTriggerEnter2D()에서 실행시킬 함수
     *         Player의 hp를 깎음
     */
    public virtual void HitOnPlayer(int hitCount = 1)
    {
        if (Player.instance.isFevered == true || Player.instance.HasShield == true)
        {
            OnDead(false);
        }
        else // both false (dont have fever and shield)
        {
            OnDead(true);
        }
        Player.instance.TakeDamage(hitCount);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            HitOnPlayer();
        }
        else if (other.tag == "Enemy" || other.tag == "Dragon" || other.tag == "Spear" || other.tag == "BoomEffect")
        {
            OnDead(false);
        }
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
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
        SpriteRenderer enemySpriteRenderer = rigEnemy.AddComponent<SpriteRenderer>();
        Rigidbody2D rigid = rigEnemy.AddComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        enemySpriteRenderer.sprite = sprite;
        enemySpriteRenderer.sortingLayerName = "Enemy";
    }


    protected void KeepOnTrail()
    {
        Transform trailWave = transform.Find("Trail Wave");
        if (trailWave != null)
        {
            ParticleSystem trailWaveParticle = trailWave.GetComponent<ParticleSystem>();

            trailWaveParticle.Stop();
        }
    }
}
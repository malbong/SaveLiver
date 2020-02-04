using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject indicatorObj;
    private bool hasIndicator = false;
    protected bool hasItem = false;
    protected Rigidbody2D parent;


    void Start()
    {
        hasItem = false;

        parent = gameObject.GetComponentInParent<Rigidbody2D>();

        SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;

        GetComponentInParent<SpriteRenderer>().enabled = true;
        GetComponentInParent<Collider2D>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        hasIndicator = false;

        StartCoroutine(TimeCheckAndDestroy());
    }


    protected void OnEnable()
    {
        Start();
    }


    void FixedUpdate()
    {
        if (GameManager.instance.isPause) return;

        SetMyIndicator();
        if (hasIndicator)
        {
            OffScreenIndicator.instance.DrawIndicator(gameObject, indicatorObj);
        }
    }


    /********************************************
     * @함수명 : SetMyIndicator()
     * @작성자 : zeli
     * @입력 : void
     * @출력 : bool
     * @설명 : 나의 Indicator를 생성, 제거
     *         FixedUpdate에서 실행함
     */
    private void SetMyIndicator()
    {
        if (IsOffScreen() && !hasIndicator) // 화면 밖인데 내 Indicator가 없으면
        {
            indicatorObj = ObjectPooler.instance.GetIndicatorObject(1); // 내 Indicator 생성
            indicatorObj.transform.SetParent(OffScreenIndicator.instance.transform);
            indicatorObj.SetActive(true);
            hasIndicator = true;
        }
        else if (!IsOffScreen() && hasIndicator) // 화면 안인데 내 Indicator가 있으면
        {
            indicatorObj.SetActive(false); // 내 Indicator를 비활성화
            hasIndicator = false;
        }
    }


    /********************************************
     * @함수명 : IsOffScreen()
     * @작성자 : zeli
     * @입력 : void
     * @출력 : bool
     * @설명 : Item이 화면 밖에 있는지 검사
     *         밖: true, 안: false
     */
    private bool IsOffScreen()
    {
        Vector3 vec = Camera.main.WorldToViewportPoint(transform.position);
        if (vec.x <= 1 && vec.y <= 1 && vec.x >= 0 && vec.y >= 0) // 화면 안쪽 범위
            return false;
        else
            return true;
    }


    /********************************************
     * @함수명 : OnDisable()
     * @작성자 : zeli
     * @입력 : void
     * @출력 : void
     * @설명 : Item을 비활성화 할 때, Indicator를 가지고 있다면 같이 비활성화
     */
    private void OnDisable()
    {
        if (hasIndicator && indicatorObj != null) indicatorObj.SetActive(false);
    }


    IEnumerator TimeCheckAndDestroy()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        yield return new WaitForSeconds(ItemManager.instance.itemLifeTime);
        SpriteRenderer spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        if (!hasItem)
        {
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

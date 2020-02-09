using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance { get; set; }

    // 싱글톤
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // instance 초기화
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int maxHp = 3;
    public int hp;
    public float speed;
    public float rotateSpeed;

    public bool isFevered = false;

    public ParticleSystem onDeadParticle;

    private Camera cam;

    private Coroutine runningCoroutine = null;

    public Transform arrowRotate; // 화살표 회전 컨트롤

    private Rigidbody2D playerRigid;
    public Animator feverAni;
    private SpriteRenderer playerSpriteRenderer;
    private BoxCollider2D playerCollider;

    public GameObject shield;
    public Sprite shieldSprite;
    public GameObject fever;

    public bool isAlive = true;

    public bool HasShield { get; set; } = false;
    public bool HasSpeedUp { get; set; } = false;
    public bool HasRotateUp { get; set; } = false;

    public int feverNum { get; set; } = 0;
    public int speedUpNum { get; set; } = 0;

    public Animator boatAnim;

    public Image decreaseHpImage;
    public Text decreaseHpText;

    public SpriteRenderer faceSprite;
    public SpriteRenderer boatSprite;
    public ParticleSystemRenderer waveLeftParticle;
    public ParticleSystemRenderer waveRightParticle;

    public Sprite[] boatSprites;
    public Sprite[] faceSprites;
    public Material[] waveMaterials;

    public bool isReversed = false;
    public GameObject confusionRotator;
    private bool isTriggerConfusionRunning = false;

    private bool isTriggerBlindingRunning = false;
    public GameObject blindPanel;

    public float soulLucky = 1;

    public GameObject BulletShooter;
    private bool isPlayerBeatRunning = false;

    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        runningCoroutine = StartCoroutine(RotateAngle(180, -1)); // 시작하면 Player를 180도 오른쪽으로 돌리기.

        //커스텀 능력치 적용 전 초기화
        /*
        rotateSpeed = 5f;
        speed = 4f;
        isFevered = false;//
        HasSpeedUp = false;//
        HasRotateUp = false;//
        HasShield = false;
        doubleSoulLucky = false;
        maxHp = 3;
        hp = maxHp;
        GameManager.instance.UpdateLiverCountText();
        SpawnManager.instance.itemSpawnTime = 5;
        soulLucky = 1;
        */

        UpdateCustom();
        ApplyCustomAbility();

        isReversed = false;
        isTriggerConfusionRunning = false;
        isTriggerBlindingRunning = false;
        isPlayerBeatRunning = false;
    }

    void FixedUpdate()
    {
        if (GameManager.instance.isPause) return;

        if (isAlive)
        {
            playerRigid.velocity = arrowRotate.up * speed; // 화살표 방향으로 speed만큼 직진
        }
        else
        {
            playerRigid.velocity = Vector3.zero;
        }
    }


    /**************************************
    * @함수명: TurnAngle(Vector3 currentJoystickVec)
    * @작성자: zeli
    * @입력: currentJoystickVec (조이스틱 방향)
    * @출력: void
    * @설명: 조이스틱의 방향을 읽고, 그 방향으로 화살표를 회전시키는 메소드
    *        터치가 발생하면 호출됨
    */
    public void TurnAngle(Vector3 currentJoystickVec)
    {
        Vector3 originJoystickVec = arrowRotate.up; // 화살표 방향 벡터

        float angle = Vector3.Angle(currentJoystickVec, originJoystickVec);
        int sign = (Vector3.Cross(currentJoystickVec, originJoystickVec).z > 0) ? -1 : 1;
        // angle: 현재 바라보고 있는 벡터와, 조이스틱 방향 벡터 사이의 각도
        // sign: Player가 바라보는 방향 기준으로, 왼쪽:+ 오른쪽:-

        //코루틴이 실행중이면 실행 중인 코루틴 중단 후 코루틴 실행 (코루틴이 한 개만 존재하도록) 
        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
        }
        runningCoroutine = StartCoroutine(RotateAngle(angle, sign));
    }
    

    /**************************************
    * @함수명: RotateAngle(float angle, int sign)
    * @작성자: zeli
    * @입력: angle(현재 바라보고 있는 방향과 클릭한 방향의 사이각도), sign(각도 부호)
    * @출력: IEnumerator
    * @설명: 일정한 속도(rotateSpeed)로 회전.
    *        터치가 발생하면 호출됨
    */
    IEnumerator RotateAngle(float angle, int sign)
    {
        if (isReversed)
        {
            angle = 180 - angle;
            sign *= -1;
        }
        
        float mod = angle % rotateSpeed;
        for (float i = mod; i < angle; i += rotateSpeed)
        {
            arrowRotate.Rotate(0, 0, sign * rotateSpeed);
            yield return new WaitForSeconds(0.01f); // 1프레임 대기
        }
        
        arrowRotate.Rotate(0, 0, sign * mod); // 남은 각도 회전
    }


    /**************************************
    * @함수명: TakeDamage
    * @작성자: zeli
    * @입력: damage, isDragon (overload)
    * @출력: void
    * @설명: 적들로부터 받은 데미지를 받은 후 계산
    *        hp(Liver)가 없으면 OnDead 처리
    *        피격효과 실행
    */
    public void TakeDamage(int damage)
    {
        if (isPlayerBeatRunning) return;

        if (HasShield) 
        {
            ShieldEnd();
            return;
        }

        if (isFevered)
        {
            return;
        }

        if (PlayerInformation.isVibrationOn == true)
        {
            Vibration.Vibrate(800);
        }

        if(hp > 1)
        {
            StartCoroutine(PlayerBeat());
            StartCoroutine(ShowDecreaseHpText(damage));
        }

        hp -= damage;
        GameManager.instance.UpdateLiverCountText();
        if(hp <= 0)
        {
            OnDead();
        }
    }

    public void TakeDamage(bool isDragon)
    {
        if (HasShield)
        {
            ShieldEnd();
            return;
        }

        if (isFevered)
        {
            return;
        }

        if (PlayerInformation.isVibrationOn == true)
        {
            Vibration.Vibrate(800);
        }

        if (isDragon)
        {
            hp = 0;
            GameManager.instance.UpdateLiverCountText();
            OnDead();
        }
    }



    /**************************************
    * @함수명: PlayerBeat
    * @작성자: zeli
    * @입력: void
    * @출력: IEnumertor
    * @설명: Player가 피격될 시 깜빡임 효과
    */
    IEnumerator PlayerBeat()
    {
        isPlayerBeatRunning = true;

        playerCollider.enabled = false;
        int count = 0;
        
        while(count < 10)
        {
            if(count%2 == 0)
            {
                playerSpriteRenderer.color = new Color32(255, 255, 255, 90);
            }
            else
                playerSpriteRenderer.color = new Color32(255, 255, 255, 180);

            yield return new WaitForSeconds(0.16f);
            count += 1;
        }

        playerSpriteRenderer.color = new Color32(255, 255, 255, 255);
        playerCollider.enabled = true;

        isPlayerBeatRunning = false; 
    }


    /**************************************
    * @함수명: OnDead
    * @작성자: zeli, malbong
    * @입력: void
    * @출력: void
    * @설명: hp(Liver)가 없으면 발동
    */
    public void OnDead()
    {
        if (isAlive == false) return; // dont re died

        isAlive = false; // died

        GetComponent<BoxCollider2D>().enabled = false;

        cam = Camera.main;
        cam.GetComponent<AudioListener>().enabled = true;
        cam.transform.parent = null;

        ParticleSystem instance = Instantiate(onDeadParticle, transform.position, Quaternion.identity);
        instance.Play();
        instance.GetComponent<AudioSource>().Play();
        Destroy(instance.gameObject, instance.main.startLifetime.constant);

        KeepOnTrail();

        GameManager.instance.ReportScore(GameManager.instance.totalScore);

        transform.gameObject.SetActive(false);

        GameManager.instance.PlayerDied();
    }


    /**************************************
    * @함수명: FeverTime
    * @작성자: zeli
    * @입력: breath
    * @출력: void
    * @설명: 무적 활성화
    *        무적 애니메이션 재생
    *        Fever에서 사용함
    */
    public void FeverTime()
    {
        isFevered = true;

        fever.SetActive(true);

        //feverAni.SetBool("feverAnimation", true);
        //boatAnim.SetBool("feverAnimation", true);
    }


    /**************************************
    * @함수명: EndFeverTime
    * @작성자: zeli
    * @입력: breath
    * @출력: void
    * @설명: 무적 비활성화
    *        무적 애니메이션 정지
    *        Fever에서 사용
    */
    public void EndFeverTime()
    {
        isFevered = false;

        fever.SetActive(false);

        //feverAni.SetBool("feverAnimation", false);
        //boatAnim.SetBool("feverAnimation", false);
    }


    private void KeepOnTrail()
    {
        Transform rotator = transform.Find("Rotator");
        if (rotator != null)
        {
            Transform trailWaveLeft = rotator.Find("Trail Wave Left");
            Transform trailWaveRight = rotator.Find("Trail Wave Right");
            if (trailWaveLeft != null && trailWaveRight != null)
            {
                trailWaveLeft.parent = null;
                trailWaveRight.parent = null;

                ParticleSystem trailWaveLeftParticle = trailWaveLeft.GetComponent<ParticleSystem>();
                ParticleSystem trailWaveRightParticle = trailWaveRight.GetComponent<ParticleSystem>();

                trailWaveLeftParticle.Stop();
                trailWaveRightParticle.Stop();

                Destroy(trailWaveLeftParticle.gameObject, trailWaveLeftParticle.main.duration);
                Destroy(trailWaveRightParticle.gameObject, trailWaveLeftParticle.main.duration);
            }
            else
            {
                Debug.Log("Not Find Trail Wave");
            }
        }
        else
        {
            Debug.Log("Not Find Rotator");
        }
    }


    public void ShieldStart()
    {
        HasShield = true;
        shield.SetActive(true);
    }


    public void ShieldEnd()
    {
        HasShield = false;
        shield.GetComponent<Animator>().SetTrigger("Broken");
        ItemManager.instance.ShieldBrokenPlay();
        StartCoroutine(ShieldEndWait());
    }


    IEnumerator ShieldEndWait()
    {
        yield return new WaitForSeconds(0.2f);
        shield.GetComponent<SpriteRenderer>().sprite = shieldSprite;
        shield.SetActive(false);
    }


    public IEnumerator ShowDecreaseHpText(int damage)
    {
        decreaseHpText.text = "-" + damage;
        decreaseHpText.gameObject.SetActive(true);
        decreaseHpImage.gameObject.SetActive(true);

        Color tmpColor = decreaseHpText.color;

        Vector3 tmpPosition = decreaseHpImage.gameObject.transform.localPosition;

        while (true)
        {
            tmpColor.a -= Time.deltaTime;
            tmpColor.a -= Time.deltaTime;

            decreaseHpText.color = tmpColor;
            decreaseHpImage.color = tmpColor;

            decreaseHpImage.gameObject.transform.Translate(0, 0.005f, 0);
            if (decreaseHpText.color.a <= 0) break;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        decreaseHpImage.gameObject.SetActive(false);
        decreaseHpText.gameObject.SetActive(false);

        tmpColor.a = 1.0f;
        decreaseHpText.color = tmpColor;
        decreaseHpImage.color = tmpColor;

        decreaseHpImage.gameObject.transform.localPosition = tmpPosition;
    }


    public void UpdateCustom()
    {
        // boat
        switch (PlayerInformation.customs[0])
        {
            case 0:
                boatSprite.sprite = boatSprites[0];
                break;
            case 1:
                boatSprite.sprite = boatSprites[1];
                break;
            case 2:
                boatSprite.sprite = boatSprites[2];
                break;
            case 3:
                boatSprite.sprite = boatSprites[3];
                break;
            case 4:
                boatSprite.sprite = boatSprites[4];
                break;
        }
        
        // face
        switch (PlayerInformation.customs[1])
        {
            case 0:
                faceSprite.sprite = faceSprites[0];
                break;
            case 1:
                faceSprite.sprite = faceSprites[1];
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
        
        // wave
        switch (PlayerInformation.customs[2])
        {
            case 0:
                waveLeftParticle.trailMaterial = waveMaterials[0];
                waveRightParticle.trailMaterial = waveMaterials[0];
                break;
            case 1:
                waveLeftParticle.trailMaterial = waveMaterials[1];
                waveRightParticle.trailMaterial = waveMaterials[1];
                break;
            case 2:
                waveLeftParticle.trailMaterial = waveMaterials[2];
                waveRightParticle.trailMaterial = waveMaterials[2];
                break;
            case 3:
                waveLeftParticle.trailMaterial = waveMaterials[3];
                waveRightParticle.trailMaterial = waveMaterials[3];
                break;
            case 4:
                waveLeftParticle.trailMaterial = waveMaterials[4];
                waveRightParticle.trailMaterial = waveMaterials[4];
                break;
        }
    }


    private void ApplyCustomAbility()
    {   
        //check boat
        switch (PlayerInformation.customs[0])
        {
            case 0: //default
                break;

            case 1: // rotate + 1
                rotateSpeed += 1f;
                break;

            case 2: // speed + 1 & rotate + 1
                speed += 0.75f;
                rotateSpeed += 1f;
                break;

            case 3: // speed + 3
                speed += 2.25f;
                break;

            case 4: // + speed + 1 & rotate + 3 + shooting
                speed += 0.75f;
                rotateSpeed += 3;

                BulletShooter.SetActive(true);
                break;
        }
        //check face
        switch (PlayerInformation.customs[1])
        {
            case 0: //default
                break;

            case 1: //shield
                ShieldStart();
                break;

            case 2: //shield + liver
                ShieldStart();

                maxHp = 4;
                hp = maxHp;
                GameManager.instance.UpdateLiverCountText();
                break;

            case 3: //liver-1 + shield + soul x 2
                maxHp = 2;
                hp = maxHp;
                GameManager.instance.UpdateLiverCountText();
                
                ShieldStart();

                soulLucky = 2; //200 %
                
                //1. 변수 Player.instance.doubleSoul = true; 
                //2. soul쪽에서 PlayerInformation.customs[1] == 3 -> 어디에서 관리하는지 모름
                break;

            case 4: //shield + liver 4 + item spawn
                ShieldStart();
                
                maxHp = 4;
                hp = maxHp;
                GameManager.instance.UpdateLiverCountText();

                soulLucky = 1.5f;
                
                SpawnManager.instance.itemSpawnTime = 10f / 3f;
                break;
        }
    }


    private IEnumerator TriggerConfusion()
    {
        isTriggerConfusionRunning = true;

        isReversed = true;

        confusionRotator.SetActive(true);

        float secondsUnit = 0;
        while (true)
        {
            confusionRotator.transform.Rotate(0, 0, 10); //효과
            for (int i = 0; i < 3; i++) confusionRotator.transform.GetChild(i).rotation = Quaternion.identity; //자식은 고정

            yield return new WaitForSeconds(Time.deltaTime);
            secondsUnit += Time.deltaTime;
            if (secondsUnit >= 3.0f) break;
        }

        confusionRotator.SetActive(false);

        isReversed = false;

        isTriggerConfusionRunning = false;
    }


    public void ConfusePlayer()
    {
        if (isTriggerConfusionRunning) StopCoroutine("TriggerConfusion");

        StartCoroutine("TriggerConfusion");
    }


    private IEnumerator TriggerBlinding()
    {
        isTriggerBlindingRunning = true;

        blindPanel.SetActive(true);
        Image blindImage = blindPanel.GetComponent<Image>();
        Color tmpColor = blindImage.color;

        float secondsUnit = 0;
        while (true)
        {
            if (blindImage.color.a < 1.0f)
            {
                tmpColor.a += 0.1f;
                blindImage.color = tmpColor;
            }
            yield return new WaitForSeconds(Time.deltaTime);
            secondsUnit += Time.deltaTime;
            if (secondsUnit >= 3.0f) break;
        }

        while (true)
        {
            tmpColor.a -= 0.1f;
            blindImage.color = tmpColor;
            yield return new WaitForSeconds(Time.deltaTime);
            if (blindImage.color.a <= 0) break;
        }
        
        blindPanel.SetActive(false);

        isTriggerBlindingRunning = false;
    }


    public void BlindPlayer()
    {
        if (isTriggerBlindingRunning) StopCoroutine("TriggerBlinding");

        StartCoroutine("TriggerBlinding");
    }
}
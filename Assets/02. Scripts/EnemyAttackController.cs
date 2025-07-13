using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;


//적이 공격을 하기 위한 시스템.
public class EnemyAttackController : MonoBehaviour
{
    //GameManager gameManager;
    EnemyAttackWarning attackWarning;

    public ArrowSystem arrowSystem;

    private int atkDirection = 0; //0일시 좌, 1일시 우
    [SerializeField] private GameObject EnemyBombPrefab;    //폭탄 프리펩   //현재 폭탄 이펙트 없어서 사용 X
    [SerializeField] private float attackPrePareTimeMin;    //적 공격 준비시간 최소
    [SerializeField] private float attackPrePareTimeMax;    //적 공격 준비시간 최대
    private float attackPrepareTime;    //적이 공격준비하는 시간, 랜덤으로 지정


    [SerializeField] private float attackComingTime;  //적의 공격이 날아오는 시간
    private bool isParryAble = false;  //공격을 패링할 수 있음. True일시 패링 활성화
    [SerializeField] private float parryTime = 1f;  //패링 시간 지정

    private bool isTryParry = false; //패링 시도 유무. 사용 X 사이클 생김
    public bool isParrying = false;
    private bool isParrySuccess = false; //패링 성공 유무

    public GameObject parryEffect; //패링 성공시 나오는 효과
    public GameObject parryEffect2;
    public GameObject parryFailEffect; //패링 실패시 나오는 효과
    public GameObject parryFailEffect2; //패링 실패시 나오는 효과

    void Start()
    {
        //gameManager = GetComponent<GameManager>();
        attackWarning = GetComponent<EnemyAttackWarning>();
        //StartCoroutine(EnemyAttack());
    }

    public IEnumerator EnemyAttack()
    {
        while (true)
        {
            atkDirection = Random.Range(0, 2);  //0은 좌, 1은 우;
            isParrySuccess = false; //초기화

            attackPrepareTime = Random.Range(attackPrePareTimeMin, attackPrePareTimeMax);
            Debug.Log("준비시간: " + attackPrepareTime);

            Debug.Log("공격 준비");
            yield return new WaitForSeconds(attackPrepareTime);

            yield return StartCoroutine(EnemyAttackDirection(atkDirection));//좌 or 공격

            //패링 성공 유무에 따라서 공격을 입을지, 안 입을지 판단.
            if (!isParrySuccess)
            {
                Debug.Log("패링 실패");
                if (atkDirection == 0)
                {
                    parryFailEffect.GetComponent<Image>().enabled = true;
                }
                else
                {
                    parryFailEffect2.GetComponent<Image>().enabled = true;
                }
                SoundManager.Instance.PlaySound(8);
                GameManager.Instance.TakeDamage(Stage.Instance.GetStageData().enemyBombDamage);

                yield return new WaitForSeconds(1f);

                if (atkDirection == 0)
                {
                    parryFailEffect.GetComponent<Image>().enabled = false;
                }
                else
                {
                    parryFailEffect2.GetComponent<Image>().enabled = false;
                }
                //플레이어에게 피해를 주는 효과 메서드
            }
        }
    }

    public void StopAttack()
    {
        StopCoroutine(EnemyAttackDirection(atkDirection));
    }
    /// <summary>
    /// 적의 공격이 좌와 우로 나누어질때를 구분함.
    /// </summary>
    /// <param name="atkDir">0일시 좌, 1일시 우,</param>
    /// <returns></returns>
    IEnumerator EnemyAttackDirection(int atkDir)
    {
        if (atkDir == 0)
        {
            Debug.Log("좌 공격 시작");
            //적의 공격이 좌에서 올 경우
            //+)여기에 좌에서 오는 효과 적용
            attackWarning.WarnEffect(0, attackComingTime);
        }
        else
        {
            Debug.Log("우 공격 시작");
            //적의 공격이 우에서 올 경우
            //+)여기에 우에서 오는 효과 적용
            attackWarning.WarnEffect(1, attackComingTime);
        }
        //여기다가 UI상에서 공격이 날라가는 메서드 적용

        yield return new WaitForSeconds(attackComingTime);
        Debug.Log("공격이 바로 앞까지 옴");
        attackWarning.WarnParry(atkDir, parryTime);
        isParryAble = true;
        yield return new WaitForSeconds(parryTime);
        isParryAble = false;

    }

    /// <summary>
    /// 적의 공격을 패링하는 메서드. 입력키에 집어넣어서 사용
    /// </summary>
    /// <param name="parryDir">0일시 좌 패링, 1일시 우 패링,</param>
    public void ParryEnemyAttack(int parryDir)
    {
        //패링 가능할때만 패링할수 있음. 
        if (isParryAble && !isParrySuccess)
        {
            if (parryDir == 0)
            {
                if (atkDirection == 0)
                {
                    Debug.Log("좌 패링 성공");
                    arrowSystem.Anim.enabled = true;
                    arrowSystem.Anim.SetTrigger("LeftParrying");
                    isParrying = true;
                    StartCoroutine(IsParrying());
                    isParrySuccess = true;
                }
                else
                {
                    Debug.Log("우 패링 실패");
                    isParrySuccess = false;
                }
                TryParry();
                isParryAble = false;

            }
            if (parryDir == 1)
            {
                if (atkDirection == 1)
                {
                    Debug.Log("우 패링 성공");
                    arrowSystem.Anim.enabled = true;
                    arrowSystem.Anim.SetTrigger("RightParrying");
                    isParrying = true; 
                    StartCoroutine(IsParrying());
                    isParrySuccess = true;
                }
                else
                {
                    Debug.Log("좌 패링 실패");
                    isParrySuccess = false;
                }
                TryParry();
                isParryAble = false;
            }
        }
    }

    IEnumerator IsParrying()
    {
        arrowSystem.throwBackGround.SetActive(true);
        arrowSystem.arrowTimer.gameObject.SetActive(false);
        arrowSystem.arrowParent.gameObject.SetActive(false);
        arrowSystem.arrowBackground.SetActive(false);
        arrowSystem.createBread.SetActive(false);
        yield return new WaitForSeconds(1f);
        if (atkDirection == 0)
        {
            parryEffect.GetComponent<Image>().enabled = true;
            parryEffect.GetComponent<Animator>().enabled = true;
        }
        else
        {
            parryEffect2.GetComponent<Image>().enabled = true;
            parryEffect2.GetComponent<Animator>().enabled = true;
        }
        SoundManager.Instance.PlaySound(6);
        Stage.Instance.TakeDamage(GameManager.Instance.PlayerParryDamage + GameManager.Instance.Combo);
        GameManager.Instance.GetComponent<UIManager>().enemyHpText.text = Stage.Instance.GetStageData().enemyHp.ToString();
        yield return new WaitForSeconds(1f);
        if (Stage.Instance.GetStageData().enemyHp > 0)
        {         
            arrowSystem.throwBackGround.SetActive(false);
            arrowSystem.arrowParent.gameObject.SetActive(true);
            arrowSystem.arrowTimer.gameObject.SetActive(true);
            arrowSystem.arrowBackground.SetActive(true);
            parryEffect.GetComponent<Image>().enabled = false;
            parryEffect2.GetComponent<Image>().enabled = false;
            parryEffect.GetComponent<Animator>().enabled = false;
            parryEffect2.GetComponent<Animator>().enabled = false;
        }
        else
        {
            parryEffect.GetComponent<Image>().enabled = false;
            parryEffect2.GetComponent<Image>().enabled = false;
            parryEffect.GetComponent<Animator>().enabled = false;
            parryEffect2.GetComponent<Animator>().enabled = false;
            arrowSystem.enemyDieF();
            StopAllCoroutines();
        }
        isParrying = false;
    }

    /// <summary>
    /// 패링 시도시 패링 경고 효과 끊기
    /// </summary>
    /// <param name="isParry">패리했는지 유무</param>
    private void TryParry()
    {
        attackWarning.isTryParry = true;
    }

}

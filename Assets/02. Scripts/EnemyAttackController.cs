using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.XR;


//적이 공격을 하기 위한 시스템.
public class EnemyAttackController : MonoBehaviour
{
    EnemyAttackWarning attackWarning;

    public ArrowSystem arrowSystem;

    private int atkDirection = 0; //0일시 좌, 1일시 우
    [SerializeField] private GameObject EnemyBombPrefab;    //폭탄 프리펩
    [SerializeField] private float attackPrePareTimeMin;    //적 공격 준비시간 최소
    [SerializeField] private float attackPrePareTimeMax;    //적 공격 준비시간 최대
    private float attackPrepareTime;    //적이 공격준비하는 시간, 랜덤으로 지정


    [SerializeField] private float attackComingTime;  //적의 공격이 날아오는 시간
    private bool isParryAble = false;  //공격을 패링할 수 있음. True일시 패링 활성화
    [SerializeField] private float parryTime = 1f;  //패링 시간 지정

    private bool isTryParry = false; //패링 시도 유무(패링 효과 관리용)
    public bool isParrying = false;
    private bool isParrySuccess = false; //패링 성공 유무

    void Start()
    {
        attackWarning = GetComponent<EnemyAttackWarning>();
        StartCoroutine(EnemyAttack());
    }

    IEnumerator EnemyAttack()
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
                //플레이어에게 피해를 주는 효과 메서드
            }
        }
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
    /// 적의 공격을 패링하는 메서드.
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
                    isParryAble = true;
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
        yield return new WaitForSeconds(2f);
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

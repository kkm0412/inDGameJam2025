using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;


//적이 공격을 하기 위한 시스템.
public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] private GameObject EnemyBombPrefab;    //폭탄 프리펩
    [SerializeField] private float attackPrePareTimeMin;    //적 공격 준비시간 최소
    [SerializeField] private float attackPrePareTimeMax;    //적 공격 준비시간 최대
    private float attackPrepareTime;    //적이 공격준비하는 시간, 랜덤으로 지정


    [SerializeField] private float attackComingTime;  //적의 공격이 날아오는 시간
    private bool isParryAble = false;  //공격을 패링할 수 있음. True일시 패링 활성화
    [SerializeField] private float parryTime = 1f;  //패링 시간 지정
    private bool isParrySuccess = false; //패링 성공 유무

    void Start()
    {
        StartCoroutine(EnemyAttack());
    }

    IEnumerator EnemyAttack()
    {
        while (true)
        {
            isParrySuccess = false; //초기화

            attackPrepareTime = Random.Range(attackPrePareTimeMin, attackPrePareTimeMax);
            Debug.Log("공격 준비");
            yield return new WaitForSeconds(attackPrepareTime);

            Debug.Log("공격 시작");
            //여기다가 UI상에서 공격이 날라가는 메서드 적용
            GameObject enemyBomb = Instantiate(EnemyBombPrefab);
            enemyBomb.GetComponent<EnemyBomb>().SetTime(attackComingTime, parryTime);
            //
            yield return new WaitForSeconds(attackComingTime);

            Debug.Log("공격이 바로 앞까지 옴");
            isParryAble = true;
            yield return new WaitForSeconds(parryTime);
            isParryAble = false;

            //패링 성공 유무에 따라서 공격을 입을지, 안 입을지 판단.
            if (!isParrySuccess)
            {
                Debug.Log("패링 실패");
                //플레이어에게 피해를 주는 효과 메서드
            }
        }
    }
    /// <summary>
    /// 적 공격 패링하는 메서드, 컨트롤 클래스를 패링 키에다가 넣으면 됨.
    /// </summary>
    public void ParryEnemyAttack()
    {
        //패링 가능할때만 패링할수 있음. 
        if (isParryAble && !isParrySuccess)
        {
            isParrySuccess = true;
            Debug.Log("패링 성공");
            //패링 성공시 효과 적용 메서드
        }
    }
    //사용법
    //
    // EnemyAttackController enemycontroller;
    // enemycontroller = this.GetComponent<EnemyAttackController>();
    // if (Input.GetMouseButtonDown(0)) //키 입력
    // {
    //     enemycontroller.ParryEnemyAttack();
    // }

}

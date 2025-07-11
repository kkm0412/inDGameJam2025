using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : MonoBehaviour
{
    //폭탄에다가 리포지트리로 넣어주세요.

    private float attackingComingTime = 1f;   //폭탄 생성시
    [SerializeField] private float maxScaleSize = 2f;   // 커지는 최고 크기 설정
    private float growSize;
    private Vector3 initialScale;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("공격 오는 시간" + attackingComingTime);
        Debug.Log("커지는 크기" + maxScaleSize);
        //날라가는 시간이 최대가 될때 최대 크기가 되게 하는 함수
        //
        initialScale = this.transform.localScale;   //기본 스케일 저장

        growSize = maxScaleSize / attackingComingTime;

        StartCoroutine(BombExplode());
        Debug.Log("growTime" + growSize);
    }

    private void Update()
    {
        this.transform.localScale += initialScale * growSize * Time.deltaTime;
    }

    /// <summary>
    /// 폭탄 다가오는 시간 설정. EnemyAttackControl에서 객체 생성 후 사용.
    /// </summary>
    public void SetTime(float comingTime, float parryTime)
    {
        attackingComingTime = comingTime + parryTime;
        //+) 패링시간동안 색깔 변환등의 효과 적용
    }

    IEnumerator BombExplode()
    {
        yield return new WaitForSeconds(attackingComingTime);
        Destroy(this.gameObject);
    }
}
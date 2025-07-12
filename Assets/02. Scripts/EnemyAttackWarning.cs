using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttackWarning : MonoBehaviour
{
    [Tooltip("0은 왼쪽, 1은 오른쪽. 2개로 제한")]
    [SerializeField] private Image[] warning = new Image[2];
    [SerializeField] private Image[] warningParry = new Image[2];

    [SerializeField] private GameObject[] criticalWarningEffect;

    public bool isTryParry = false;

    void Start()
    {
        warning[0] = warning[0].GetComponent<Image>();
        warning[1] = warning[1].GetComponent<Image>();
        warning[0] = warning[0].GetComponentInChildren<Image>();
        warning[1] = warning[1].GetComponentInChildren<Image>();
        warningParry[0] = warningParry[0].GetComponent<Image>();
        warningParry[0] = warningParry[0].GetComponentInChildren<Image>();
        warningParry[1] = warningParry[1].GetComponent<Image>();
        warningParry[1] = warningParry[1].GetComponentInChildren<Image>();
        warningParry[0].enabled = false;
        warningParry[1].enabled = false;

    }
    /// <summary>
    /// 적 공격 경고 효과 적용
    /// </summary>
    /// <param name="warnDir"> 0일시 좌, 1일시 우</param>
    /// <param name="attackComingTime"> 경고 시간</param>
    public void WarnEffect(int warnDir, float attackComingTime)
    {
        float warnTime = (attackComingTime - 0.1f) / 4f;
        StartCoroutine(WarnEffectBlink(warnDir, warnTime));
    }

    /// <summary>
    /// 패링 효과 적용
    /// </summary>
    /// <param name="warnDir">경고 위치 좌: 0, 우: 1</param>
    /// <param name="parryTime">패링 시간</param>
    public void WarnParry(int warnDir, float parryTime)
    {
        StartCoroutine(WarnEffectDanger(warnDir, parryTime));
    }

    /// <summary>
    /// 적 공격 경고 이펙트
    /// </summary>
    /// <param name="warnDir">공격 방향</param>
    /// <param name="warnTime">총 경고 시간</param>
    /// <returns></returns>
    /// 


    private void ShowCriticalWarningEffect(int dir)
    {
        if (dir < 0 || dir >= criticalWarningEffect.Length)
        {
            Debug.LogWarning($"[경고] 잘못된 인덱스: {dir}");
            return;
        }

        criticalWarningEffect[dir].SetActive(true);
        Debug.Log($"[위험] 경고 방향 {dir}에서 최대 밝기에 도달!");

        // 일정 시간 후 자동 비활성화
        StartCoroutine(HideCriticalWarningEffect(dir, 4.0f));
    }

    private IEnumerator HideCriticalWarningEffect(int dir, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (dir < 0 || dir >= criticalWarningEffect.Length)
        {
            Debug.LogWarning($"[경고] 잘못된 인덱스: {dir}");
            yield break;
        }

        criticalWarningEffect[dir].SetActive(false);
    }

    IEnumerator WarnEffectBlink(int warnDir, float warnTime)
    {
        Color color = warning[warnDir].color;
        float alphaMax = 0.6f;
        float warnSensitivity = 1f / (warnTime - 0.1f) * alphaMax;
        Debug.Log($"[WarnEffect] 민감도: {warnSensitivity}");

        float elapsed = 0f;
        float triggerTime = 1.5f; // 깜빡이 시작 후 0.3초 뒤에 이펙트 실행
        bool hasShownEffect = false;

        // 1단계: 점점 밝아짐
        while (color.a < alphaMax)
        {
            elapsed += Time.deltaTime;

            if (elapsed >= triggerTime && !hasShownEffect)
            {
                ShowCriticalWarningEffect(warnDir);
                hasShownEffect = true;
            }

            color.a += warnSensitivity * Time.deltaTime;
            warning[warnDir].color = color;
            yield return null;
        }

        // 2단계: 점점 어두워짐
        while (color.a > 0)
        {
            color.a -= warnSensitivity * Time.deltaTime;
            warning[warnDir].color = color;
            yield return null;
        }

        // 3단계: 다시 점점 밝아짐
        while (color.a < alphaMax)
        {
            color.a += warnSensitivity * Time.deltaTime;
            warning[warnDir].color = color;
            yield return null;
        }

        // 4단계: 다시 점점 어두워짐
        while (color.a > 0)
        {
            color.a -= warnSensitivity * Time.deltaTime;
            warning[warnDir].color = color;
            yield return null;
        }

        yield return null;
    }


    /*
        IEnumerator WarnEffectBlink(int warnDir, float warnTime)
        {
            Color color = warning[warnDir].color;
            float alphaMax = 0.6f;
            float warnSensitivity = 1f / (warnTime - 0.1f) * alphaMax;
            Debug.Log(warnSensitivity);

            while (color.a < alphaMax)
            {
                color.a += warnSensitivity * Time.deltaTime;
                warning[warnDir].color = color;
                yield return null;
            }
            while (color.a > 0)
            {
                color.a -= warnSensitivity * Time.deltaTime;
                warning[warnDir].color = color;
                yield return null;
            }
            while (color.a < alphaMax)
            {
                color.a += warnSensitivity * Time.deltaTime;
                warning[warnDir].color = color;
                yield return null;
            }
            while (color.a > 0)
            {
                color.a -= warnSensitivity * Time.deltaTime;
                warning[warnDir].color = color;
                yield return null;
            }
            yield return null;

        }

        */



    /// <summary>
    /// 패링 가능시 이펙트
    /// </summary>
    /// <param name="warnDir">0: 좌, 1: 우</param>
    /// <param name="parryTime">패링 시간</param>
    /// <returns></returns>
    IEnumerator WarnEffectDanger(int warnDir, float parryTime)
    {
        float timer = 0f;   //패링 체크용 타이머
        Debug.Log("timer: " + timer + " parryTime: " + parryTime);
        warningParry[warnDir].enabled = true;

        Color color = warningParry[warnDir].color;
        color.a = 0.5f;
        warningParry[warnDir].color = color;
        criticalWarningEffect[warnDir].SetActive(true);
        while (timer < parryTime)
        {
            if (isTryParry == true)
            {
                Debug.Log("패링 시도함");
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        criticalWarningEffect[warnDir].SetActive(false);

        yield return null;
        color.a = 0;
        warningParry[warnDir].color = color;

        //초기화
        warningParry[warnDir].enabled = false;
        isTryParry = false;

    }
}

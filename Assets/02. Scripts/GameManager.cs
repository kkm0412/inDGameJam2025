using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static Action<int> OnHpChanged;
    private ArrowSystem arrowSystem;

    // 플레이어의 체력
    [SerializeField]
    private int playerHp = 100;
    private int PlayerHp => playerHp;

    // 플레이어 체력 회복 범위 (예: 1~3)
    [SerializeField]
    private int playerHeal = 1;
    private int PlayerHeal => playerHeal;

    [SerializeField]
    // 플레이어 폭탄의 데미지 범위 (예: 5~10)
    private int playerBombDamage = 5;
    private int PlayerBombDamage => playerBombDamage;

    // 폭탄 사용 쿨타임 (초)
    [SerializeField]
    private float playerBombCooldown = 10f;
    private float PlayerBombCooldown => playerBombCooldown;

    [SerializeField]
    private int combo = 0;
    public int Combo => combo;

    // 각 스테이지 제한 시간 (초)
    public float stageTimeLimit = 120f;
    public float leftStageTime { get; private set; }

    // 게임 시작 시의 스테이지 레벨 (0부터 시작 아님, 1부터 시작)
    public int nowStage = 2;

    public bool stageStart = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        combo = 0;
        leftStageTime = stageTimeLimit;
        arrowSystem = GetComponent<ArrowSystem>();
    }

    private void Start()
    {
        OnHpChanged?.Invoke(playerHp);
        arrowSystem.customer.gameObject.SetActive(false);
        StartCoroutine(GameStart());
    }
    // Update is called once per frame
    void Update()
    {
        if (stageStart)
        {
            leftStageTime -= Time.deltaTime;
            if (leftStageTime <= 0f)
            {
                stageStart = false;
                StageEnd();
            }
        }
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.stageStart = true;
        arrowSystem.customer.gameObject.SetActive(true);
        arrowSystem.StartArrowInput();
    }

    public void IncreCombo()
    {
        combo += 1;
    }

    public void ResetCombo()
    {
        combo = 0;
    }

    public void TakeDamage(int amount)
    {
        playerHp -= amount;
        OnHpChanged?.Invoke(playerHp);
        if (playerHp <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("게임 오버");
        arrowSystem.StopInput();
    }

    void StageEnd()
    {
        Debug.Log("스테이지 종료");
    }
}
